using Discord;
using MongoDB.Driver;
using Shuvi.Classes.Data.User;
using Shuvi.Classes.Extensions;
using Shuvi.Classes.Factories.CustomEmbed;
using Shuvi.Classes.Types.Characteristics.Bonuses;
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
using System.Reflection.PortableExecutable;

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
            status.Add(new ActionResult(battleLocalization.Get("Status/BattleStart")));
            while (true)
            {
                hod++;
                var embed = CreateFightEmbed(context, status, hod, player, enemy);
                var components = new ComponentBuilder()
                    .WithButton(battleLocalization.Get("Btn/LightAttack"), "lightAttack", ButtonStyle.Danger, row: 0)
                    .WithButton(battleLocalization.Get("Btn/HeavyAttack"), "heavyAttack", ButtonStyle.Danger, row: 0)
                    .WithButton(battleLocalization.Get("Btn/Dodge"), "dodge", ButtonStyle.Success, row: 1)
                    .WithButton(battleLocalization.Get("Btn/Defense"), "defense", ButtonStyle.Success, row: 1)
                    .WithButton(player.Skill.Info.GetName(context.Language), "skill", ButtonStyle.Primary, disabled: !player.Skill.CanUse(player), row: 2)
                    .WithButton(player.Spell.Info.GetName(context.Language), "spell", ButtonStyle.Primary, disabled: !player.Spell.CanCast(player), row: 2)
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
            var playerFullChar = new FightBonuses()
            {
                AttackDamage = MathF.Round(player.AllCharacteristics.GetFullAttackDamage(), 1),
                AbilityPower = MathF.Round(player.AllCharacteristics.GetFullAbilityPower(), 1),
                Armor = MathF.Round(player.AllCharacteristics.GetFullArmor(), 1),
                MagicResistance = MathF.Round(player.AllCharacteristics.GetFullMagicResistance(), 1),
                CriticalStrikeChance = MathF.Round(player.AllCharacteristics.GetFullCriticalStrikeChance() * 100, 1),
                CriticalStrikeDamageMultiplier = MathF.Round(player.AllCharacteristics.GetFullCriticalStrikeDamageMultiplier() * 100, 1),
                StrikeChance = MathF.Round((1f + player.AllCharacteristics.GetFullStrikeChance() - enemy.AllCharacteristics.GetFullDodgeChance()) * 100, 1),
                DodgeChance = MathF.Round((player.AllCharacteristics.GetFullDodgeChance() - enemy.AllCharacteristics.GetFullStrikeChance()) * 100, 1)
            };

            var playerBonuses = new FightBonuses()
            {
                AttackDamage = MathF.Round(playerFullChar.AttackDamage- player.Characteristics.GetAttackDamage(), 1),
                AbilityPower = MathF.Round(playerFullChar.AbilityPower - player.Characteristics.GetAbilityPower(), 1),
                Armor = MathF.Round(playerFullChar.Armor - player.Characteristics.GetArmor(), 1),
                MagicResistance = MathF.Round(playerFullChar.MagicResistance - player.Characteristics.GetMagicResistance(), 1),
                CriticalStrikeChance = MathF.Round(playerFullChar.CriticalStrikeChance - player.Characteristics.GetCriticalStrikeChance() * 100, 1),
                CriticalStrikeDamageMultiplier =  
                MathF.Round(playerFullChar.CriticalStrikeDamageMultiplier - player.Characteristics.GetCriticalStrikeDamageMultiplier() * 100, 1),
                StrikeChance = MathF.Round((player.AllCharacteristics.GetFullStrikeChance() - player.Characteristics.GetStrikeChance()) * 100, 1),
                DodgeChance = MathF.Round((player.AllCharacteristics.GetFullDodgeChance() - player.Characteristics.GetDodgeChance()) * 100, 1),
            };

            var enemyFullChar = new FightBonuses()
            {
                AttackDamage = MathF.Round(enemy.AllCharacteristics.GetFullAttackDamage(), 1),
                AbilityPower = MathF.Round(enemy.AllCharacteristics.GetFullAbilityPower(), 1),
                Armor = MathF.Round(enemy.AllCharacteristics.GetFullArmor(), 1),
                MagicResistance = MathF.Round(enemy.AllCharacteristics.GetFullMagicResistance(), 1),
                CriticalStrikeChance = MathF.Round(enemy.AllCharacteristics.GetFullCriticalStrikeChance() * 100, 1),
                CriticalStrikeDamageMultiplier = MathF.Round(enemy.AllCharacteristics.GetFullCriticalStrikeDamageMultiplier() * 100, 1),
                StrikeChance = MathF.Round((1f + enemy.AllCharacteristics.GetFullStrikeChance() - player.AllCharacteristics.GetFullDodgeChance()) * 100, 1),
                DodgeChance = MathF.Round((enemy.AllCharacteristics.GetFullDodgeChance() - player.AllCharacteristics.GetFullStrikeChance()) * 100, 1)
            };

            var enemyBonuses = new FightBonuses()
            {
                AttackDamage = MathF.Round(enemyFullChar.AttackDamage - enemy.Characteristics.GetAttackDamage(), 1),
                AbilityPower = MathF.Round(enemyFullChar.AbilityPower - enemy.Characteristics.GetAbilityPower(), 1),
                Armor = MathF.Round(enemyFullChar.Armor - enemy.Characteristics.GetArmor(), 1),
                MagicResistance = MathF.Round(enemyFullChar.MagicResistance - enemy.Characteristics.GetMagicResistance(), 1),
                CriticalStrikeChance = MathF.Round(enemyFullChar.CriticalStrikeChance - enemy.Characteristics.GetCriticalStrikeChance() * 100, 1),
                CriticalStrikeDamageMultiplier = 
                MathF.Round(enemyFullChar.CriticalStrikeDamageMultiplier - enemy.Characteristics.GetCriticalStrikeDamageMultiplier() * 100, 1),
                StrikeChance = MathF.Round((enemy.AllCharacteristics.GetFullStrikeChance() - enemy.Characteristics.GetStrikeChance()) * 100, 1),
                DodgeChance = MathF.Round((enemy.AllCharacteristics.GetFullDodgeChance() - enemy.Characteristics.GetDodgeChance()) * 100, 1),
            };

            return EmbedFactory.CreateEmbed()
                .WithAuthor(battleLocalization.Get("Embed/Battle/Author/SinglePVE").Format(hod))
                .WithDescription(status.GetDescriptions())
                .AddField(player.Name,
                $"**{namesLocalization.Get("AttackDamage")}:** {playerFullChar.AttackDamage.WithBonus(playerBonuses.AttackDamage)}\n" +
                $"**{namesLocalization.Get("AbilityPower")}:** {playerFullChar.AbilityPower.WithBonus(playerBonuses.AbilityPower)}\n" +
                $"**{namesLocalization.Get("Armor")}:** {playerFullChar.Armor.WithBonus(playerBonuses.Armor)}\n" +
                $"**{namesLocalization.Get("MagicResistance")}:** {playerFullChar.MagicResistance.WithBonus(playerBonuses.MagicResistance)}\n" +
                $"**{namesLocalization.Get("CriticalStrikeChance")}:** {playerFullChar.CriticalStrikeChance.WithBonusPercent(playerBonuses.CriticalStrikeChance)}\n" +
                $"**{namesLocalization.Get("CriticalStrikeDamageMultiplier")}:** {playerFullChar.CriticalStrikeDamageMultiplier
                .WithBonusPercent(playerBonuses.CriticalStrikeDamageMultiplier)}\n" +
                $"**{namesLocalization.Get("DodgeChance")}:** {playerFullChar.DodgeChance.WithBonusPercent(playerBonuses.DodgeChance)}\n" +
                $"**{namesLocalization.Get("StrikeChance")}:** {playerFullChar.StrikeChance.WithBonusPercent(playerBonuses.StrikeChance)}\n" +
                $"**{namesLocalization.Get("Health")}:** {player.Characteristics.Health.Now}/" +
                $"{player.Characteristics.Health.Max.WithBonus(player.EffectBonuses.Health)}\n" +
                $"**{namesLocalization.Get("Mana")}:** {player.Characteristics.Mana.Now}/" +
                $"{player.Characteristics.Mana.Max.WithBonus(player.EffectBonuses.Mana)}",
                true)
                .AddField(battleLocalization.Get("Embed/Battle/Info"),
                $"{battleLocalization.Get("Embed/Battle/Spell").Format(player.Spell.Info.GetName(context.Language))}\n" +
                $"{(player.Spell.HaveSpell() ? $"{battleLocalization.Get("Embed/Battle/SpellCost")
                .Format(player.Spell.Cost, EmojiService.Get("MagicFull"))}\n" : "")}" +
                $"{battleLocalization.Get("Embed/Battle/Skill").Format(player.Skill.Info.GetName(context.Language))}\n" +
                $"{(player.Skill.HaveSkill() ? battleLocalization.Get("Embed/Battle/SkillUsesLeft")
                .Format(player.Skill.UsesLeft) : "")}",
                true)
                .AddField(" - - - - - - - - - - - - - - - - - - - - - - - - - - - - -", "** **", false)
                .AddField(enemy.Name,
                $"**{namesLocalization.Get("AttackDamage")}:** {enemyFullChar.AttackDamage.WithBonus(enemyBonuses.AttackDamage)}\n" +
                $"**{namesLocalization.Get("AbilityPower")}:** {enemyFullChar.AbilityPower.WithBonus(enemyBonuses.AbilityPower)}\n" +
                $"**{namesLocalization.Get("Armor")}:** {enemyFullChar.Armor.WithBonus(enemyBonuses.Armor)}\n" +
                $"**{namesLocalization.Get("MagicResistance")}:** {enemyFullChar.MagicResistance.WithBonus(enemyBonuses.MagicResistance)}\n" +
                $"**{namesLocalization.Get("CriticalStrikeChance")}:** {enemyFullChar.CriticalStrikeChance.WithBonusPercent(enemyBonuses.CriticalStrikeChance)}\n" +
                $"**{namesLocalization.Get("CriticalStrikeDamageMultiplier")}:** {enemyFullChar.CriticalStrikeDamageMultiplier
                .WithBonusPercent(enemyBonuses.CriticalStrikeDamageMultiplier)}\n" +
                $"**{namesLocalization.Get("DodgeChance")}:** {enemyFullChar.DodgeChance.WithBonusPercent(enemyBonuses.DodgeChance)}\n" +
                $"**{namesLocalization.Get("StrikeChance")}:** {enemyFullChar.StrikeChance.WithBonusPercent(enemyBonuses.StrikeChance)}\n" +
                $"**{namesLocalization.Get("Health")}:** {enemy.Characteristics.Health.Now}/" +
                $"{enemy.Characteristics.Health.Max.WithBonus(enemy.EffectBonuses.Health)}\n" +
                $"**{namesLocalization.Get("Mana")}:** {enemy.Characteristics.Mana.Now}/" +
                $"{enemy.Characteristics.Mana.Max.WithBonus(enemy.EffectBonuses.Mana)}",
                true)
                .AddField(battleLocalization.Get("Embed/Battle/Info"),
                $"{battleLocalization.Get("Embed/Battle/Spell").Format(enemy.Spell.Info.GetName(context.Language))}\n" +
                $"{(enemy.Spell.HaveSpell() ? $"{battleLocalization.Get("Embed/Battle/SpellCost")
                .Format(enemy.Spell.Cost, EmojiService.Get("MagicFull"))}\n" : "")}",
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
            status.Add(dbUser.Rating.AddPoints(enemy.RatingGet, enemy.Rank, context.Language));
            dbUser.Statistics.UpdateMaxRating(dbUser.Rating.Points);
            status.Add(new ActionResult(battleLocalization.Get("Status/BattleWin")));
            status.Add(new ActionResult(drop.GetDropInfo(context.Language)));
            await UserDatabase.UpdateUser(dbUser.Id, new UpdateDefinitionBuilder<UserData>()
                .Set(x => x.HealthRegenTime, dbUser.Characteristics.Health.RegenTime)
                .Set(x => x.ManaRegenTime, dbUser.Characteristics.Mana.RegenTime)
                .Set(x => x.Rating, dbUser.Rating.Points)
                .Set(x => x.Statistics.EnemyKills, dbUser.Statistics.EnemyKills)
                .Set(x => x.Statistics.MaxRating, dbUser.Statistics.MaxRating)
                .Set(x => x.Inventory, dbUser.Inventory.GetItemsDictionary()));
            var embed = CreateFightEmbed(context, status, hod, player, enemy).ToEmbedBuilder()
                .WithAuthor(battleLocalization.Get("Embed/Battle/Author/SinglePVE/Win").Format(hod))
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
            status.Add(new ActionResult(battleLocalization.Get("Status/BattleDraw")));
            await UserDatabase.UpdateUser(dbUser.Id, new UpdateDefinitionBuilder<UserData>()
                .Set(x => x.HealthRegenTime, dbUser.Characteristics.Health.RegenTime)
                .Set(x => x.ManaRegenTime, dbUser.Characteristics.Mana.RegenTime));
            var embed = CreateFightEmbed(context, status, hod, player, enemy).ToEmbedBuilder()
                .WithAuthor(battleLocalization.Get("Embed/Battle/Author/SinglePVE/Draw").Format(hod))
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
                .WithAuthor(battleLocalization.Get("Embed/Battle/Author/SinglePVE/Loose").Format(hod))
                .WithColor(new Color(0, 0, 0))
                .Build();
            await context.Interaction.ModifyOriginalResponseAsync(msg => { msg.Embed = embed; msg.Components = new ComponentBuilder().Build(); });
            await context.LastInteraction.TryDeferAsync();
            await PlayerDiePart.Start(context, dbUser, battleLocalization.Get("Status/BattleLoose").Format(enemy.Name));
        }
    }
}
