using Discord;
using Discord.WebSocket;
using Shuvi.Classes.Extensions;
using Shuvi.Classes.Factories.CustomEmbed;
using Shuvi.Classes.Settings;
using Shuvi.Classes.Types.Interaction;
using Shuvi.Enums.Check;
using Shuvi.Enums.Localization;
using Shuvi.Enums.Rating;
using Shuvi.Enums.User;
using Shuvi.Interfaces.User;
using Shuvi.Services.StaticServices.Check;
using Shuvi.Services.StaticServices.Emoji;
using Shuvi.Services.StaticServices.Localization;

namespace Shuvi.CommandParts
{
    public static class UserProfilePart
    {
        private static readonly LocalizationLanguagePart _localizationPart = LocalizationService.Get("profilePart");

        public static async Task Start(CustomInteractionContext context, IDatabaseUser dbUser, IUser user, bool canEdit = false)
        {
            var errorLocalization = LocalizationService.Get("errorPart").Get(context.Language);
            await ProfileView(context, dbUser, user, canEdit);
            do
            {
                var interaction = (context.LastInteraction as SocketMessageComponent)!;
                switch (interaction.Data.CustomId)
                {
                    case "view":
                        switch (interaction.Data.Values.First())
                        {
                            case "profile":
                                await ProfileView(context, dbUser, user, canEdit);
                                break;
                            case "characteristics":
                                await CharacteristicsViewPart.Start(context, dbUser, user, canEdit);
                                break;
                            case "equipment":
                                await EquipmentViewPart.Start(context, dbUser, user, canEdit);
                                break;
                            case "location":
                                await LocationViewPart.Start(context, dbUser, user, canEdit);
                                break;
                            case "statistics":
                                await StatisticsViewPart.Start(context, dbUser, user, canEdit);
                                break;
                            case "inventory":
                                await InventoryPart.Start(context, dbUser, user, canEdit);
                                break;
                        }
                        break;
                    case "edit":
                        switch (interaction.Data.Values.First())
                        {
                            case "upgrade":
                                if (UserCheckService.IsUseCommand(TrackedCommand.Upgrade, dbUser.Id))
                                {
                                    await context.SendError(errorLocalization.Get("AlreadyUseCommand"), context.Language);
                                    return;
                                }
                                try
                                {
                                    UserCheckService.AddUserToCommand(TrackedCommand.Upgrade, dbUser.Id);
                                    await UpgradePart.Start(context, dbUser, false);
                                    UserCheckService.RemoveUserFromCommand(TrackedCommand.Upgrade, dbUser.Id);
                                }
                                catch
                                {
                                    UserCheckService.RemoveUserFromCommand(TrackedCommand.Upgrade, dbUser.Id);
                                    throw;
                                }
                                break;
                            case "customization":
                                await CustomizationPart.Start(context, dbUser);
                                break;
                            case "fightSettings":
                                await FightSettingsPart.Start(context, dbUser);
                                break;
                            case "premium":
                                break;
                            case "spell":
                                await SpellChangePart.Start(context, dbUser);
                                break;
                            case "skill":
                                await SkillChangePart.Start(context, dbUser);
                                break;
                            case "chooseProfession":
                                await ProfessionChoosePart.Start(context, dbUser);
                                break;
                        }
                        break;
                    default:
                        await ProfileView(context, dbUser, user, canEdit);
                        break;
                }
                if (context.LastInteraction is null)
                {
                    await context.CurrentMessage!.RemoveButtonsAsync();
                    return;
                }
            } while (context.LastInteraction is not null);
        }

        public static async Task ProfileView(CustomInteractionContext context, IDatabaseUser dbUser, IUser user, bool canEdit = false)
        {
            var errorLocalization = LocalizationService.Get("errorPart").Get(context.Language);
            while (context.LastInteraction is not null)
            {
                var profileLocalization = _localizationPart.Get(context.Language);
                var namesLocalization = LocalizationService.Get("names").Get(context.Language);
                var equipmentBonuses = dbUser.Equipment.GetBonuses();
                var embed = EmbedFactory.CreateUserEmbed(dbUser)
                    .WithAuthor(profileLocalization.Get("Embed/Profile/Author").Format(user.Username), user.GetAvatarUrl())
                    .WithDescription(GetBadges(dbUser.Customization.Badges))
                    .AddField(profileLocalization.Get("Embed/Profile/Rank").Format(dbUser.Rating.Rank.GetName()),
                    $"{profileLocalization.Get("Embed/Profile/Rating").Format($"{dbUser.Rating.Points}" +
                    $"{(dbUser.Rating.Rank.CanRankUp() ? "/" + (Rank)(dbUser.Rating.Rank + 1).GetNeedRating() : ' ')}")}\n" +
                    $"{profileLocalization.Get("Embed/Profile/Race").Format(namesLocalization.Get($"Race/{dbUser.Race.GetName()}"))}\n" +
                    $"{(dbUser.Subrace == UserSubrace.NoSubrace ? "" : $"{profileLocalization.Get("Embed/Profile/Subrace")
                    .Format(namesLocalization.Get($"subrace/{dbUser.Subrace.GetName()}"))}\n")}" +
                    $"{profileLocalization.Get("Embed/Profile/Profession").Format(namesLocalization.Get($"Profession/{dbUser.Profession.GetName()}"))}\n" +
                    $"{profileLocalization.Get("Embed/Profile/MagicType").Format(dbUser.MagicInfo.Info.GetName(context.Language))}\n" +
                    $"**{namesLocalization.Get("Gold")}:** {dbUser.Wallet.Gold} {EmojiService.Get("Gold")}\n" +
                    $"**{namesLocalization.Get("Dispoints")}:** {dbUser.Wallet.Dispoints} {EmojiService.Get("Dispoints")}",
                    true)
                    .AddField("** **",
                    $"**{namesLocalization.Get("Energy")}:** " +
                    $"[{GetEmojiBar(EmojiService.Get("EnergyFull"), EmojiService.Get("EnergyEmpty"), dbUser.Characteristics.Energy.GetCurrent(),
                    dbUser.Characteristics.Energy.Max, UserSettings.EnergyDisplayMax)}] " +
                    $"{dbUser.Characteristics.Energy.GetCurrent()}/{dbUser.Characteristics.Energy.Max}\n" +
                    $"{(dbUser.Characteristics.Energy.GetRemainingRegenTime() == 0 ? "\n" :
                    $"[{profileLocalization.Get("Embed/Profile/Recover")} <t:{dbUser.Characteristics.Energy.RegenTime}:R>]")}\n" +

                    $"**{namesLocalization.Get("Health")}:** " +
                    $"[{GetEmojiBar(EmojiService.Get("HealthFull"), EmojiService.Get("HealthEmpty"), dbUser.Characteristics.Health.GetCurrent(),
                    dbUser.Characteristics.Health.Max, UserSettings.HealthDisplayMax)}] " +
                    $"{dbUser.Characteristics.Health.GetCurrent()}/{dbUser.Characteristics.Health.Max}\n" +
                    $"{(dbUser.Characteristics.Health.GetRemainingRegenTime() == 0 ? "\n" :
                    $"[{profileLocalization.Get("Embed/Profile/Recover")} <t:{dbUser.Characteristics.Health.RegenTime}:R>]")}\n" +

                    $"**{namesLocalization.Get("Mana")}:** " +
                    $"[{GetEmojiBar(EmojiService.Get("MagicFull"), EmojiService.Get("MagicEmpty"), dbUser.Characteristics.Mana.GetCurrent(),
                    dbUser.Characteristics.Mana.Max, UserSettings.ManaDisplayMax)}] " +
                    $"{dbUser.Characteristics.Mana.GetCurrent()}/{dbUser.Characteristics.Mana.Max}\n" +
                    $"{(dbUser.Characteristics.Mana.GetRemainingRegenTime() == 0 ? "" :
                    $"[{profileLocalization.Get("Embed/Profile/Recover")} <t:{dbUser.Characteristics.Mana.RegenTime}:R>]")}")
                    .Build();
                var components = GetProfileSelectMenus(context.Language, dbUser, canEdit);
                await context.Interaction.ModifyOriginalResponseAsync(msg => { msg.Embed = embed; msg.Components = components; });
                await context.LastInteraction.TryDeferAsync();
                var interaction = await context.WaitForButton();
                if (interaction is null)
                {
                    await context.CurrentMessage!.RemoveButtonsAsync();
                    return;
                }
                switch (interaction.Data.CustomId)
                {
                    default:
                        return;
                }
            }
        }

        private static string GetBadges(UserBadges badges)
        {
            var result = new List<string>();
            IEmote? emote;
            foreach (var badge in badges.GetFlags())
            {
                emote = badge.GetBadgeEmoji();
                if (emote is not null)
                    result.Add(emote.ToString()!);
            }
            return result.Count < 1 ? string.Empty : $"-ˏˋ. {string.Join("", result)} ˊˎ-";
        }

        private static string GetEmojiBar(IEmote fullEmoji, IEmote emptyEmoji, int current, int max, int displayMax)
        {
            var fullEmojiCount = (current / (max / displayMax));
            return $"{fullEmoji.ToString()!.Multiple(fullEmojiCount)}" +
                $"{emptyEmoji.ToString()!.Multiple((displayMax - fullEmojiCount))}";
        }

        public static MessageComponent GetProfileSelectMenus(Language lang, IDatabaseUser dbUser, bool canEdit = false)
        {
            var profileLocalization = _localizationPart.Get(lang);
            if (canEdit)
            {

                var viewOptions = new List<SelectMenuOptionBuilder>()
                    {
                        new SelectMenuOptionBuilder(profileLocalization.Get("Select/View/Option/Profile"), "profile"),
                        new SelectMenuOptionBuilder(profileLocalization.Get("Select/View/Option/Characteristics"), "characteristics"),
                        new SelectMenuOptionBuilder(profileLocalization.Get("Select/View/Option/Equipment"), "equipment"),
                        new SelectMenuOptionBuilder(profileLocalization.Get("Select/View/Option/Location"), "location"),
                        new SelectMenuOptionBuilder(profileLocalization.Get("Select/View/Option/Inventory"), "inventory"),
                        new SelectMenuOptionBuilder(profileLocalization.Get("Select/View/Option/Statistics"), "statistics"),
                        //new SelectMenuOptionBuilder(profileLocalization.Get("select/view/option/pet"), "pet")
                    };
                var editOptions = new List<SelectMenuOptionBuilder>()
                    {
                        new SelectMenuOptionBuilder(profileLocalization.Get("Select/Edit/Option/Upgrade"), "upgrade"),
                        new SelectMenuOptionBuilder(profileLocalization.Get("Select/Edit/Option/Customization"), "customization"),
                        new SelectMenuOptionBuilder(profileLocalization.Get("Select/Edit/Option/FightSettings"), "fightSettings"),
                        new SelectMenuOptionBuilder(profileLocalization.Get("Select/Edit/Option/Spell"), "spell"),
                        dbUser.Profession == UserProfession.NoProfession ?
                        new SelectMenuOptionBuilder(profileLocalization.Get("Select/Edit/Option/ChooseProfession"), "chooseProfession") :
                        new SelectMenuOptionBuilder(profileLocalization.Get("Select/Edit/Option/Skill"), "skill")
                        // new SelectMenuOptionBuilder(profileLocalization.Get("select/edit/option/premium"), "premium")
                    };
                return new ComponentBuilder()
                    .WithSelectMenu("view", viewOptions, profileLocalization.Get("Select/View/Name"))
                    .WithSelectMenu("edit", editOptions, profileLocalization.Get("Select/Edit/Name"))
                    .Build();
            }
            else
            {
                var viewOptions = new List<SelectMenuOptionBuilder>()
                    {
                        new SelectMenuOptionBuilder(profileLocalization.Get("Select/View/Option/Profile"), "profile"),
                        new SelectMenuOptionBuilder(profileLocalization.Get("Select/View/Option/Characteristics"), "characteristics"),
                        new SelectMenuOptionBuilder(profileLocalization.Get("Select/View/Option/Equipment"), "equipment"),
                        new SelectMenuOptionBuilder(profileLocalization.Get("Select/View/Option/Location"), "location"),
                        new SelectMenuOptionBuilder(profileLocalization.Get("Select/View/Option/Statistics"), "statistics"),
                        //new SelectMenuOptionBuilder(profileLocalization.Get("Select/View/Option/Pet"), "pet")
                    };
                return new ComponentBuilder()
                    .WithSelectMenu("view", viewOptions, profileLocalization.Get("Select/View/Name"))
                    .Build();
            }
        }
    }
}
