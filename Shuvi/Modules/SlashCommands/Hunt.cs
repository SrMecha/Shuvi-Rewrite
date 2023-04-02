using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Shuvi.Classes.Extensions;
using Shuvi.Classes.Factories.CustomEmbed;
using Shuvi.Classes.Types.Interaction;
using Shuvi.CommandParts;
using Shuvi.Enums.Check;
using Shuvi.Services.StaticServices.Check;
using Shuvi.Services.StaticServices.Database;
using Shuvi.Services.StaticServices.Localization;

namespace Shuvi.Modules.SlashCommands
{
    public class HuntCommandModule : InteractionModuleBase<CustomInteractionContext>
    {
        private readonly LocalizationLanguagePart _localizationPart = LocalizationService.Get("huntPart");
        private readonly DiscordShardedClient _client;

        public HuntCommandModule(IServiceProvider provider)
        {
            _client = provider.GetRequiredService<DiscordShardedClient>();
        }

        [SlashCommand("hunt", "Охота на монстров.")]
        public async Task HuntCommandAsync()
        {
            await DeferAsync();
            var dbUser = await UserDatabase.TryGetUser(Context.User.Id);
            if (dbUser is null)
            {
                await AccountCreatePart.Start(Context);
                return;
            }
            var huntLocalization = _localizationPart.Get(Context.Language);
            if (!dbUser.Characteristics.HaveEnergy(HuntPart.HuntEnergyCost))
            {
                await Context.SendError(huntLocalization.Get("error/dontHaveEnergy"), Context.Language);
                return;
            }
            if (UserCheckService.IsUseCommand(TrackedCommand.Hunt, dbUser.Id))
            {
                await Context.SendError(huntLocalization.Get("error/alreadyUse"), Context.Language);
                return;
            }
            if (!dbUser.Location.GetLocation().Enemies.HaveEnemies())
            {
                await Context.Interaction.ModifyOriginalResponseAsync(msg => { 
                    msg.Embed = EmbedFactory.CreateUserEmbed(Context.User, dbUser).WithDescription(huntLocalization.Get("error/dontHaveEnemies")).Build(); 
                });
                return;
            }
            try
            {
                UserCheckService.AddUserToCommand(TrackedCommand.Hunt, dbUser.Id);
                await HuntPart.Start(Context, dbUser);
                UserCheckService.RemoveUserFromCommand(TrackedCommand.Hunt, dbUser.Id);
            }
            catch
            {
                UserCheckService.RemoveUserFromCommand(TrackedCommand.Hunt, dbUser.Id);
            }
        }
    }
}
