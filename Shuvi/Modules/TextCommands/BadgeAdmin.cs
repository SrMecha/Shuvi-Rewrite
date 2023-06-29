using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Shuvi.Classes.Data.User;
using Shuvi.Classes.Extensions;
using Shuvi.Classes.Factories.CustomEmbed;
using Shuvi.Enums.Localization;
using Shuvi.Enums.User;
using Shuvi.Services.StaticServices.Database;

namespace Shuvi.Modules.TextCommands
{
    public class BadgeAdminCommandModule : ModuleBase<SocketCommandContext>
    {
        private readonly DiscordShardedClient _client;
        private readonly CommandService _commands;

        public BadgeAdminCommandModule(IServiceProvider provider)
        {
            _client = provider.GetRequiredService<DiscordShardedClient>();
            _commands = provider.GetRequiredService<CommandService>();
        }

        [Command("BadgeList", true)]
        public async Task BadgeListCommandAsync()
        {
            var result = new List<string>();
            var row = 0;
            foreach (UserBadges badge in Enum.GetValues(typeof(UserBadges)))
            {
                result.Add($"[{row}] {badge.GetBadgeEmoji()}");
                row++;
            }
            var embed = EmbedFactory.CreateInfoEmbed(string.Join("\n", result));
            await ReplyAsync(embed: embed);
        }

        [Command("GiveBadge", true)]
        public async Task GiveBadgeCommandAsync(
            [Summary("userId")] ulong userId,
            [Summary("badgeIndex")] int badgeIndex
            )
        {
            var badge = (UserBadges)(1 << (badgeIndex - 1));
            var dbUser = await UserDatabase.TryGetUser(userId);
            if (dbUser is null)
            {
                await ReplyAsync(embed: EmbedFactory.CreateErrorEmbed("Пользователь еще не создал аккаунт.", Language.Ru));
                return;
            }
            dbUser.Customization.AddBadge(badge);
            await UserDatabase.UpdateUser(
                userId,
                new UpdateDefinitionBuilder<UserData>().Set(x => x.Badges, dbUser.Customization.Badges));
            var embed = EmbedFactory.CreateInfoEmbed($"<@{userId}> + {badge.GetBadgeEmoji()}");
            await ReplyAsync(embed: embed);
        }

        [Command("RemoveBadge", true)]
        public async Task RemoveBadgeCommandAsync(
            [Summary("userId")] ulong userId,
            [Summary("badgeIndex")] int badgeIndex
            )
        {
            var badge = (UserBadges)(1 << (badgeIndex - 1));
            var dbUser = await UserDatabase.TryGetUser(userId);
            if (dbUser is null)
            {
                await ReplyAsync(embed: EmbedFactory.CreateErrorEmbed("Пользователь еще не создал аккаунт.", Language.Ru));
                return;
            }
            dbUser.Customization.RemoveBadge(badge);
            await UserDatabase.UpdateUser(
                userId,
                new UpdateDefinitionBuilder<UserData>().Set(x => x.Badges, dbUser.Customization.Badges));
            var embed = EmbedFactory.CreateInfoEmbed($"<@{userId}> - {badge.GetBadgeEmoji()}");
            await ReplyAsync(embed: embed);
        }
    }
}
