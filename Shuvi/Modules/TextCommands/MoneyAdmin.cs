using Discord.Commands;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;
using Shuvi.Classes.Data.User;
using Shuvi.Services.StaticServices.Database;
using Shuvi.Classes.Factories.CustomEmbed;
using Shuvi.Enums.Localization;
using Shuvi.Enums.Money;

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
                [Summary("user")] IUser paramUser,
                [Summary("amount")] int amount
                )
        {
            var dbUser = await UserDatabase.TryGetUser(paramUser.Id);
            if (dbUser is null)
            {
                await ReplyAsync(embed: EmbedFactory.CreateErrorEmbed("Пользователь еще не создал аккаунт.", Language.Ru));
                return;
            }
            dbUser.Wallet.Add(MoneyType.Gold, amount);
            await UserDatabase.UpdateUser(
                paramUser.Id,
                new UpdateDefinitionBuilder<UserData>().Set(x => x.Gold, dbUser.Wallet.Gold));
            var embed = EmbedFactory.CreateInfoEmbed($"Вы успешно выдали {amount} золота пользователю {paramUser.Username}.");
            await ReplyAsync(embed: embed);
        }

        [Command("RemoveGold", true)]
        public async Task RemoveGoldCommandAsync(
                [Summary("user")] IUser paramUser,
                [Summary("amount")] int amount
                )
        {
            var dbUser = await UserDatabase.TryGetUser(paramUser.Id);
            if (dbUser is null)
            {
                await ReplyAsync(embed: EmbedFactory.CreateErrorEmbed("Пользователь еще не создал аккаунт.", Language.Ru));
                return;
            }
            dbUser.Wallet.Reduce(MoneyType.Gold, amount);
            await UserDatabase.UpdateUser(
                paramUser.Id,
                new UpdateDefinitionBuilder<UserData>().Set(x => x.Gold, dbUser.Wallet.Gold));
            var embed = EmbedFactory.CreateInfoEmbed($"Вы успешно забрали {amount} золота у пользователя {paramUser.Username}.");
            await ReplyAsync(embed: embed);
        }

        [Command("GiveDispoints", true)]
        public async Task GiveDispointsCommandAsync(
                [Summary("user")] IUser paramUser,
                [Summary("amount")] int amount
                )
        {
            var dbUser = await UserDatabase.TryGetUser(paramUser.Id);
            if (dbUser is null)
            {
                await ReplyAsync(embed: EmbedFactory.CreateErrorEmbed("Пользователь еще не создал аккаунт.", Language.Ru));
                return;
            }
            dbUser.Wallet.Add(MoneyType.Dispoints, amount);
            await UserDatabase.UpdateUser(
                paramUser.Id,
                new UpdateDefinitionBuilder<UserData>().Set(x => x.Dispoints, dbUser.Wallet.Dispoints));
            var embed = EmbedFactory.CreateInfoEmbed($"Вы успешно выдали {amount} диспоинтов пользователю {paramUser.Username}.");
            await ReplyAsync(embed: embed);
        }

        [Command("RemoveDispoints", true)]
        public async Task RemoveDispointsCommandAsync(
                [Summary("user")] IUser paramUser,
                [Summary("amount")] int amount
                )
        {
            var dbUser = await UserDatabase.TryGetUser(paramUser.Id);
            if (dbUser is null)
            {
                await ReplyAsync(embed: EmbedFactory.CreateErrorEmbed("Пользователь еще не создал аккаунт.", Language.Ru));
                return;
            }
            dbUser.Wallet.Reduce(MoneyType.Dispoints, amount);
            await UserDatabase.UpdateUser(
                paramUser.Id,
                new UpdateDefinitionBuilder<UserData>().Set(x => x.Dispoints, dbUser.Wallet.Dispoints));
            var embed = EmbedFactory.CreateInfoEmbed($"Вы успешно забрали {amount} диспоинтов у пользователя {paramUser.Username}.");
            await ReplyAsync(embed: embed);
        }
    }
}
