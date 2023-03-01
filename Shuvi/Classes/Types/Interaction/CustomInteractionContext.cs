using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Shuvi.Classes.Extensions;

namespace Shuvi.Classes.Types.Interaction
{
    public class CustomInteractionContext : ShardedInteractionContext
    {
        public SocketInteraction LastInteraction { get; set; }
        public IUserMessage? CurrentMessage { get; set; }

        public CustomInteractionContext(DiscordShardedClient client, SocketInteraction interaction) : base(client, interaction)
        {
            LastInteraction = interaction;
            CurrentMessage = null;
        }
        public async Task<SocketMessageComponent?> TryWaitForButtonInteraction(IUserMessage? message = null, ulong? userId = null)
        {
            CurrentMessage ??= await LastInteraction.GetOriginalResponseAsync();
            var result =  await Client.WaitForButtonInteraction(message ?? CurrentMessage, userId ?? User.Id);
            LastInteraction = result;
            return result;
        }
    }
}
