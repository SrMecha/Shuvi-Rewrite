using Discord.Interactions;
using Discord.WebSocket;
using Discord;

namespace Shuvi.Classes.Extensions
{
    public static class ClientExtension
    {
        public static async Task<SocketMessageComponent> WaitForButtonInteraction(this DiscordShardedClient client, IUserMessage message, ulong userId)
        {
            bool check(SocketInteraction inter)
            {
                if (inter.Type != InteractionType.MessageComponent) return false;
                return (inter as SocketMessageComponent)!.Message.Id == message.Id && inter.User.Id == userId;
            }
            SocketInteraction output = await InteractionUtility.WaitForInteractionAsync(client, new TimeSpan(0, 5, 0), check);
            return (output as SocketMessageComponent)!;
        }
    }
}
