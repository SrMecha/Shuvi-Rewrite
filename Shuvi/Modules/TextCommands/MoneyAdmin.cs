﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Shuvi.Classes.Data.User;
using Shuvi.Classes.Factories.CustomEmbed;
using Shuvi.Enums.Localization;
using Shuvi.Enums.Money;
using Shuvi.Services.StaticServices.Database;

namespace Shuvi.Modules.TextCommands
{
    public class MoneyAdminCommandModule : ModuleBase<SocketCommandContext>
    {
        private readonly DiscordShardedClient _client;

        public MoneyAdminCommandModule(IServiceProvider provider)
        {
            _client = provider.GetRequiredService<DiscordShardedClient>();
        }

        [Command("GiveGold", true)]
        public async Task GiveGoldCommandAsync(
                [Summary("userId")] ulong userId,
                [Summary("amount")] int amount
                )
        {
            var dbUser = await UserDatabase.TryGetUser(userId);
            if (dbUser is null)
            {
                await ReplyAsync(embed: EmbedFactory.CreateErrorEmbed("Пользователь еще не создал аккаунт.", Language.Ru));
                return;
            }
            dbUser.Wallet.Add(MoneyType.Gold, amount);
            await UserDatabase.UpdateUser(
                userId,
                new UpdateDefinitionBuilder<UserData>().Set(x => x.Gold, dbUser.Wallet.Gold));
            var embed = EmbedFactory.CreateInfoEmbed($"Вы успешно выдали {amount} золота пользователю <@{userId}>.");
            await ReplyAsync(embed: embed);
        }

        [Command("RemoveGold", true)]
        public async Task RemoveGoldCommandAsync(
                [Summary("userId")] ulong userId,
                [Summary("amount")] int amount
                )
        {
            var dbUser = await UserDatabase.TryGetUser(userId);
            if (dbUser is null)
            {
                await ReplyAsync(embed: EmbedFactory.CreateErrorEmbed("Пользователь еще не создал аккаунт.", Language.Ru));
                return;
            }
            dbUser.Wallet.Reduce(MoneyType.Gold, amount);
            await UserDatabase.UpdateUser(
                userId,
                new UpdateDefinitionBuilder<UserData>().Set(x => x.Gold, dbUser.Wallet.Gold));
            var embed = EmbedFactory.CreateInfoEmbed($"Вы успешно забрали {amount} золота у пользователя <@{userId}>.");
            await ReplyAsync(embed: embed);
        }

        [Command("GiveDispoints", true)]
        public async Task GiveDispointsCommandAsync(
                [Summary("userId")] ulong userId,
                [Summary("amount")] int amount
                )
        {
            var dbUser = await UserDatabase.TryGetUser(userId);
            if (dbUser is null)
            {
                await ReplyAsync(embed: EmbedFactory.CreateErrorEmbed("Пользователь еще не создал аккаунт.", Language.Ru));
                return;
            }
            dbUser.Wallet.Add(MoneyType.Dispoints, amount);
            await UserDatabase.UpdateUser(
                userId,
                new UpdateDefinitionBuilder<UserData>().Set(x => x.Dispoints, dbUser.Wallet.Dispoints));
            var embed = EmbedFactory.CreateInfoEmbed($"Вы успешно выдали {amount} диспоинтов пользователю <@{userId}>.");
            await ReplyAsync(embed: embed);
        }

        [Command("RemoveDispoints", true)]
        public async Task RemoveDispointsCommandAsync(
                [Summary("userId")] ulong userId,
                [Summary("amount")] int amount
                )
        {
            var dbUser = await UserDatabase.TryGetUser(userId);
            if (dbUser is null)
            {
                await ReplyAsync(embed: EmbedFactory.CreateErrorEmbed("Пользователь еще не создал аккаунт.", Language.Ru));
                return;
            }
            dbUser.Wallet.Reduce(MoneyType.Dispoints, amount);
            await UserDatabase.UpdateUser(
                userId,
                new UpdateDefinitionBuilder<UserData>().Set(x => x.Dispoints, dbUser.Wallet.Dispoints));
            var embed = EmbedFactory.CreateInfoEmbed($"Вы успешно забрали {amount} диспоинтов у пользователя <@{userId}>.");
            await ReplyAsync(embed: embed);
        }
    }
}
