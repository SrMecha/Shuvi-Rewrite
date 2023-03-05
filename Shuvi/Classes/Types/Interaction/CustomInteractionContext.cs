using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Shuvi.Classes.Extensions;
using Shuvi.Classes.Factories.CustomEmbed;
using Shuvi.Enums.Localization;
using static MongoDB.Bson.Serialization.Serializers.SerializerHelper;

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
            await LastInteraction.RespondAsync(embed: embed, ephemeral: true);
        }
        public async Task SendError(string description, Language lang)
        {
            await LastInteraction.RespondAsync(embed: EmbedFactory.CreateErrorEmbed(description, lang), ephemeral: true);
        }
    }
}
