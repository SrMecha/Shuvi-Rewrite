using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Shuvi.Classes.Types.Interaction;
using Shuvi.CommandParts;
using Shuvi.Services.StaticServices.Database;
using Shuvi.Services.StaticServices.Localization;

namespace Shuvi.Modules.SlashCommands
{
    public class TravelCommandModule : InteractionModuleBase<CustomInteractionContext>
    {
        private readonly LocalizationLanguagePart _errorPart = LocalizationService.Get("errorPart");
        private readonly DiscordShardedClient _client;

        public TravelCommandModule(IServiceProvider provider)
        {
            _client = provider.GetRequiredService<DiscordShardedClient>();
        }

        [SlashCommand("travel", "Перейти в другой регион или локацию.")]
        public async Task TravelCommandAsync()
        {
            await DeferAsync();
            var dbUser = await UserDatabase.TryGetUser(Context.User.Id);
            if (dbUser is null)
            {
                await AccountCreatePart.Start(Context);
                return;
            }
            Context.SetLanguage(dbUser);
            await TravelPart.Start(Context, dbUser);
        }
    }
}
