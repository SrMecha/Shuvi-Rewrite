using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Shuvi.Classes.Types.Interaction;
using Shuvi.CommandParts;
using Shuvi.Services.StaticServices.Database;
using Shuvi.Services.StaticServices.Localization;

namespace Shuvi.Modules.SlashCommands
{
    public class ProfileCommandModule : InteractionModuleBase<CustomInteractionContext>
    {
        private readonly DiscordShardedClient _client;

        public ProfileCommandModule(IServiceProvider provider)
        {
            _client = provider.GetRequiredService<DiscordShardedClient>();
        }

        [SlashCommand("profile", "Информация о игроке")]
        public async Task ProfileCommandAsync([Summary("user", "Выберите пользователя.")] IUser? paramUser = null)
        {
            await DeferAsync();
            var localization = LocalizationService.Get("profilePart").Get(Context.Language);
            paramUser ??= Context.User;
            var isOwnProfile = paramUser.Id == Context.User.Id;
            var dbUser = await UserDatabase.TryGetUser(paramUser is null ? Context.User.Id : paramUser.Id);
            var user = paramUser ?? Context.User;
            if (dbUser is null)
            {
                if (isOwnProfile)
                {
                    await AccountCreatePart.Start(Context);
                    return;
                }
                await Context.SendError(localization.Get("error/accountNotFound"), Context.Language);
                return;
            }
            await UserProfilePart.Start(Context, dbUser!, user, isOwnProfile);
        }
    }
}
