﻿using Discord;
using Shuvi.Classes.Extensions;
using Shuvi.Classes.Factories.CustomEmbed;
using Shuvi.Classes.Settings;
using Shuvi.Classes.Types.Interaction;
using Shuvi.Enums.Rating;
using Shuvi.Enums.User;
using Shuvi.Interfaces.User;
using Shuvi.Services.StaticServices.Emoji;
using Shuvi.Services.StaticServices.Localization;

namespace Shuvi.CommandParts
{
    public static class ProfilePart
    {
        private static readonly LocalizationLanguagePart _localizationPart = LocalizationService.Get("profilePart");

        public static async Task Start(CustomInteractionContext context, IDatabaseUser dbUser)
        {
            var profileLocalization = _localizationPart.Get(dbUser.Language);
            var namesLocalization = LocalizationService.Get("names").Get(dbUser.Language);
            var equipmentBonuses = dbUser.Equipment.GetBonuses();
            var embed = EmbedFactory.CreateUserEmbed(context.User, dbUser)
                .WithAuthor(profileLocalization.Get("embed/profile/author"))
                .WithDescription(GetBadges(dbUser.Customization.Badges))
                .AddField(profileLocalization.Get("embed/profile/rank").Format(dbUser.Rating.Rank.GetName()),
                $"{profileLocalization.Get("embed/profile/rating").Format($"{dbUser.Rating.Points}" +
                $"{(dbUser.Rating.Rank.CanRankUp() ? "/" + (Rank)(dbUser.Rating.Rank + 1).GetNeedRating() : ' ')}")}\n" +
                $"{profileLocalization.Get("embed/profile/race").Format(namesLocalization.Get($"race/{dbUser.Race.GetLowerName()}"))}\n" +
                $"{(dbUser.Subrace == UserSubrace.NoSubrace ? "" : $"{profileLocalization.Get("embed/profile/subrace")
                .Format(namesLocalization.Get($"subrace/{dbUser.Subrace.GetLowerName()}"))}\n")}" +
                $"{profileLocalization.Get("embed/profile/profession").Format(namesLocalization.Get($"profession/{dbUser.Profession.GetLowerName()}"))}\n" +
                $"**{namesLocalization.Get("gold")}:** {dbUser.Wallet.Gold} {EmojiService.Get("gold")}\n" +
                $"**{namesLocalization.Get("dispoints")}:** {dbUser.Wallet.Dispoints} {EmojiService.Get("dispoints")}",
                true)
                .AddField(profileLocalization.Get("embed/profile/characteristics"),
                $"**{namesLocalization.Get("strength")}:** {dbUser.Characteristics.Strength.WithBonus(equipmentBonuses.Strength)}\n" +
                $"**{namesLocalization.Get("agility")}:** {dbUser.Characteristics.Agility.WithBonus(equipmentBonuses.Agility)}\n" +
                $"**{namesLocalization.Get("luck")}:** {dbUser.Characteristics.Luck.WithBonus(equipmentBonuses.Luck)}\n" +
                $"**{namesLocalization.Get("intellect")}:** {dbUser.Characteristics.Intellect.WithBonus(equipmentBonuses.Intellect)}\n" +
                $"**{namesLocalization.Get("endurance")}:** {dbUser.Characteristics.Endurance.WithBonus(equipmentBonuses.Endurance)}",
                true)
                .AddField("** **",
                $"**{namesLocalization.Get("enegry")}:** " +
                $"[{GetEmojiBar(EmojiService.Get("energyFull"), EmojiService.Get("energyEmpty"), dbUser.Characteristics.Energy.GetCurrent(), 
                dbUser.Characteristics.Energy.Max, UserSettings.EnergyDisplayMax)}] " +
                $"{dbUser.Characteristics.Energy.GetCurrent()}/{dbUser.Characteristics.Energy.Max}\n\n" +

                $"**{namesLocalization.Get("health")}:** " +
                $"[{GetEmojiBar(EmojiService.Get("healthFull"), EmojiService.Get("healthEmpty"), dbUser.Characteristics.Health.GetCurrent(), 
                dbUser.Characteristics.Health.Max, UserSettings.HealthDisplayMax)}] " +
                $"{dbUser.Characteristics.Health.GetCurrent()}/{dbUser.Characteristics.Health.Max}\n\n" +

                $"**{namesLocalization.Get("mana")}:** " +
                $"[{GetEmojiBar(EmojiService.Get("magicFull"), EmojiService.Get("magicEmpty"), dbUser.Characteristics.Mana.GetCurrent(), 
                dbUser.Characteristics.Mana.Max, UserSettings.ManaDisplayMax)}] " +
                $"{dbUser.Characteristics.Mana.GetCurrent()}/{dbUser.Characteristics.Mana.Max}")
                .Build();
            var components = new ComponentBuilder()
                .Build();
            await context.Interaction.ModifyOriginalResponseAsync(msg => { msg.Embed = embed; msg.Components = components; });
        }
        private static string GetBadges(UserBadges badges)
        {
            var result = new List<string>();
            IEmote? emote;
            foreach(var badge in badges.GetFlags())
            {
                emote = badge.GetBadgeEmoji();
                if (emote is not null)
                    result.Add(emote.ToString()!);
            }
            return string.Join("", result);
        }
        private static string GetEmojiBar(IEmote fullEmoji, IEmote emptyEmoji, int current, int max, int displayMax)
        {
            var fullEmojiCount = (byte)(current / (max / displayMax));
            return $"{fullEmoji.ToString()!.Multiple(fullEmojiCount)}" +
                $"{emptyEmoji.ToString()!.Multiple((byte)(displayMax - fullEmojiCount))}";
        }
    }
}
