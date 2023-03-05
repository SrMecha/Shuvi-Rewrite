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
        private readonly LocalizationKeyPart _localization;

        public ProfileCommandModule(IServiceProvider provider)
        {
            _client = provider.GetRequiredService<DiscordShardedClient>();
            _localization = LocalizationService.Get("profilePart").Get(Context.Language);
        }

        [SlashCommand("profile", "Информаиця о игроке")]
        public async Task ProfileCommandAsync([Summary("user", "Выберите пользователя.")] IUser? paramUser = null)
        {
            await DeferAsync();
            paramUser = Context.User;
            var isOwnProfile = paramUser.Id == Context.User.Id;
            var dbUser = await UserDatabase.TryGetUser(Context.User.Id);
            if (dbUser is null)
            {
                if (isOwnProfile)
                {
                    await AccountCreatePart.Start(Context);
                    return;
                }
                await Context.SendError(_localization.Get("error/accountNotFound"), Context.Language);
                return;
            }
            await ProfilePart.Start(Context, dbUser!, isOwnProfile);
        }
    }
}
