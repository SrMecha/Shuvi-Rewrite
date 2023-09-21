using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Shuvi.Classes.Types.Interaction;
using Shuvi.CommandParts;
using Shuvi.Enums.Check;
using Shuvi.Services.StaticServices.Check;
using Shuvi.Services.StaticServices.Database;
using Shuvi.Services.StaticServices.Localization;

namespace Shuvi.Modules.SlashCommands
{
    public class UpgradeCommandModule : InteractionModuleBase<CustomInteractionContext>
    {
        private readonly LocalizationLanguagePart _errorPart = LocalizationService.Get("errorPart");
        private readonly DiscordShardedClient _client;

        public UpgradeCommandModule(IServiceProvider provider)
        {
            _client = provider.GetRequiredService<DiscordShardedClient>();
        }

        [SlashCommand("upgrade", "Повысить свои характеристики.")]
        public async Task UpgradeCommandAsync()
        {
            await DeferAsync();
            var dbUser = await UserDatabase.TryGetUser(Context.User.Id);
            if (dbUser is null)
            {
                await AccountCreatePart.Start(Context);
                return;
            }
            var errorLocalization = _errorPart.Get(Context.Language);
            if (UserCheckService.IsUseCommand(TrackedCommand.Upgrade, dbUser.Id))
            {
                await Context.SendError(errorLocalization.Get("AlreadyUseCommand"), Context.Language);
                return;
            }
            try
            {
                UserCheckService.AddUserToCommand(TrackedCommand.Upgrade, dbUser.Id);
                await UpgradePart.Start(Context, dbUser, true);
                UserCheckService.RemoveUserFromCommand(TrackedCommand.Upgrade, dbUser.Id);
            }
            catch
            {
                UserCheckService.RemoveUserFromCommand(TrackedCommand.Upgrade, dbUser.Id);
                throw;
            }
        }
    }
}
