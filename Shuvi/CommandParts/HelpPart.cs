using Discord;
using Shuvi.Classes.Extensions;
using Shuvi.Classes.Factories.CustomEmbed;
using Shuvi.Classes.Types.Interaction;
using Shuvi.Interfaces.User;
using Shuvi.Services.StaticServices.Info;
using Shuvi.Services.StaticServices.Localization;

public static class HelpPart
{
    private static readonly LocalizationLanguagePart _localizationPart = LocalizationService.Get("helpPart");

    public static async Task Start(CustomInteractionContext context, IDatabaseUser dbUser)
    {
        var helpLocalization = _localizationPart.Get(context.Language);
        var embed = EmbedFactory.CreateBotOwnerEmbed()
            .WithAuthor(helpLocalization.Get("Embed/Commands/Author"))
            .WithDescription(helpLocalization.Get("Embed/Commands/Desc"))
            .AddField(helpLocalization.Get("Embed/Commands/CommandList"),
            BotInfoService.CommandsDescription,
            true)
            .Build();
        var components = new ComponentBuilder()
            .WithButton(helpLocalization.Get("Btn/Support"), style: ButtonStyle.Link, url: "https://discord.gg/Thq3Bjvn2t")
            .Build();
        await context.Interaction.ModifyOriginalResponseAsync(msg => { msg.Embed = embed; msg.Components = components; });
        await context.LastInteraction.TryDeferAsync();
    }
}
