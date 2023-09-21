using Discord;
using MongoDB.Driver;
using Shuvi.Classes.Data.User;
using Shuvi.Classes.Extensions;
using Shuvi.Classes.Factories.User;
using Shuvi.Classes.Settings;
using Shuvi.Classes.Types.Interaction;
using Shuvi.Enums.Money;
using Shuvi.Enums.User;
using Shuvi.Interfaces.User;
using Shuvi.Services.StaticServices.Database;
using Shuvi.Services.StaticServices.Event;
using Shuvi.Services.StaticServices.Localization;

namespace Shuvi.CommandParts
{
    public static class PlayerDiePart
    {
        private static readonly LocalizationLanguagePart _localizationPart = LocalizationService.Get("playerDiePart");

        public static async Task Start(CustomInteractionContext context, IDatabaseUser dbUser, string deadReason)
        {
            EventManager.InvokeOnPlayerDie(dbUser);
            var deadLocalization = _localizationPart.Get(context.Language);
            var embed = new EmbedBuilder()
                .WithAuthor(deadLocalization.Get("Embed/Dead/Author"))
                .WithDescription($"__{deadReason}__\n{deadLocalization.Get("Embed/Dead/Desc")}")
                .WithColor(new Color(0, 0, 0))
                .Build();
            dbUser.Rating.SetPoints(dbUser.Rating.Points / 2, context.Language);
            dbUser.Characteristics.Mana.Add(9999);
            dbUser.Characteristics.Energy.Add(9999);
            dbUser.Characteristics.Health.Add(9999);
            dbUser.Characteristics.Mana.SetMax(UserSettings.StandartMana);
            dbUser.Characteristics.Health.SetMax(UserSettings.StandartHealth);
            dbUser.Characteristics.Energy.SetMax(UserSettings.StandartEnergy);
            dbUser.Characteristics.Set(1, 1, 1, 1, 1);
            dbUser.Wallet.Set(MoneyType.Gold, 0);
            dbUser.Inventory.Clear();
            dbUser.Statistics.AddDeathCount(1);
            dbUser.SetProfession(UserProfession.NoProfession);
            dbUser.SetRace(UserFactory.GenerateRandomUserRace());
            dbUser.SetSubrace(UserFactory.GenerateRandomUserSubrace(dbUser.Race));
            dbUser.Spell.SetSpell(null);
            dbUser.Skill.SetSkill(null);
            dbUser.Statistics.RecordLiveTime();
            dbUser.Equipment.RemoveAllEquipment();
            dbUser.Location.SetRegion(0, 0);
            await UserDatabase.UpdateUser(dbUser.Id, new UpdateDefinitionBuilder<UserData>()
                .Set(x => x.Rating, dbUser.Rating.Points)
                .Set(x => x.Gold, dbUser.Wallet.Gold)
                .Set(x => x.Spell, dbUser.Spell.SpellId)
                .Set(x => x.Skill, dbUser.Skill.SkillId)
                .Set(x => x.Race, dbUser.Race)
                .Set(x => x.Subrace, dbUser.Subrace)
                .Set(x => x.Profession, dbUser.Profession)
                .Set(x => x.Inventory, dbUser.Inventory.GetItemsDictionary())
                .Set(x => x.Weapon, dbUser.Equipment.Weapon)
                .Set(x => x.Helmet, dbUser.Equipment.Helmet)
                .Set(x => x.Armor, dbUser.Equipment.Armor)
                .Set(x => x.Leggings, dbUser.Equipment.Leggings)
                .Set(x => x.Boots, dbUser.Equipment.Boots)
                .Set(x => x.Strength, dbUser.Characteristics.Strength)
                .Set(x => x.Agility, dbUser.Characteristics.Agility)
                .Set(x => x.Luck, dbUser.Characteristics.Luck)
                .Set(x => x.Intellect, dbUser.Characteristics.Intellect)
                .Set(x => x.Endurance, dbUser.Characteristics.Endurance)
                .Set(x => x.MaxMana, dbUser.Characteristics.Mana.Max)
                .Set(x => x.MaxHealth, dbUser.Characteristics.Health.Max)
                .Set(x => x.HealthRegenTime, dbUser.Characteristics.Health.RegenTime)
                .Set(x => x.ManaRegenTime, dbUser.Characteristics.Mana.RegenTime)
                .Set(x => x.EnergyRegenTime, dbUser.Characteristics.Energy.RegenTime)
                .Set(x => x.Statistics.LiveTime, dbUser.Statistics.LiveTime)
                .Set(x => x.Statistics.DeathCount, dbUser.Statistics.DeathCount)
                .Set(x => x.RegionId, dbUser.Location.RegionId)
                .Set(x => x.LocationId, dbUser.Location.LocationId));
            await context.CurrentMessage.ReplyAsync(embed: embed);
            await context.LastInteraction.TryDeferAsync();
        }
    }
}
