using Discord;
using MongoDB.Driver;
using Shuvi.Classes.Data.User;
using Shuvi.Classes.Extensions;
using Shuvi.Classes.Factories.CustomEmbed;
using Shuvi.Classes.Types.Combat;
using Shuvi.Classes.Types.Interaction;
using Shuvi.Classes.Types.Status;
using Shuvi.Interfaces.Combat;
using Shuvi.Interfaces.Enemy;
using Shuvi.Interfaces.Status;
using Shuvi.Interfaces.User;
using Shuvi.Services.StaticServices.Database;
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
                    .WithButton(player.Skill.Info.GetName(context.Language), "spell", ButtonStyle.Primary, disabled: !player.Skill.CanUse(player), row: 2)
                    .WithButton(player.Spell.Info.GetName(context.Language), "skill", ButtonStyle.Primary, disabled: !player.Spell.CanCast(player), row: 2)
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
                        status.Add(player.RandomAction(enemy, context.Language));
                    }
                    else
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
                    await FightWin(context, status, hod, dbUser, player, dbEnemy, enemy);
                    return;
                }
                status.Add(enemy.RandomAction(player, context.Language));
                if (player.IsDead)
                {
                    await FightLoose(context, status, hod, dbUser, player, enemy);
                    return;
                }
                player.Update(context.Language);
                if (player.IsDead)
                {
                    await FightLoose(context, status, hod, dbUser, player, enemy);
                    return;
                }
                enemy.Update(context.Language);
                if (enemy.IsDead)
                {
                    await FightWin(context, status, hod, dbUser, player, dbEnemy, enemy);
                    return;
                }
            }
        }
        public static Embed CreateFightEmbed(CustomInteractionContext context, IResultStorage status, int hod, ICombatPlayer player, ICombatEnemy enemy)
        {
            var battleLocalization = _localizationPart.Get(context.Language);
            var namesLocalization = LocalizationService.Get("names").Get(context.Language);
            return EmbedFactory.CreateUserEmbed(context.User)
                .WithAuthor(battleLocalization.Get("embed/battle/author/singlePVE").Format(hod))
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
        public static async Task FightWin(CustomInteractionContext context, IResultStorage status, int hod,
            IDatabaseUser dbUser, ICombatPlayer player, IDatabaseEnemy dbEnemy, ICombatEnemy enemy)
        {
            var battleLocalization = _localizationPart.Get(context.Language);
            var drop = enemy.Drop.GetDrop(dbUser.Characteristics.Luck);
            dbUser.Inventory.AddItems(drop);
            dbUser.Characteristics.Health.Reduce(dbUser.Characteristics.Health.GetCurrent() - player.Characteristics.Health.Now);
            dbUser.Characteristics.Mana.Reduce(dbUser.Characteristics.Mana.GetCurrent() - player.Characteristics.Mana.Now);
            dbUser.Statistics.AddEnemyKilled(dbEnemy.Id, 1);
            status.Add(dbUser.Rating.AddPoints(enemy.RatingGet, enemy.Rank));
            dbUser.Statistics.UpdateMaxRating(dbUser.Rating.Points);
            status.Add(new ActionResult(battleLocalization.Get("status/battleWin")));
            status.Add(new ActionResult(drop.GetDropInfo(context.Language)));
            await UserDatabase.UpdateUser(dbUser.Id, new UpdateDefinitionBuilder<UserData>()
                .Set(x => x.HealthRegenTime, dbUser.Characteristics.Health.RegenTime)
                .Set(x => x.ManaRegenTime, dbUser.Characteristics.Mana.RegenTime)
                .Set(x => x.Rating, dbUser.Rating.Points)
                .Set(x => x.Statistics.EnemyKills, dbUser.Statistics.EnemyKills)
                .Set(x => x.Statistics.MaxRating, dbUser.Statistics.MaxRating)
                .Set(x => x.Inventory, dbUser.Inventory.GetItemsDictionary()));
            var embed = CreateFightEmbed(context, status, hod, player, enemy).ToEmbedBuilder()
                .WithAuthor(battleLocalization.Get("embed/battle/author/singlePVE/win").Format(hod))
                .WithColor(Color.Green)
                .Build();
            await context.Interaction.ModifyOriginalResponseAsync(msg => { msg.Embed = embed; msg.Components = new ComponentBuilder().Build(); });
            await context.LastInteraction.TryDeferAsync();
        }
        public static async Task FightDraw(CustomInteractionContext context, IResultStorage status, int hod, 
            IDatabaseUser dbUser, ICombatPlayer player, ICombatEnemy enemy)
        {
            var battleLocalization = _localizationPart.Get(context.Language);
            dbUser.Characteristics.Health.Reduce(dbUser.Characteristics.Health.GetCurrent() - player.Characteristics.Health.Now);
            dbUser.Characteristics.Mana.Reduce(dbUser.Characteristics.Mana.GetCurrent() - player.Characteristics.Mana.Now);
            status.Add(new ActionResult(battleLocalization.Get("status/battleDraw")));
            await UserDatabase.UpdateUser(dbUser.Id, new UpdateDefinitionBuilder<UserData>()
                .Set(x => x.HealthRegenTime, dbUser.Characteristics.Health.RegenTime)
                .Set(x => x.ManaRegenTime, dbUser.Characteristics.Mana.RegenTime));
            var embed = CreateFightEmbed(context, status, hod, player, enemy).ToEmbedBuilder()
                .WithAuthor(battleLocalization.Get("embed/battle/author/singlePVE/draw").Format(hod))
                .WithColor(Color.LighterGrey)
                .Build();
            await context.Interaction.ModifyOriginalResponseAsync(msg => { msg.Embed = embed; msg.Components = new ComponentBuilder().Build(); });
            await context.LastInteraction.TryDeferAsync();
        }
        public static async Task FightLoose(CustomInteractionContext context, IResultStorage status, int hod,
            IDatabaseUser dbUser, ICombatPlayer player, ICombatEnemy enemy)
        {
            var battleLocalization = _localizationPart.Get(context.Language);
            var embed = CreateFightEmbed(context, status, hod, player, enemy).ToEmbedBuilder()
                .WithAuthor(battleLocalization.Get("embed/battle/author/singlePVE/loose").Format(hod))
                .WithColor(new Color(0, 0, 0))
                .Build();
            await context.Interaction.ModifyOriginalResponseAsync(msg => { msg.Embed = embed; msg.Components = new ComponentBuilder().Build(); });
            await context.LastInteraction.TryDeferAsync();
            await PlayerDiePart.Start(context, dbUser, battleLocalization.Get("status/battleLoose").Format(enemy.Name));
        }
    }
}
