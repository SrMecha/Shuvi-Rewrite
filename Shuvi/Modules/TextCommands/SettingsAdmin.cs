using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Shuvi.Classes.Factories.CustomEmbed;
using Shuvi.Services.StaticServices.Info;

namespace Shuvi.Modules.TextCommands
{
    public class SettingsAdminCommandModule : ModuleBase<SocketCommandContext>
    {
        private readonly DiscordShardedClient _client;

        public SettingsAdminCommandModule(IServiceProvider provider)
        {
            _client = provider.GetRequiredService<DiscordShardedClient>();
        }

        [Command("SetVersion", true)]
        public async Task GiveItemCommandAsync([Summary("version")] string version)
        {
            await BotInfoService.SetVersion(version);
            var embed = EmbedFactory.CreateInfoEmbed($"Версия обновлена до **{version}**.");
            await ReplyAsync(embed: embed);
        }
    }
}
