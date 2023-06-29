using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Shuvi.Classes.Data.User;
using Shuvi.Classes.Extensions;
using Shuvi.Classes.Factories.CustomEmbed;
using Shuvi.Enums.Localization;
using Shuvi.Services.StaticServices.Database;

namespace Shuvi.Modules.TextCommands
{
    public class RatingAdminCommandModule : ModuleBase<SocketCommandContext>
    {
        private readonly DiscordShardedClient _client;

        public RatingAdminCommandModule(IServiceProvider provider)
        {
            _client = provider.GetRequiredService<DiscordShardedClient>();
        }

        [Command("AddRating", true)]
        public async Task AddRatingCommandAsync(
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
            var results = dbUser.Rating.AddPoints(amount, Language.Ru);
            await UserDatabase.UpdateUser(
                userId,
                new UpdateDefinitionBuilder<UserData>().Set(x => x.Rating, dbUser.Rating.Points));
            var embed = EmbedFactory.CreateInfoEmbed($"Вы успешно выдали {amount} " +
                $"рейтинга пользователю <@{userId}> ({results.RankBefore.GetName()} -> {results.RankAfter.GetName()}).");
            await ReplyAsync(embed: embed);
        }

        [Command("RemoveRating", true)]
        public async Task RemoveRatingCommandAsync(
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
            var results = dbUser.Rating.RemovePoints(amount, Language.Ru);
            await UserDatabase.UpdateUser(
                userId,
                new UpdateDefinitionBuilder<UserData>().Set(x => x.Rating, dbUser.Rating.Points));
            var embed = EmbedFactory.CreateInfoEmbed($"Вы успешно сняли {amount} " +
                $"рейтинга пользователю <@{userId}> ({results.RankBefore.GetName()} -> {results.RankAfter.GetName()}).");
            await ReplyAsync(embed: embed);
        }
    }
}
