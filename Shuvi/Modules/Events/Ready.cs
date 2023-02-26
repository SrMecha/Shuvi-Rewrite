using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Shuvi.Services.Сonfigurator;

namespace Shuvi.Modules.Events
{
    public class ReadyEventModule : InteractionModuleBase<ShardedInteractionContext>
    {
        private readonly DiscordShardedClient _client;

        public ReadyEventModule(IServiceProvider provider)
        {
            _client = provider.GetRequiredService<DiscordShardedClient>();
            _client.ShardReady += ShardReadyAsync;
        }

        private async Task ShardReadyAsync(DiscordSocketClient shard)
        {
            await ServiceConfigurator.ConfigureLogs(_client);
            Console.WriteLine($"Шард №{shard.ShardId} Подключён.");
        }

    }
}
