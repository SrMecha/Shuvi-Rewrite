using Discord;
using Shuvi.Classes.Extensions;
using Shuvi.Classes.Factories.CustomEmbed;
using Shuvi.Classes.Types.Combat;
using Shuvi.Classes.Types.Interaction;
using Shuvi.Classes.Types.Status;
using Shuvi.Interfaces.Combat;
using Shuvi.Interfaces.Enemy;
using Shuvi.Interfaces.Status;
using Shuvi.Interfaces.User;
using Shuvi.Services.StaticServices.Emoji;
using Shuvi.Services.StaticServices.Localization;

namespace Shuvi.CommandParts
{
    public static class PlayerVersusEnemyPart
    {
        private static readonly LocalizationLanguagePart _localizationPart = LocalizationService.Get("battlePart");
        private const int MaxHod = 100; 

        public static async Task Start(CustomInteractionContext context, IDatabaseUser dbUser, IDatabaseEnemy dbEnemy)
        {
            var battleLocalization = _localizationPart.Get(context.Language);
            var player = new CombatPlayer(dbUser, context.User.Username);
            var enemy = new CombatEnemy(dbEnemy, context.Language);
            var isAfk = false;
            var hod = 0;
            var status = new ResultStorage();
            status.Add(new ActionResult(battleLocalization.Get("status/battleStart")));
            while (true)
            {
                hod++;
                var embed = CreateFightEmbed(context, status, hod, player, enemy);
                var components = new ComponentBuilder()
                    .WithButton(battleLocalization.Get("btn/lightAttack"), "lightAttack", ButtonStyle.Danger, row: 0)
                    .WithButton(battleLocalization.Get("btn/heavyAttack"), "heavyAttack", ButtonStyle.Danger, row: 0)
                    .WithButton(battleLocalization.Get("btn/dodge"), "dodge", ButtonStyle.Success, row: 1)
                    .WithButton(battleLocalization.Get("btn/defense"), "defense", ButtonStyle.Success, row: 1)
                    .WithButton(player.Skill.Info.GetName(context.Language), "spell", ButtonStyle.Primary, disabled: player.Skill.CanUse(player),row: 2)
                    .WithButton(player.Spell.Info.GetName(context.Language), "skill", ButtonStyle.Primary, disabled: player.Spell.CanCast(player), row: 2)
                    .Build();
                status.Clear();
                if (isAfk)
                    status.Add(player.RandomAction(enemy, context.Language));
                else
                {
                    await context.Interaction.ModifyOriginalResponseAsync(msg => { msg.Embed = embed; msg.Components = components; });
                    await context.LastInteraction.TryDeferAsync();
                    var interaction = await context.WaitForButton();
                    if (interaction is null)
                    {
                        isAfk = true;
                        await context.CurrentMessage!.RemoveButtonsAsync();
                        return;
                    }
                    switch (interaction.Data.CustomId)
                    {
                        case "lightAttack":
                            status.Add(player.DealLightDamage(enemy, context.Language));
                            break;
                        case "heavyAttack":
                            status.Add(player.DealHeavyDamage(enemy, context.Language));
                            break;
                        case "dodge":
                            status.Add(player.PreparingForDodge(enemy, context.Language));
                            break;
                        case "defense":
                            status.Add(player.PreparingForDefense(enemy, context.Language));
                            break;
                        case "spell":
                            status.Add(player.CastSpell(enemy, context.Language));
                            break;
                        case "skill":
                            status.Add(player.UseSkill(enemy, context.Language));
                            break;
                        default:
                            break;
                    }
                }
                if (enemy.IsDead)
                {

                }

            }
        }
        public static Embed CreateFightEmbed(CustomInteractionContext context, IResultStorage status, int hod, ICombatPlayer player, ICombatEnemy enemy)
        {
            var battleLocalization = _localizationPart.Get(context.Language);
            var namesLocalization = LocalizationService.Get("names").Get(context.Language);
            return EmbedFactory.CreateUserEmbed(context.User)
                .WithAuthor(battleLocalization.Get("embed/battle/singlePVE/author").Format(hod))
                .WithDescription(status.GetDescriptions())
                .AddField(player.Name,
                $"**{namesLocalization.Get("strength")}** {player.Characteristics.Strength.WithBonus(player.EffectBonuses.Strength)}\n" +
                $"**{namesLocalization.Get("agility")}:** {player.Characteristics.Agility.WithBonus(player.EffectBonuses.Agility)}\n" +
                $"**{namesLocalization.Get("luck")}:** {player.Characteristics.Luck.WithBonus(player.EffectBonuses.Luck)}\n" +
                $"**{namesLocalization.Get("intellect")}:** {player.Characteristics.Intellect.WithBonus(player.EffectBonuses.Intellect)}\n" +
                $"**{namesLocalization.Get("endurance")}:** {player.Characteristics.Endurance.WithBonus(player.EffectBonuses.Endurance)}\n" +
                $"**{namesLocalization.Get("health")}:** {player.Characteristics.Health.Now}/" +
                $"{player.Characteristics.Health.Max.WithBonus(player.EffectBonuses.Health)}\n" +
                $"**{namesLocalization.Get("mana")}:** {player.Characteristics.Mana.Now}/" +
                $"{player.Characteristics.Mana.Max.WithBonus(player.EffectBonuses.Mana)}",
                true)
                .AddField(battleLocalization.Get("embed/battle/info"),
                $"{battleLocalization.Get("embed/battle/spell").Format(enemy.Spell.Info.GetName(context.Language))}\n" +
                $"{(player.Spell.HaveSpell() ? $"{battleLocalization.Get("embed/battle/spellCost")
                .Format(enemy.Spell.Cost, EmojiService.Get("magicFull"))}\n" : "")}" +
                $"{battleLocalization.Get("embed/battle/skill").Format(player.Skill.Info.GetName(context.Language))}\n" +
                $"{(player.Skill.HaveSkill() ? battleLocalization.Get("embed/battle/skillUsesLeft")
                .Format(player.Skill.UsesLeft) : "")}",
                true)
                .AddField(" - - - - - - - - - - - - - - - - - - - - - - - - - - - - -", "** **", false)
                .AddField(enemy.Name,
                $"**{namesLocalization.Get("strength")}** {enemy.Characteristics.Strength.WithBonus(enemy.EffectBonuses.Strength)}\n" +
                $"**{namesLocalization.Get("agility")}:** {enemy.Characteristics.Agility.WithBonus(enemy.EffectBonuses.Agility)}\n" +
                $"**{namesLocalization.Get("luck")}:** {enemy.Characteristics.Luck.WithBonus(enemy.EffectBonuses.Luck)}\n" +
                $"**{namesLocalization.Get("intellect")}:** {enemy.Characteristics.Intellect.WithBonus(enemy.EffectBonuses.Intellect)}\n" +
                $"**{namesLocalization.Get("endurance")}:** {enemy.Characteristics.Endurance.WithBonus(enemy.EffectBonuses.Endurance)}\n" +
                $"**{namesLocalization.Get("health")}:** {enemy.Characteristics.Health.Now}/" +
                $"{enemy.Characteristics.Health.Max.WithBonus(enemy.EffectBonuses.Health)}\n" +
                $"**{namesLocalization.Get("mana")}:** {enemy.Characteristics.Mana.Now}/" +
                $"{enemy.Characteristics.Mana.Max.WithBonus(enemy.EffectBonuses.Mana)}",
                true)
                .AddField(battleLocalization.Get("embed/battle/info"),
                $"{battleLocalization.Get("embed/battle/spell").Format(enemy.Spell.Info.GetName(context.Language))}\n" +
                $"{(player.Spell.HaveSpell() ? $"{battleLocalization.Get("embed/battle/spellCost")
                .Format(enemy.Spell.Cost, EmojiService.Get("magicFull"))}\n" : "")}",
                true)
                .Build();
        }
    }
}
