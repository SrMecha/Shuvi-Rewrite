﻿using Discord;
using MongoDB.Driver;
using Shuvi.Classes.Data.User;
using Shuvi.Classes.Extensions;
using Shuvi.Classes.Factories.CustomEmbed;
using Shuvi.Classes.Types.Interaction;
using Shuvi.Enums.Image;
using Shuvi.Enums.Localization;
using Shuvi.Interfaces.Customization;
using Shuvi.Interfaces.User;
using Shuvi.Services.StaticServices.Database;
using Shuvi.Services.StaticServices.Emoji;
using Shuvi.Services.StaticServices.Localization;

namespace Shuvi.CommandParts
{
    public static class CustomizationPart
    {
        private static readonly LocalizationLanguagePart _localizationPart = LocalizationService.Get("customizationPart");

        public static async Task Start(CustomInteractionContext context, IDatabaseUser dbUser)
        {
            var custLocalization = _localizationPart.Get(context.Language);
            while (context.LastInteraction is not null)
            {
                var embed = EmbedFactory.CreateUserEmbed(dbUser)
                    .WithAuthor($"{custLocalization.Get("Embed/Author/WithUser").Format(context.User.Username)}")
                    .WithDescription($"{custLocalization.Get("Embed/Current/Avatar").Format(dbUser.Customization.Avatar is null ?
                    custLocalization.Get("Embed/DontHave") : dbUser.Customization.Avatar.Info.GetName(context.Language))}\n" +
                    $"{custLocalization.Get("Embed/Current/Banner").Format(dbUser.Customization.Banner is null ?
                    custLocalization.Get("Embed/DontHave") : dbUser.Customization.Banner.Info.GetName(context.Language))}")
                    .Build();
                var components = new ComponentBuilder()
                    .WithButton(custLocalization.Get("Category/Avatar"), "avatar", ButtonStyle.Primary, row: 0)
                    .WithButton(custLocalization.Get("Category/Banner"), "banner", ButtonStyle.Primary, row: 0)
                    .WithButton(custLocalization.Get("Btn/Exit"), "exit", ButtonStyle.Danger, row: 1)
                    .Build();
                await context.Interaction.ModifyOriginalResponseAsync(msg => { msg.Embed = embed; msg.Components = components; });
                await context.LastInteraction.TryDeferAsync();
                var interaction = await context.WaitForButton();
                if (interaction is null)
                {
                    await context.CurrentMessage!.RemoveButtonsAsync();
                    return;
                }
                switch (interaction.Data.CustomId)
                {
                    case "avatar":
                        await AvatarPart(context, dbUser);
                        break;
                    case "banner":
                        await BannerPart(context, dbUser);
                        break;
                    case "exit":
                        return;
                }
            }
        }

        private static async Task AvatarPart(CustomInteractionContext context, IDatabaseUser dbUser)
        {
            var custLocalization = _localizationPart.Get(context.Language);
            var currentPage = 0;
            var maxPage = dbUser.Customization.Avatars.Count / 10;
            var arrow = 0;
            while (context.LastInteraction is not null)
            {
                var haveCustomization = HaveCustomization(dbUser.Customization.Avatars);
                var embedBuilder = EmbedFactory.CreateUserEmbed(dbUser)
                    .WithAuthor($"{custLocalization.Get("Embed/Author/Single")} | {custLocalization.Get("Category/Avatar")}")
                    .WithDescription($"{custLocalization.Get("Embed/Desc/Avatar").Format(dbUser.Customization.Avatar is null ?
                    custLocalization.Get("Embed/DontHave") : dbUser.Customization.Avatar.Info.GetName(context.Language))}\n\n" +
                    $"{GetCustomizationsString(dbUser.Customization.Avatars, context.Language, currentPage, arrow)}")
                    .WithFooter(custLocalization.Get("Embed/Page").Format(currentPage + 1, maxPage + 1));
                if (haveCustomization)
                    embedBuilder.WithThumbnailUrl(GetCurrentImage(dbUser.Customization.Avatars, currentPage, arrow).URL);
                var components = new ComponentBuilder()
                    .WithSelectMenu("select", GetCustomizationOptions(dbUser.Customization.Avatars, context.Language, currentPage, arrow),
                    custLocalization.Get("Select/Name/Avatar"), disabled: !haveCustomization, row: 0)
                    .WithButton("<", "<", ButtonStyle.Primary, disabled: currentPage < 1, row: 1)
                    .WithButton(custLocalization.Get("Btn/Choose"), "equip", ButtonStyle.Primary, disabled: !haveCustomization, row: 1)
                    .WithButton(">", ">", ButtonStyle.Primary, disabled: currentPage >= maxPage, row: 1)
                    .WithButton(custLocalization.Get("Btn/Back"), "exit", ButtonStyle.Danger, row: 2)
                    .WithButton(custLocalization.Get("Btn/Remove/Avatar"), "remove", ButtonStyle.Secondary,
                    disabled: dbUser.Customization.Avatar is null, row: 2)
                    .Build();
                await context.Interaction.ModifyOriginalResponseAsync(msg => { msg.Embed = embedBuilder.Build(); msg.Components = components; });
                await context.LastInteraction.TryDeferAsync();
                var interaction = await context.WaitForButton();
                if (interaction is null)
                {
                    await context.CurrentMessage!.RemoveButtonsAsync();
                    return;
                }
                switch (interaction.Data.CustomId)
                {
                    case "select":
                        arrow = int.Parse(interaction.Data.Values.First());
                        break;
                    case "<":
                        arrow = 0;
                        currentPage--;
                        break;
                    case "equip":
                        var image = GetCurrentImage(dbUser.Customization.Avatars, currentPage, arrow);
                        dbUser.Customization.SetImage(image.Type, image.Id);
                        await UserDatabase.UpdateUser(dbUser.Id, new UpdateDefinitionBuilder<UserData>()
                            .Set(x => x.Avatar, image.Id));
                        return;
                    case ">":
                        arrow = 0;
                        currentPage++;
                        break;
                    case "exit":
                        return;
                    case "remove":
                        dbUser.Customization.SetImage(ImageType.Avatar, null);
                        await UserDatabase.UpdateUser(dbUser.Id, new UpdateDefinitionBuilder<UserData>()
                            .Set(x => x.Avatar, null));
                        return;

                }
            }
        }

        private static async Task BannerPart(CustomInteractionContext context, IDatabaseUser dbUser)
        {
            var custLocalization = _localizationPart.Get(context.Language);
            var currentPage = 0;
            var maxPage = dbUser.Customization.Banners.Count / 10;
            var arrow = 0;
            while (context.LastInteraction is not null)
            {
                var haveCustomization = HaveCustomization(dbUser.Customization.Banners);
                var embedBuilder = EmbedFactory.CreateUserEmbed(dbUser)
                    .WithAuthor($"{custLocalization.Get("Embed/Author/Single")} | {custLocalization.Get("Category/Banner")}")
                    .WithDescription($"{custLocalization.Get("Embed/Desc/Banner").Format(dbUser.Customization.Banner is null ?
                    custLocalization.Get("Embed/DontHave") : dbUser.Customization.Banner.Info.GetName(context.Language))}\n\n" +
                    $"{GetCustomizationsString(dbUser.Customization.Banners, context.Language, currentPage, arrow)}")
                    .WithFooter(custLocalization.Get("Embed/Page").Format(currentPage + 1, maxPage + 1));
                if (haveCustomization)
                    embedBuilder.WithImageUrl(GetCurrentImage(dbUser.Customization.Banners, currentPage, arrow).URL);
                var components = new ComponentBuilder()
                    .WithSelectMenu("select", GetCustomizationOptions(dbUser.Customization.Banners, context.Language, currentPage, arrow),
                    custLocalization.Get("Select/Name/Banner"), disabled: !haveCustomization, row: 0)
                    .WithButton("<", "<", ButtonStyle.Primary, disabled: currentPage < 1, row: 1)
                    .WithButton(custLocalization.Get("Btn/Choose"), "equip", ButtonStyle.Primary, disabled: !haveCustomization, row: 1)
                    .WithButton(">", ">", ButtonStyle.Primary, disabled: currentPage >= maxPage, row: 1)
                    .WithButton(custLocalization.Get("Btn/Back"), "exit", ButtonStyle.Danger, row: 2)
                    .WithButton(custLocalization.Get("Btn/Remove/banner"), "remove", ButtonStyle.Secondary,
                    disabled: dbUser.Customization.Banner is null, row: 2)
                    .Build();
                await context.Interaction.ModifyOriginalResponseAsync(msg => { msg.Embed = embedBuilder.Build(); msg.Components = components; });
                await context.LastInteraction.TryDeferAsync();
                var interaction = await context.WaitForButton();
                if (interaction is null)
                {
                    await context.CurrentMessage!.RemoveButtonsAsync();
                    return;
                }
                switch (interaction.Data.CustomId)
                {
                    case "select":
                        arrow = int.Parse(interaction.Data.Values.First());
                        break;
                    case "<":
                        arrow = 0;
                        currentPage--;
                        break;
                    case "equip":
                        var image = GetCurrentImage(dbUser.Customization.Banners, currentPage, arrow);
                        dbUser.Customization.SetImage(image.Type, image.Id);
                        await UserDatabase.UpdateUser(dbUser.Id, new UpdateDefinitionBuilder<UserData>()
                            .Set(x => x.Banner, image.Id));
                        return;
                    case ">":
                        arrow = 0;
                        currentPage++;
                        break;
                    case "exit":
                        return;
                    case "remove":
                        dbUser.Customization.SetImage(ImageType.Banner, null);
                        await UserDatabase.UpdateUser(dbUser.Id, new UpdateDefinitionBuilder<UserData>()
                            .Set(x => x.Banner, null));
                        return;

                }
            }
        }

        private static IImage GetCurrentImage(List<IImage> images, int page, int arrow)
        {
            return images[page * 10 + arrow];
        }

        private static string GetCustomizationsString(List<IImage> images, Language lang, int page, int arrow)
        {
            var row = 0;
            var result = new List<string>();
            for (int i = page * 10; i < images.Count && i < page * 10 + 10; i++)
            {
                result.Add(row == arrow ? $"{EmojiService.Get("ChoosePoint")}{images[i].Info.GetName(lang)}" : images[i].Info.GetName(lang));
                row++;
            }
            return result.Count < 1 ? _localizationPart.Get(lang).Get("List/Empty") : string.Join("\n", result);
        }

        private static List<SelectMenuOptionBuilder> GetCustomizationOptions(List<IImage> images, Language lang, int page, int arrow)
        {
            var result = new List<SelectMenuOptionBuilder>();
            var row = 0;
            for (int i = page * 10; i < images.Count && i < page * 10 + 10; i++)
            {
                result.Add(new(images[i].Info.GetName(lang), row.ToString()));
                row++;
            }
            if (result.Count < 1)
                result.Add(new(_localizationPart.Get(lang).Get("List/Empty"), "Empty"));
            return result;
        }

        private static bool HaveCustomization(List<IImage> images)
        {
            return images.Count > 0;
        }
    }
}
