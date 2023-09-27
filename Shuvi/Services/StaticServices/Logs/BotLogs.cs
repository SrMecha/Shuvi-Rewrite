using Discord;
using Discord.WebSocket;
using Shuvi.Classes.Data.Settings;
using Shuvi.Classes.Extensions;
using Shuvi.Enums.Rating;
using Shuvi.Interfaces.User;
using Shuvi.Services.StaticServices.Emoji;
using Shuvi.Services.StaticServices.Event;

namespace Shuvi.Services.StaticServices.Logs
{
    public static class BotLogs
    {
        private static DiscordShardedClient? _client;
        private static SocketTextChannel? _serverLogsChannel;
        private static SocketTextChannel? _userLogsChannel;

        public static void Init(LogsData data, DiscordShardedClient client)
        {
            _client = client;
            if (_serverLogsChannel is not null || _userLogsChannel is not null)
                return;
            _serverLogsChannel = client.GetChannel(data.ServerLogChannelId) as SocketTextChannel;
            _userLogsChannel = client.GetChannel(data.UserLogChannelId) as SocketTextChannel;
            EventManager.OnAccountCreate += OnAccountCreate;
            EventManager.OnPlayerDie += OnPlayerDie;
            EventManager.OnPlayerRankUp += OnPlayerRankUp;
            EventManager.OnGuildEnter += OnGuildEnter;
            EventManager.OnGuildLeave += OnGuildLeave;

        }

        private static async ValueTask OnAccountCreate(IUser user)
        {
            if (_userLogsChannel == null)
            {
                Console.WriteLine("Логи для пользователей не настроены.");
                return;
            }
            await _userLogsChannel.SendMessageAsync($"[{EmojiService.Get("NewPlayer")}] {user.Username}#{user.Discriminator}\n`id: {user.Id}`");
        }

        private static async ValueTask OnPlayerDie(IDatabaseUser dbUser)
        {
            if (_userLogsChannel == null)
            {
                Console.WriteLine("Логи для пользователей не настроены.");
                return;
            }
            var user = _client!.GetUser(dbUser.Id);
            await _userLogsChannel.SendMessageAsync($"[{EmojiService.Get("PlayerDead")}] [{dbUser.Rating.Rank.GetName()}]" +
                $"{user.Username}#{user.Discriminator}\n`id: {user.Id}`");
        }

        private static async ValueTask OnPlayerRankUp(IDatabaseUser dbUser, Rank rankBefore, Rank rankAfter)
        {
            if (_userLogsChannel == null)
            {
                Console.WriteLine("Логи для пользователей не настроены.");
                return;
            }
            var user = _client!.GetUser(dbUser.Id);
            await _userLogsChannel.SendMessageAsync($"[{EmojiService.Get("RankUp")}] [{rankBefore.GetName()} -> {rankAfter.GetName()}] " +
                $"{user.Username}#{user.Discriminator}\n`id: {user.Id}`");
        }

        private static async ValueTask OnGuildEnter(SocketGuild guild)
        {
            if (_serverLogsChannel == null)
            {
                Console.WriteLine("Логи для серверов не настроены.");
                return;
            }
            await _serverLogsChannel.SendMessageAsync($"[{EmojiService.Get("GuildEnter")}] {guild.Name} | {guild.MemberCount} Уч.\n" +
                $"`id: {guild.Id}`");
        }

        private static async ValueTask OnGuildLeave(SocketGuild guild)
        {
            if (_serverLogsChannel == null)
            {
                Console.WriteLine("Логи для серверов не настроены.");
                return;
            }
            await _serverLogsChannel.SendMessageAsync($"[{EmojiService.Get("GuildLeave")}] {guild.Name} | {guild.MemberCount} Уч.\n" +
                $"`id: {guild.Id}`");
        }
    }
}
