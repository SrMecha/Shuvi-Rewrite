using Discord;
using Shuvi.Classes.Extensions;
using Shuvi.Classes.Factories.CustomEmbed;
using Shuvi.Classes.Types.Characteristics.Bonuses;
using Shuvi.Classes.Types.Interaction;
using Shuvi.Interfaces.User;
using Shuvi.Services.StaticServices.Localization;
using System;

namespace Shuvi.CommandParts
{
    public static class CharacteristicsViewPart
    {
        private static readonly LocalizationLanguagePart _localizationPart = LocalizationService.Get("characteristicsViewPart");

        public static async Task Start(CustomInteractionContext context, IDatabaseUser dbUser, IUser user, bool canEdit = false)
        {
            var charLocalization = _localizationPart.Get(context.Language);
            var namesLocalization = LocalizationService.Get("names").Get(context.Language);
            var equipmentBonuses = dbUser.Equipment.GetBonuses();
            var allCharacteristics = new AllBonuses();
            allCharacteristics.Add(dbUser.Characteristics);
            allCharacteristics.Add(equipmentBonuses);
            var a = allCharacteristics.GetFullAttackDamage();
            var fullFightCharacteristics = new FightBonuses()
            {
                AttackDamage = MathF.Round(allCharacteristics.GetFullAttackDamage(), 1),
                AbilityPower = MathF.Round(allCharacteristics.GetFullAbilityPower(), 1),
                Armor = MathF.Round(allCharacteristics.GetFullArmor(), 1),
                MagicResistance = MathF.Round(allCharacteristics.GetFullMagicResistance(), 1),
                CriticalStrikeChance = MathF.Round(allCharacteristics.GetFullCriticalStrikeChance() * 100, 1),
                CriticalStrikeDamageMultiplier = MathF.Round(allCharacteristics.GetFullCriticalStrikeDamageMultiplier() * 100, 1),
                StrikeChance = MathF.Round(allCharacteristics.GetFullStrikeChance() * 100, 1),
                DodgeChance = MathF.Round(allCharacteristics.GetFullDodgeChance() * 100, 1)
            };
            var fightCharacteristics = new FightBonuses()
            {
                AttackDamage = dbUser.Characteristics.GetAttackDamage(),
                AbilityPower = dbUser.Characteristics.GetAbilityPower(),
                Armor = dbUser.Characteristics.GetArmor(),
                MagicResistance = dbUser.Characteristics.GetMagicResistance(),
                CriticalStrikeChance = dbUser.Characteristics.GetCriticalStrikeChance() * 100,
                CriticalStrikeDamageMultiplier = dbUser.Characteristics.GetCriticalStrikeDamageMultiplier() * 100,
                StrikeChance = dbUser.Characteristics.GetStrikeChance() * 100,
                DodgeChance = dbUser.Characteristics.GetDodgeChance() * 100
            };
            var fightBonuses = new FightBonuses()
            {
                AttackDamage = MathF.Round(fullFightCharacteristics.AttackDamage - fightCharacteristics.AttackDamage, 1),
                AbilityPower = MathF.Round(fullFightCharacteristics.AbilityPower - fightCharacteristics.AbilityPower, 1),
                Armor = MathF.Round(fullFightCharacteristics.Armor - fightCharacteristics.Armor, 1),
                MagicResistance = MathF.Round(fullFightCharacteristics.MagicResistance - fightCharacteristics.MagicResistance, 1),
                CriticalStrikeChance = MathF.Round(fullFightCharacteristics.CriticalStrikeChance - fightCharacteristics.CriticalStrikeChance, 1),
                CriticalStrikeDamageMultiplier = MathF.Round(fullFightCharacteristics.CriticalStrikeDamageMultiplier 
                - fightCharacteristics.CriticalStrikeDamageMultiplier, 1),
                StrikeChance = MathF.Round(fullFightCharacteristics.StrikeChance - fightCharacteristics.StrikeChance, 1),
                DodgeChance = MathF.Round(fullFightCharacteristics.DodgeChance - fightCharacteristics.DodgeChance, 1),
            };
            while (context.LastInteraction is not null)
            {
                var embed = EmbedFactory.CreateUserEmbed(dbUser) 
                    .WithAuthor(charLocalization.Get("Embed/Author").Format(user.Username), user.GetAvatarUrl())
                    .AddField(charLocalization.Get("Embed/Characteristics"),
                    $"**{namesLocalization.Get("Strength")}:** {allCharacteristics.Strength.WithBonus(equipmentBonuses.Strength)}\n" +
                    $"**{namesLocalization.Get("Agility")}:** {allCharacteristics.Agility.WithBonus(equipmentBonuses.Agility)}\n" +
                    $"**{namesLocalization.Get("Luck")}:** {allCharacteristics.Luck.WithBonus(equipmentBonuses.Luck)}\n" +
                    $"**{namesLocalization.Get("Intellect")}:** {allCharacteristics.Intellect.WithBonus(equipmentBonuses.Intellect)}\n" +
                    $"**{namesLocalization.Get("Endurance")}:** {allCharacteristics.Endurance.WithBonus(equipmentBonuses.Endurance)}\n" +
                    $"**{namesLocalization.Get("Health")}:** {dbUser.Characteristics.Health.GetCurrent()}/{dbUser.Characteristics.Health.Max}\n" +
                    $"**{namesLocalization.Get("Mana")}:** {dbUser.Characteristics.Mana.GetCurrent()}/{dbUser.Characteristics.Mana.Max}",
                    true)
                    .AddField(charLocalization.Get("Embed/Parameters"),
                    $"**{namesLocalization.Get("AttackDamage")}:** {fullFightCharacteristics.AttackDamage.WithBonus(fightBonuses.AttackDamage)}\n" +
                    $"**{namesLocalization.Get("AbilityPower")}:** {fullFightCharacteristics.AbilityPower.WithBonus(fightBonuses.AbilityPower)}\n" +
                    $"**{namesLocalization.Get("Armor")}:** {fullFightCharacteristics.Armor.WithBonus(fightBonuses.Armor)}\n" +
                    $"**{namesLocalization.Get("MagicResistance")}:** {fullFightCharacteristics.MagicResistance.WithBonus(fightBonuses.MagicResistance)}\n" +
                    $"**{namesLocalization.Get("CriticalStrikeChance")}:** {fullFightCharacteristics.CriticalStrikeChance
                    .WithBonusPercent(fightBonuses.CriticalStrikeChance)}\n" +
                    $"**{namesLocalization.Get("CriticalStrikeDamageMultiplier")}:** {fullFightCharacteristics.CriticalStrikeDamageMultiplier
                    .WithBonusPercent(fightBonuses.CriticalStrikeDamageMultiplier)}\n" +
                    $"**{namesLocalization.Get("StrikeChance")}:** {fullFightCharacteristics.StrikeChance.WithBonus(fightBonuses.StrikeChance)}\n" +
                    $"**{namesLocalization.Get("DodgeChance")}:** {fullFightCharacteristics.DodgeChance.WithBonus(fightBonuses.DodgeChance)}\n" +
                    $"",
                    true)
                    .Build();
                var components = UserProfilePart.GetProfileSelectMenus(context.Language, dbUser, canEdit);
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
    }
}
