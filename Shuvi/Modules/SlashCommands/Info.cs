using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Shuvi.Classes.Types.Interaction;
using Shuvi.CommandParts;
using Shuvi.Services.StaticServices.Database;
using Shuvi.Services.StaticServices.Localization;

namespace Shuvi.Modules.SlashCommands
{
    public class InfoCommandModule : InteractionModuleBase<CustomInteractionContext>
    {
        private readonly LocalizationLanguagePart _errorPart = LocalizationService.Get("errorPart");
        private readonly DiscordShardedClient _client;

        public InfoCommandModule(IServiceProvider provider)
        {
            _client = provider.GetRequiredService<DiscordShardedClient>();
        }

        [SlashCommand("info", "Информация о боте.")]
        public async Task InfoCommandAsync()
        {
            await DeferAsync();
            var dbUser = await UserDatabase.TryGetUser(Context.User.Id);
            if (dbUser is null)
            {
                await AccountCreatePart.Start(Context);
                return;
            }
            Context.SetLanguage(dbUser);
            await BotInfoPart.Start(Context, dbUser);
        }
    }
}
