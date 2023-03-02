using Discord.WebSocket;

namespace Shuvi.Classes.Extensions
{
    public static class SocketInteractionExt
    {
        public static async Task TryDeferAsync(this SocketInteraction interaction)
        {
            try
            {
                await interaction.DeferAsync();
            }
            catch { }
        }
    }
}
