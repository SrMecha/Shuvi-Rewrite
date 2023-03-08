using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Shuvi.Classes.Types.Interaction;
using Shuvi.CommandParts;
using Shuvi.Services.StaticServices.Database;

namespace Shuvi.Modules.SlashCommands
{
    public class InventoryCommandModule : InteractionModuleBase<CustomInteractionContext>
    {
        private readonly DiscordShardedClient _client;

        public InventoryCommandModule(IServiceProvider provider)
        {
            _client = provider.GetRequiredService<DiscordShardedClient>();
        }

        [SlashCommand("inventory", "Посмотреть инвентарь.")]
        public async Task ProfileCommandAsync()
        {
            await DeferAsync();
            var dbUser = await UserDatabase.TryGetUser(Context.User.Id);
            if (dbUser is null)
            {
                await AccountCreatePart.Start(Context);
                return;
            }
            await InventoryPart.Start(Context, dbUser!);
            if (Context.LastInteraction is not null)
                await DeleteOriginalResponseAsync();
        }
    }
}
