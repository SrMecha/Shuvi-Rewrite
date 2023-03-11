using Discord.WebSocket;
using Shuvi.Classes.Factories.CustomEmbed;
using Shuvi.Enums.Localization;

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
        public static async Task SendError(this SocketInteraction interaction, string description, Language lang)
        {
            await interaction.RespondAsync(embed: EmbedFactory.CreateErrorEmbed(description, lang), ephemeral: true);
        }
        public static async Task SendInfo(this SocketInteraction interaction, string description, string title = "")
        {
            await interaction.RespondAsync(embed: EmbedFactory.CreateInfoEmbed(description, title), ephemeral: true);
        }
    }
}
