using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Shuvi.Classes.Extensions;
using Shuvi.Classes.Factories.CustomEmbed;
using Shuvi.Enums.Localization;

namespace Shuvi.Classes.Types.Interaction
{
    public class CustomInteractionContext : ShardedInteractionContext
    {
        public SocketInteraction LastInteraction { get; set; }
        public IUserMessage? CurrentMessage { get; set; }
        public Language Language { get; init; }

        public CustomInteractionContext(DiscordShardedClient client, SocketInteraction interaction) : base(client, interaction)
        {
            LastInteraction = interaction;
            CurrentMessage = null;
            Language = interaction.UserLocale.AsLanguage();
        }
        public async Task<SocketMessageComponent?> WaitForButton(IUserMessage? message = null, ulong? userId = null)
        {
            CurrentMessage ??= await LastInteraction.GetOriginalResponseAsync();
            var result = await Client.WaitForButtonInteraction(message ?? CurrentMessage, userId ?? User.Id);
            LastInteraction = result;
            return result;
        }
        public async Task SendError(Embed embed)
        {
            if (!LastInteraction.HasResponded)
                await LastInteraction.RespondAsync(embed: embed, ephemeral: true);
            else
                await Interaction.ModifyOriginalResponseAsync(msg => { msg.Embed = embed; });
        }
        public async Task SendError(string description, Language lang)
        {
            if (!LastInteraction.HasResponded)
                await LastInteraction.RespondAsync(embed: EmbedFactory.CreateErrorEmbed(description, lang), ephemeral: true);
            else
                await Interaction.ModifyOriginalResponseAsync(msg => { msg.Embed = EmbedFactory.CreateErrorEmbed(description, lang); });
        }
    }
}
