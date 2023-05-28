using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Shuvi.Services.StaticServices.Event;

namespace Shuvi.Modules.Events
{
    public class GuildsEventModule : InteractionModuleBase<ShardedInteractionContext>
    {
        private readonly DiscordShardedClient _client;

        public GuildsEventModule(IServiceProvider provider)
        {
            _client = provider.GetRequiredService<DiscordShardedClient>();
            _client.JoinedGuild += JoinedGuild;
            _client.LeftGuild += LeftGuild;
        }

        private async Task JoinedGuild(SocketGuild guild)
        {
            EventManager.InvokeOnGuildEnter(guild);
        }
        private async Task LeftGuild(SocketGuild guild)
        {
            EventManager.InvokeOnGuildLeave(guild);
        }

    }
}
