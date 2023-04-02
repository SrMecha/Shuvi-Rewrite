using Discord;
using MongoDB.Driver;
using Shuvi.Classes.Data.User;
using Shuvi.Classes.Extensions;
using Shuvi.Classes.Factories.CustomEmbed;
using Shuvi.Classes.Types.Combat;
using Shuvi.Classes.Types.Interaction;
using Shuvi.Interfaces.Spell;
using Shuvi.Interfaces.User;
using Shuvi.Services.StaticServices.Database;
using Shuvi.Services.StaticServices.Info;
using Shuvi.Services.StaticServices.Localization;

namespace Shuvi.CommandParts
{
    public static class HuntPart
    {
        private static readonly LocalizationLanguagePart _localizationPart = LocalizationService.Get("huntPart");
        public const int HuntEnergyCost = 1;

        public static async Task Start(CustomInteractionContext context, IDatabaseUser dbUser)
        {
            var huntLocalization = _localizationPart.Get(context.Language);
            var namesLocalization = LocalizationService.Get("names").Get(context.Language);
            while (context.LastInteraction is not null)
            {
                dbUser.Characteristics.Energy.Reduce(HuntEnergyCost);
                await UserDatabase.UpdateUser(dbUser.Id, 
                    new UpdateDefinitionBuilder<UserData>().Set(x => x.EnergyRegenTime, dbUser.Characteristics.Energy.RegenTime));
                var enemy = EnemyDatabase.GetEnemy(dbUser.Location.GetLocation().Enemies.GetRandom());
                var combatEnemy = new CombatEnemy(enemy, context.Language);
                var canViewStats = EnemyInfoService.CanViewEnemyCharacteristics(dbUser, enemy);
                var canViewSpell = EnemyInfoService.CanViewEnemyAbilities(dbUser, enemy);
                var embed = EmbedFactory.CreateUserEmbed(context.User, dbUser)
                    .WithAuthor(huntLocalization.Get("embed/search/author"))
                    .WithDescription($"{huntLocalization.Get("embed/search/meet").Format(enemy.Info.GetName(context.Language))}\n" +
                    $"{huntLocalization.Get("embed/search/energyRemaining").Format(dbUser.Characteristics.Energy.GetCurrent(), 
                    dbUser.Characteristics.Energy.Max)}")
                    .AddField(enemy.Info.GetName(context.Language),
                    canViewStats ? $"**{namesLocalization.Get("strength")}** {combatEnemy.Characteristics.Strength}\n" +
                    $"**{namesLocalization.Get("agility")}:** {combatEnemy.Characteristics.Agility}\n" +
                    $"**{namesLocalization.Get("luck")}:** {combatEnemy.Characteristics.Luck}\n" +
                    $"**{namesLocalization.Get("intellect")}:** {combatEnemy.Characteristics.Intellect}\n" +
                    $"**{namesLocalization.Get("endurance")}:** {combatEnemy.Characteristics.Endurance}" +
                    $"**{namesLocalization.Get("health")}:** {combatEnemy.Characteristics.Health.Max}/{combatEnemy.Characteristics.Health.Max}\n" +
                    $"**{namesLocalization.Get("mana")}:** {combatEnemy.Characteristics.Mana.Max}/{combatEnemy.Characteristics.Mana.Max}" :
                    huntLocalization.Get("embed/search/characteristicsUnknown"),
                    true)
                    .AddField(huntLocalization.Get("embed/search/info"),
                    $"{huntLocalization.Get("embed/search/rank").Format(enemy.Rank.ToString())}\n" +
                    $"{(canViewSpell ? huntLocalization.Get("embed/search/spell").Format(enemy.Spell.Info.GetName(context.Language)) : 
                    huntLocalization.Get("embed/search/unknown"))}")
                    .Build();
                var components = new ComponentBuilder()
                    .WithButton(huntLocalization.Get("btn/attack"), "attack", ButtonStyle.Danger, row: 0)
                    .WithButton(huntLocalization.Get("btn/tame"), "tame", ButtonStyle.Success, row: 0)
                    .WithButton(huntLocalization.Get("btn/retry"), "retry", ButtonStyle.Secondary, 
                    disabled: !dbUser.Characteristics.HaveEnergy(HuntEnergyCost), row: 1)
                    .WithButton(huntLocalization.Get("btn/leave"), "leave", ButtonStyle.Secondary, row: 2)
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
                    case "attack":

                        return;
                    case "tame":

                        return;
                    case "retry":
                        break;
                    case "leave":

                        return;
                }
            }
        }
    }
}
