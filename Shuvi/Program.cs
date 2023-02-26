using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Shuvi.Services.DiscordServices;
using Shuvi.Services.Сonfigurator;

namespace Shuvi
{
    public static class Pogram
    {
        public static void Main()
             => MainAsync()
                 .GetAwaiter()
                 .GetResult();

        public static async Task MainAsync()
        {
            ServiceConfigurator.Configure();
            var botToken = ServiceConfigurator.GetEnviroment("BotToken");
            if (botToken is null)
            {
                Console.WriteLine("Переменные среды не настроены (botToken)");
                return;
            }
            var config = new DiscordSocketConfig
            {
                AlwaysDownloadUsers = false,
                MessageCacheSize = 100,
                GatewayIntents = GatewayIntents.AllUnprivileged
            };

            var services = BuildServices(config);
            DiscordShardedClient client = services.GetRequiredService<DiscordShardedClient>();

            await services.GetRequiredService<InteractionHandlingService>()
                .InitializeAsync();
            await services.GetRequiredService<CommandHandlingService>()
                .InitializeAsync();

            await client.LoginAsync(TokenType.Bot, botToken);
            await client.StartAsync();

            await Task.Delay(Timeout.Infinite);
        }
        private static IServiceProvider BuildServices(DiscordSocketConfig config)
        {
            return new ServiceCollection()
                .AddSingleton(new DiscordShardedClient(config))
                .AddSingleton<CommandService>()
                .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordShardedClient>()))
                .AddSingleton<InteractionHandlingService>()
                .AddSingleton<CommandHandlingService>()
                .BuildServiceProvider();
        }
    }
}

