using Discord;
using Shuvi.Classes.Settings;
using Shuvi.Enums.Premium;
using Shuvi.Interfaces.User;

namespace Shuvi.Classes.Factories.CustomEmbed
{
    public static class EmbedFactory
    {
        public static EmbedBuilder CreateUserEmbed(IUser discordUser)
        {
            return new EmbedBuilder()
                .WithColor(CustomizationSettings.StandartColor)
                .WithFooter($"{discordUser.Username}#{discordUser.Discriminator} | {discordUser.Id}");
        }
        public static EmbedBuilder CreateUserEmbed(IUser discordUser, IDatabaseUser dbUser, bool withAvatar = true, bool withBanner = true)
        {
            var result = new EmbedBuilder()
                .WithColor(dbUser.Premium.HaveAbility(PremiumAbilities.ChangeColor) ? dbUser.Customization.Color : CustomizationSettings.StandartColor)
                .WithFooter($"{discordUser.Username}#{discordUser.Discriminator} | {discordUser.Id}");
            if (withAvatar && dbUser.Customization.Avatar is not null)
                result.WithThumbnailUrl(dbUser.Customization.Avatar.URL);
            if (withBanner && dbUser.Customization.Banner is not null)
                result.WithThumbnailUrl(dbUser.Customization.Banner.URL);
            return result;
        }
    }
}
