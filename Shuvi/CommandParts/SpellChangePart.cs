using Discord;
using MongoDB.Driver;
using Shuvi.Classes.Data.User;
using Shuvi.Classes.Extensions;
using Shuvi.Classes.Factories.CustomEmbed;
using Shuvi.Classes.Factories.Spell;
using Shuvi.Classes.Types.Interaction;
using Shuvi.Enums.Localization;
using Shuvi.Interfaces.Spell;
using Shuvi.Interfaces.User;
using Shuvi.Services.StaticServices.Database;
using Shuvi.Services.StaticServices.Emoji;
using Shuvi.Services.StaticServices.Localization;

namespace Shuvi.CommandParts
{
    public static class SpellChangePart
    {
        private static readonly LocalizationLanguagePart _localizationPart = LocalizationService.Get("spellChangePart");

        public static async Task Start(CustomInteractionContext context, IDatabaseUser dbUser)
        {
            var changeLocalization = _localizationPart.Get(context.Language);
            var spells = SpellFactory.GetAvailableSpells(dbUser);
            var spellOptions = GetSpellOptions(spells, context.Language);
            var haveSpells = spellOptions.Count > 0;
            if (!haveSpells)
                spellOptions.Add(new("EMPTY", "EMPTY"));
            var arrow = 0;
            while (true)
            {
                var embed = EmbedFactory.CreateUserEmbed(context.User, dbUser)
                    .WithAuthor(changeLocalization.Get("embed/select/author"))
                    .WithDescription($"{changeLocalization.Get("embed/select/magicType").Format(
                        dbUser.MagicInfo.Info.GetName(context.Language))}\n" +
                        $"{changeLocalization.Get("embed/select/currentSpell").Format(dbUser.Spell.Info.GetName(context.Language))}\n\n" +
                        $"{(haveSpells ? $"{changeLocalization.Get("embed/select/availableSpells")}\n" +
                        $"{GetSpellsString(spells, arrow, context.Language)}" : changeLocalization.Get("embed/select/empty"))}")
                    .Build();
                var components = new ComponentBuilder()
                    .WithSelectMenu("choose", spellOptions, changeLocalization.Get("select/spell/name"), disabled: !haveSpells, row: 0)
                    .WithButton(changeLocalization.Get("btn/equip"), "equip", ButtonStyle.Success, disabled: !haveSpells, row: 1)
                    .WithButton(changeLocalization.Get("btn/info"), "info", ButtonStyle.Primary, disabled: !haveSpells, row: 1)
                    .WithButton(changeLocalization.Get("btn/back"), "back", ButtonStyle.Danger, row: 2)
                    .WithButton(changeLocalization.Get("btn/unequip"), "unequip", ButtonStyle.Secondary, disabled: !dbUser.Spell.HaveSpell(), row: 2)
                    .Build();
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
                    case "choose":
                        arrow = int.Parse(interaction.Data.Values.First());
                        break;
                    case "equip":
                        var spellName = spells[arrow].SpellName;
                        dbUser.Spell.SetSpell(spellName);
                        await UserDatabase.UpdateUser(dbUser.Id, new UpdateDefinitionBuilder<UserData>()
                            .Set(x => x.Spell, spellName));
                        break;
                    case "unequip":
                        dbUser.Spell.SetSpell(null);
                        await UserDatabase.UpdateUser(dbUser.Id, new UpdateDefinitionBuilder<UserData>()
                            .Set(x => x.Spell, null));
                        break;
                    case "info":
                        await SpellViewPart.Start(context, dbUser, spells[arrow]);
                        break;
                    case "back":
                        return;
                }
            }
        }

        public static string GetSpellsString(List<ISpell> spells, int arrow, Language lang)
        {
            var result = new List<string>();
            var row = 0;
            foreach (var spell in spells)
            {
                result.Add($"{(row == arrow ? EmojiService.Get("choosePoint") : string.Empty)} {spell.Info.GetName(lang)}");
                row++;
            }
            return result.Count > 0 ? string.Join("\n", result) : _localizationPart.Get(lang).Get("embed/select/empty");
        }

        public static List<SelectMenuOptionBuilder> GetSpellOptions(List<ISpell> spells, Language lang)
        {
            var result = new List<SelectMenuOptionBuilder>();
            var row = 0;
            foreach (var spell in spells)
            {
                var description = spell.Info.GetDescription(lang);
                if (description.Length > 70)
                    description = $"{description[..70]}...";
                result.Add(new(spell.Info.GetName(lang), row.ToString(), description));
                row++;
            }
            return result;
        }
    }
}
