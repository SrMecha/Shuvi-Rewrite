using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Shuvi.Classes.Extensions;
using Shuvi.Classes.Types.Interaction;
using Shuvi.CommandParts;
using Shuvi.Enums.Top;
using Shuvi.Services.StaticServices.Database;
using Shuvi.Services.StaticServices.Localization;
using Shuvi.Services.StaticServices.Top;

namespace Shuvi.Modules.SlashCommands
{
    public class TopCommandModule : InteractionModuleBase<CustomInteractionContext>
    {
        private readonly LocalizationLanguagePart _errorPart = LocalizationService.Get("errorPart");
        private readonly DiscordShardedClient _client;

        public TopCommandModule(IServiceProvider provider)
        {
            _client = provider.GetRequiredService<DiscordShardedClient>();
        }

        [SlashCommand("top", "Посмотреть топ мира.")]
        public async Task TopCommandAsync([Summary("type", "Выберите тип топа.")] UserTopType topType = UserTopType.Rating)
        {
            await DeferAsync();
            var dbUser = await UserDatabase.TryGetUser(Context.User.Id);
            if (dbUser is null)
            {
                await AccountCreatePart.Start(Context);
                return;
            }
            if (!TopService.IsDataSet(topType))
            {
                await Context.SendError(_errorPart.Get(Context.Language).Get("topNotLoaded"), Context.Language);
                return;
            }
            await TopViewPart.Start(Context, dbUser, topType);
        }
    }
}
