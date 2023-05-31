using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Shuvi.Classes.Factories.CustomEmbed;
using Shuvi.Classes.Types.Interaction;
using Shuvi.CommandParts;
using Shuvi.Enums.Check;
using Shuvi.Services.StaticServices.Check;
using Shuvi.Services.StaticServices.Database;
using Shuvi.Services.StaticServices.Localization;

namespace Shuvi.Modules.SlashCommands
{
    public class ShopCommandModule : InteractionModuleBase<CustomInteractionContext>
    {
        private readonly LocalizationLanguagePart _errorPart = LocalizationService.Get("errorPart");
        private readonly DiscordShardedClient _client;

        public ShopCommandModule(IServiceProvider provider)
        {
            _client = provider.GetRequiredService<DiscordShardedClient>();
        }

        [SlashCommand("shop", "Сходить в магазин.")]
        public async Task ShopCommandAsync()
        {
            await DeferAsync();
            var dbUser = await UserDatabase.TryGetUser(Context.User.Id);
            if (dbUser is null)
            {
                await AccountCreatePart.Start(Context);
                return;
            }
            var errorLocalization = _errorPart.Get(Context.Language);
            if (UserCheckService.IsUseCommand(TrackedCommand.Shop, dbUser.Id))
            {
                await Context.SendError(errorLocalization.Get("alreadyUseCommand"), Context.Language);
                return;
            }
            if (dbUser.Location.GetLocation().Shops.Count < 1)
            {
                await Context.Interaction.ModifyOriginalResponseAsync(msg =>
                {
                    msg.Embed = EmbedFactory.CreateUserEmbed(Context.User, dbUser).WithDescription(errorLocalization.Get("dontHaveShops")).Build();
                });
                return;
            }
            try
            {
                UserCheckService.AddUserToCommand(TrackedCommand.Shop, dbUser.Id);
                await ShopPart.Start(Context, dbUser);
                UserCheckService.RemoveUserFromCommand(TrackedCommand.Shop, dbUser.Id);
            }
            catch
            {
                UserCheckService.RemoveUserFromCommand(TrackedCommand.Shop, dbUser.Id);
                throw;
            }
        }
    }
}
