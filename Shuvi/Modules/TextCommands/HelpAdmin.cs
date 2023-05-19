using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Shuvi.Classes.Factories.CustomEmbed;

namespace Shuvi.Modules.TextCommands
{
    public class HelpAdminCommandModule : ModuleBase<SocketCommandContext>
    {
        private readonly DiscordShardedClient _client;
        private readonly CommandService _commands;

        public HelpAdminCommandModule(IServiceProvider provider)
        {
            _client = provider.GetRequiredService<DiscordShardedClient>();
            _commands = provider.GetRequiredService<CommandService>();
        }

        [Command("Help", true)]
        public async Task GiveItemCommandAsync()
        {
            var result = new List<string>();

            foreach (var command in _commands.Commands.ToList())
            {
                var commandParams = new List<string>();
                foreach (var param in command.Parameters)
                {
                    if (param.IsOptional)
                        commandParams.Add($"[{param.Name}]");
                    else
                        commandParams.Add($"<{param.Name}>");
                }
                result.Add($"**{command.Name}** {(commandParams.Count != 0 ? string.Join(" ", commandParams) : "")}");
            }
            var embed = EmbedFactory.CreateInfoEmbed(string.Join("\n", result));
            await ReplyAsync(embed: embed);
        }
    }
}
