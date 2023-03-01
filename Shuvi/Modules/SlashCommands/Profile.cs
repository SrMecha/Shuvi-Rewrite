﻿using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Shuvi.Classes.Types.Interaction;

namespace Shuvi.Modules.SlashCommands
{
    public class ProfileCommandModule : InteractionModuleBase<CustomInteractionContext>
    {
        private readonly DiscordShardedClient _client;

        public ProfileCommandModule(IServiceProvider provider)
        {
            _client = provider.GetRequiredService<DiscordShardedClient>();
        }

        [SlashCommand("profile", "Информаиця о игроке")]
        public async Task ProfileCommandAsync([Summary("user", "Выберите пользователя.")] IUser? paramUser = null)
        {
            
        }
    }
}