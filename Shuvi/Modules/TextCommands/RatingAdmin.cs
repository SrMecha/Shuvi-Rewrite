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
using Shuvi.Classes.Extensions;

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
            var results = dbUser.Rating.AddPoints(amount, Language.Ru);
            await UserDatabase.UpdateUser(
                paramUser.Id,
                new UpdateDefinitionBuilder<UserData>().Set(x => x.Rating, dbUser.Rating.Points));
            var embed = EmbedFactory.CreateInfoEmbed($"Вы успешно выдали {amount} " +
                $"рейтинга пользователю {paramUser.Username} ({results.RankBefore.GetName()} -> {results.RankAfter.GetName()}).");
            await ReplyAsync(embed: embed);
        }

        [Command("RemoveRating", true)]
        public async Task RemoveRatingCommandAsync(
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
            var results = dbUser.Rating.RemovePoints(amount, Language.Ru);
            await UserDatabase.UpdateUser(
                paramUser.Id,
                new UpdateDefinitionBuilder<UserData>().Set(x => x.Rating, dbUser.Rating.Points));
            var embed = EmbedFactory.CreateInfoEmbed($"Вы успешно сняли {amount} " +
                $"рейтинга пользователю {paramUser.Username} ({results.RankBefore.GetName()} -> {results.RankAfter.GetName()}).");
            await ReplyAsync(embed: embed);
        }
    }
}
