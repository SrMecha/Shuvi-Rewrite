using Discord;
using Shuvi.Classes.Settings;
using Shuvi.Enums.Localization;
using Shuvi.Enums.Premium;
using Shuvi.Interfaces.User;
using Shuvi.Services.StaticServices.Image;
using Shuvi.Services.StaticServices.Localization;

namespace Shuvi.Classes.Factories.CustomEmbed
{
    public static class EmbedFactory
    {
        private static readonly Color _errorColor = new(197, 67, 43);
        private static readonly LocalizationLanguagePart _errorLocalization = LocalizationService.Get("errorPart");

        public static EmbedBuilder CreateEmbed()
        {
            return new EmbedBuilder()
                .WithColor(CustomizationSettings.StandartColor);
        }
        public static EmbedBuilder CreateBotOwnerEmbed()
        {
            return new EmbedBuilder()
                .WithColor(CustomizationSettings.StandartColor)
                .WithFooter($"Sr. Mecha © 2023 • All rights reserved", ImageService.BotOwnerAvatartURL);
        }

        public static EmbedBuilder CreateUserEmbed(IDatabaseUser dbUser, bool withAvatar = true, bool withBanner = true)
        {
            var result = new EmbedBuilder()
                .WithColor(dbUser.Premium.HaveAbility(PremiumAbilities.ChangeColor) ? dbUser.Customization.Color : CustomizationSettings.StandartColor);
            if (withAvatar && dbUser.Customization.Avatar is not null)
                result.WithThumbnailUrl(dbUser.Customization.Avatar.URL);
            if (withBanner && dbUser.Customization.Banner is not null)
                result.WithImageUrl(dbUser.Customization.Banner.URL);
            return result;
        }

        public static Embed CreateErrorEmbed(string description, Language lang)
        {
            return new EmbedBuilder()
                .WithAuthor(_errorLocalization.Get(lang).Get("Embed/Error"))
                .WithDescription(description)
                .WithColor(_errorColor)
                .Build();
        }

        public static Embed CreateInfoEmbed(string description)
        {
            return new EmbedBuilder()
                .WithDescription(description)
                .WithColor(Color.Blue)
                .Build();
        }

        public static Embed CreateInfoEmbed(string description, string title)
        {
            return new EmbedBuilder()
                .WithAuthor(title)
                .WithDescription(description)
                .WithColor(Color.Blue)
                .Build();
        }
    }
}
