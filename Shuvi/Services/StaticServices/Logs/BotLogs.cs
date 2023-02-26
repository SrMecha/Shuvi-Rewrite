using Discord.WebSocket;
using Shuvi.Classes.Data.Settings;

namespace Shuvi.Services.StaticServices.Logs
{
    public static class BotLogs
    {
        private static SocketTextChannel? _serverLogsChannel;
        private static SocketTextChannel? _userLogsChannel;

        public static void Init(LogsData data, DiscordShardedClient client)
        {
            if (_serverLogsChannel is not null || _userLogsChannel is not null)
                return;
            _serverLogsChannel = client.GetChannel(data.ServerLogChannelId) as SocketTextChannel;
            _userLogsChannel = client.GetChannel(data.UserLogChannelId) as SocketTextChannel;
        }
        public static async Task SendServerLog(string message)
        {
            if (_serverLogsChannel == null)
            {
                Console.WriteLine("Логи для серверов не настроены.");
                return;
            }
            await _serverLogsChannel.SendMessageAsync(message);
        }
        public static async Task SendUserLog(string message)
        {
            if (_userLogsChannel == null)
            {
                Console.WriteLine("Логи для пользователей не настроены.");
                return;
            }
            await _userLogsChannel.SendMessageAsync(message);
        }
    }
}
