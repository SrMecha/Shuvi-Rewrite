using Discord;
using MongoDB.Bson;
using Shuvi.Classes.Extensions;
using Shuvi.Classes.Factories.CustomEmbed;
using Shuvi.Classes.Types.Interaction;
using Shuvi.Enums.Image;
using Shuvi.Enums.Localization;
using Shuvi.Interfaces.Shop;
using Shuvi.Interfaces.Shop.Products;
using Shuvi.Interfaces.Spell;
using Shuvi.Interfaces.User;
using Shuvi.Services.StaticServices.Database;
using Shuvi.Services.StaticServices.Emoji;
using Shuvi.Services.StaticServices.Localization;

namespace Shuvi.CommandParts
{
    public static class ShopPart
    {
        private static readonly LocalizationLanguagePart _localizationPart = LocalizationService.Get("shopPart");

        public static async Task Start(CustomInteractionContext context, IDatabaseUser dbUser)
        {
            var shopLocalization = _localizationPart.Get(context.Language);
            var embed = EmbedFactory.CreateUserEmbed(context.User, dbUser)
                .WithDescription(shopLocalization.Get("embed/choose/desc"))
                .Build();
            var components = new ComponentBuilder()
                .WithSelectMenu("select", GetShopOptions(dbUser.Location.GetLocation().Shops, context.Language), shopLocalization.Get("select/choose/name"))
                .WithButton(shopLocalization.Get("btn/exit"), "exit", ButtonStyle.Danger)
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
                case "exit":
                    await context.Interaction.DeleteOriginalResponseAsync();
                    return;
                case "select":
                    await ShopMain(context, dbUser, new ObjectId(interaction.Data.Values.First()));
                    break;
            }
        }

        public static async Task ShopMain(CustomInteractionContext context, IDatabaseUser dbUser, ObjectId shopId)
        {
            var shopLocalization = _localizationPart.Get(context.Language);
            var shop = ShopDatabase.CreateShop(shopId);
            var categoryOptions = new List<SelectMenuOptionBuilder>();
            if (shop.Purchasing.HaveProducts())
                categoryOptions.Add(new(shopLocalization.Get("category/purchasing"), "purchasing"));
            if (shop.Selling.HaveProducts())
                categoryOptions.Add(new(shopLocalization.Get("category/selling"), "selling"));
            if (shop.Customization.HaveProducts())
                categoryOptions.Add(new(shopLocalization.Get("category/customization"), "customization"));
            var part = categoryOptions.First().Value;
            while (true)
            {
                switch (part)
                {
                    case "exit":
                        return;
                    case "purchasing":
                        part = await PurchasingPart(context, dbUser, shop, categoryOptions);
                        break;
                    case "selling":
                        part = await SellingPart(context, dbUser, shop, categoryOptions);
                        break;
                    case "customization":
                        part = await CustomizationPart(context, dbUser, shop, categoryOptions);
                        break;
                    default:
                        return;
                }
            }
        }

        public static async Task<string> PurchasingPart(CustomInteractionContext context, IDatabaseUser dbUser, 
            IShop shop, List<SelectMenuOptionBuilder> categoryOptions)
        {
            var shopLocalization = _localizationPart.Get(context.Language);
            var namesLocalization = LocalizationService.Get("names").Get(context.Language);
            var currentPage = 0;
            var maxPage = shop.Purchasing.GetTotalPages();
            var arrow = 0;
            while (true)
            {
                var embed = EmbedFactory.CreateUserEmbed(context.User, dbUser)
                    .WithAuthor($"{shop.Info.GetName(context.Language)} | {shopLocalization.Get("category/purchasing")}")
                    .WithDescription($"{namesLocalization.Get("gold")}: {dbUser.Wallet.Gold} {shop.ShopBasket.Wallet.Gold.WithSign(true)} " +
                    $"{EmojiService.Get("gold")}\n" +
                    $"{namesLocalization.Get("dispoints")}: {dbUser.Wallet.Dispoints} {shop.ShopBasket.Wallet.Dispoints.WithSign(true)} " +
                    $"{EmojiService.Get("dispoints")}")
                    .AddField(shopLocalization.Get("embed/shop"),
                    GetPurchasingProductsString(dbUser, shop, currentPage, arrow, context.Language),
                    true)
                    .AddField(shopLocalization.Get("embed/basket"),
                    GetBasketProductsString(shop, context.Language),
                    true
                    )
                    .WithFooter($"{context.User.Username}#{context.User.Discriminator} | " +
                    $"{shopLocalization.Get("embed/page").Format(currentPage + 1, maxPage + 1)}")
                    .Build();
                var components = new ComponentBuilder()
                    .WithSelectMenu("category", categoryOptions, shopLocalization.Get("select/category/name"), row: 0)
                    .WithSelectMenu("item", GetItemOptions(shop.Purchasing.GetProductsInPage(currentPage), context.Language),
                    shopLocalization.Get("select/item/name"), row: 1)
                    .WithButton("x1", "1", ButtonStyle.Success, 
                    disabled: !shop.Purchasing.CanBuy(dbUser.Inventory, dbUser.Wallet, currentPage, arrow, 1), row: 2)
                    .WithButton("x2", "2", ButtonStyle.Success,
                    disabled: !shop.Purchasing.CanBuy(dbUser.Inventory, dbUser.Wallet, currentPage, arrow, 2), row: 2)
                    .WithButton("x5", "5", ButtonStyle.Success,
                    disabled: !shop.Purchasing.CanBuy(dbUser.Inventory, dbUser.Wallet, currentPage, arrow, 5), row: 2)
                    .WithButton("<", "<", ButtonStyle.Primary, disabled: !(currentPage > 0), row: 3)
                    .WithButton(shopLocalization.Get("btn/info"), "info", ButtonStyle.Primary, disabled: currentPage > 0, row: 3)
                    .WithButton(">", ">", ButtonStyle.Primary, disabled: !(currentPage < maxPage), row: 3)
                    .WithButton(shopLocalization.Get("btn/exit"), "exit", ButtonStyle.Danger, row: 4)
                    .WithButton(shopLocalization.Get("btn/clear"), "clear", ButtonStyle.Secondary, row: 4)
                    .WithButton(shopLocalization.Get("btn/confirm"), "confirm", ButtonStyle.Success, row: 4)
                    .Build();
                await context.Interaction.ModifyOriginalResponseAsync(msg => { msg.Embed = embed; msg.Components = components; });
                await context.LastInteraction.TryDeferAsync();
                var interaction = await context.WaitForButton();
                if (interaction is null)
                {
                    await context.CurrentMessage!.RemoveButtonsAsync();
                    return "exit";
                }
                switch (interaction.Data.CustomId)
                {
                    case "category":
                        return interaction.Data.Values.First();
                    case "item":
                        arrow = int.Parse(interaction.Data.Values.First());
                        break;
                    case "<":
                        arrow = 0;
                        currentPage--;
                        break;
                    case "info":
                        await ItemViewPart.Start(context, dbUser, shop.Purchasing.GetProduct(currentPage, arrow).Id, false);
                        break;
                    case ">":
                        arrow = 0;
                        currentPage++;
                        break;
                    case "exit":
                        embed = EmbedFactory.CreateUserEmbed(context.User, dbUser)
                            .WithAuthor(shopLocalization.Get("embed/exit/author"))
                            .Build();
                        await context.Interaction.ModifyOriginalResponseAsync(msg => {
                            msg.Embed = embed;
                            msg.Components = new ComponentBuilder().Build();
                        });
                        return "exit";
                    case "clear":
                        shop.ShopBasket.Clear();
                        break;
                    case "confirm":
                        embed = EmbedFactory.CreateUserEmbed(context.User, dbUser)
                            .WithAuthor(shopLocalization.Get("embed/exit/author"))
                            .WithDescription($"{namesLocalization.Get("gold")}: {dbUser.Wallet.Gold} -> " +
                            $"{shop.ShopBasket.Wallet.Gold + dbUser.Wallet.Gold} {EmojiService.Get("gold")}\n" +
                            $"{namesLocalization.Get("dispoints")}: {dbUser.Wallet.Dispoints} -> " +
                            $"{shop.ShopBasket.Wallet.Dispoints + dbUser.Wallet.Dispoints} {EmojiService.Get("dispoints")}")
                            .AddField(shopLocalization.Get("embed/confirm/results"),
                            GetBasketProductsString(shop, context.Language, 20))
                            .Build();
                        await context.Interaction.ModifyOriginalResponseAsync(msg => { 
                            msg.Embed = embed; 
                            msg.Components = new ComponentBuilder().Build(); 
                        });
                        await shop.Confirm(dbUser);
                        return "exit";
                    default:
                        shop.Purchasing.Buy(currentPage, arrow, int.Parse(interaction.Data.CustomId));
                        break;
                }
            }
        }

        public static async Task<string> SellingPart(CustomInteractionContext context, IDatabaseUser dbUser,
            IShop shop, List<SelectMenuOptionBuilder> categoryOptions)
        {
            var shopLocalization = _localizationPart.Get(context.Language);
            var namesLocalization = LocalizationService.Get("names").Get(context.Language);
            var currentPage = 0;
            var maxPage = shop.Purchasing.GetTotalPages();
            var arrow = 0;
            while (true)
            {
                var maxCanSell = shop.Selling.GetMaxSell(dbUser.Inventory, currentPage, arrow);
                var embed = EmbedFactory.CreateUserEmbed(context.User, dbUser)
                    .WithAuthor($"{shop.Info.GetName(context.Language)} | {shopLocalization.Get("category/selling")}")
                    .WithDescription($"{namesLocalization.Get("gold")}: {dbUser.Wallet.Gold} {shop.ShopBasket.Wallet.Gold.WithSign(true)} " +
                    $"{EmojiService.Get("gold")}\n" +
                    $"{namesLocalization.Get("dispoints")}: {dbUser.Wallet.Dispoints} {shop.ShopBasket.Wallet.Dispoints.WithSign(true)} " +
                    $"{EmojiService.Get("dispoints")}")
                    .AddField(shopLocalization.Get("embed/shop"),
                    GetSellingProductsString(dbUser, shop, currentPage, arrow, context.Language),
                    true)
                    .AddField(shopLocalization.Get("embed/basket"),
                    GetBasketProductsString(shop, context.Language),
                    true
                    )
                    .WithFooter($"{context.User.Username}#{context.User.Discriminator} | " +
                    $"{shopLocalization.Get("embed/page").Format(currentPage + 1, maxPage + 1)}")
                    .Build();
                var components = new ComponentBuilder()
                    .WithSelectMenu("category", categoryOptions, shopLocalization.Get("select/category/name"), row: 0)
                    .WithSelectMenu("item", GetItemOptions(shop.Selling.GetProductsInPage(currentPage), context.Language),
                    shopLocalization.Get("select/item/name"), row: 1)
                    .WithButton("x1", "1", ButtonStyle.Success,
                    disabled: !shop.Selling.CanSell(dbUser.Inventory, currentPage, arrow, 1), row: 2)
                    .WithButton("x2", "2", ButtonStyle.Success,
                    disabled: !shop.Selling.CanSell(dbUser.Inventory, currentPage, arrow, 2), row: 2)
                    .WithButton("x5", "5", ButtonStyle.Success,
                    disabled: !shop.Selling.CanSell(dbUser.Inventory, currentPage, arrow, 5), row: 2)
                    .WithButton($"MAX x{maxCanSell}", $"+{maxCanSell}", ButtonStyle.Success,
                    disabled: !shop.Selling.CanSell(dbUser.Inventory, currentPage, arrow, 1), row: 2)
                    .WithButton("<", "<", ButtonStyle.Primary, disabled: !(currentPage > 0), row: 3)
                    .WithButton(shopLocalization.Get("btn/info"), "info", ButtonStyle.Primary, disabled: currentPage > 0, row: 3)
                    .WithButton(">", ">", ButtonStyle.Primary, disabled: !(currentPage < maxPage), row: 3)
                    .WithButton(shopLocalization.Get("btn/exit"), "exit", ButtonStyle.Danger, row: 4)
                    .WithButton(shopLocalization.Get("btn/clear"), "clear", ButtonStyle.Secondary, row: 4)
                    .WithButton(shopLocalization.Get("btn/confirm"), "confirm", ButtonStyle.Success, row: 4)
                    .Build();
                await context.Interaction.ModifyOriginalResponseAsync(msg => { msg.Embed = embed; msg.Components = components; });
                await context.LastInteraction.TryDeferAsync();
                var interaction = await context.WaitForButton();
                if (interaction is null)
                {
                    await context.CurrentMessage!.RemoveButtonsAsync();
                    return "exit";
                }
                switch (interaction.Data.CustomId)
                {
                    case "category":
                        return interaction.Data.Values.First();
                    case "item":
                        arrow = int.Parse(interaction.Data.Values.First());
                        break;
                    case "<":
                        arrow = 0;
                        currentPage--;
                        break;
                    case "info":
                        await ItemViewPart.Start(context, dbUser, shop.Selling.GetProduct(currentPage, arrow).Id, false);
                        break;
                    case ">":
                        arrow = 0;
                        currentPage++;
                        break;
                    case "exit":
                        embed = EmbedFactory.CreateUserEmbed(context.User, dbUser)
                            .WithAuthor(shopLocalization.Get("embed/exit/author"))
                            .Build();
                        await context.Interaction.ModifyOriginalResponseAsync(msg => {
                            msg.Embed = embed;
                            msg.Components = new ComponentBuilder().Build();
                        });
                        return "exit";
                    case "clear":
                        shop.ShopBasket.Clear();
                        break;
                    case "confirm":
                        embed = EmbedFactory.CreateUserEmbed(context.User, dbUser)
                            .WithAuthor(shopLocalization.Get("embed/exit/author"))
                            .WithDescription($"{namesLocalization.Get("gold")}: {dbUser.Wallet.Gold} -> " +
                            $"{shop.ShopBasket.Wallet.Gold + dbUser.Wallet.Gold} {EmojiService.Get("gold")}\n" +
                            $"{namesLocalization.Get("dispoints")}: {dbUser.Wallet.Dispoints} -> " +
                            $"{shop.ShopBasket.Wallet.Dispoints + dbUser.Wallet.Dispoints} {EmojiService.Get("dispoints")}")
                            .AddField(shopLocalization.Get("embed/confirm/results"),
                            GetBasketProductsString(shop, context.Language, 20))
                            .Build();
                        await context.Interaction.ModifyOriginalResponseAsync(msg => {
                            msg.Embed = embed;
                            msg.Components = new ComponentBuilder().Build();
                        });
                        await shop.Confirm(dbUser);
                        return "exit";
                    default:
                        shop.Selling.Sell(currentPage, arrow, int.Parse(interaction.Data.CustomId));
                        break;
                }
            }
        }

        public static async Task<string> CustomizationPart(CustomInteractionContext context, IDatabaseUser dbUser,
            IShop shop, List<SelectMenuOptionBuilder> categoryOptions)
        {
            var shopLocalization = _localizationPart.Get(context.Language);
            var namesLocalization = LocalizationService.Get("names").Get(context.Language);
            var currentPage = 0;
            var maxPage = shop.Purchasing.GetTotalPages();
            var arrow = 0;
            while (true)
            {
                var product = shop.Customization.GetProduct(currentPage, arrow);
                var embedBuilder = EmbedFactory.CreateUserEmbed(context.User, dbUser)
                    .WithAuthor($"{shop.Info.GetName(context.Language)} | {shopLocalization.Get("category/customization")}")
                    .WithDescription($"{namesLocalization.Get("gold")}: {dbUser.Wallet.Gold} {shop.ShopBasket.Wallet.Gold.WithSign(true)} " +
                    $"{EmojiService.Get("gold")}\n" +
                    $"{namesLocalization.Get("dispoints")}: {dbUser.Wallet.Dispoints} {shop.ShopBasket.Wallet.Dispoints.WithSign(true)} " +
                    $"{EmojiService.Get("dispoints")}")
                    .AddField(shopLocalization.Get("embed/shop"),
                    GetCustomizationProductsString(shop, currentPage, arrow, context.Language),
                    true)
                    .AddField(shopLocalization.Get("embed/basket"),
                    GetBasketProductsString(shop, context.Language),
                    true
                    )
                    .WithFooter($"{context.User.Username}#{context.User.Discriminator} | " +
                    $"{shopLocalization.Get("embed/page").Format(currentPage + 1, maxPage + 1)}");
                if (product.Type == ImageType.Avatar)
                    embedBuilder.WithThumbnailUrl(product.URL);
                else
                    embedBuilder.WithImageUrl(product.URL);
                var embed = embedBuilder.Build();
                var components = new ComponentBuilder()
                    .WithSelectMenu("category", categoryOptions, shopLocalization.Get("select/category/name"), row: 0)
                    .WithSelectMenu("item", GetImageOptions(shop.Customization.GetProductsInPage(currentPage), context.Language),
                    shopLocalization.Get("select/item/name"), row: 1)
                    .WithButton("<", "<", ButtonStyle.Primary, disabled: !(currentPage > 0), row: 2)
                    .WithButton(shopLocalization.Get("btn/buy"), "buy", ButtonStyle.Success,
                    disabled: !shop.Customization.CanBuy(dbUser.Customization, dbUser.Wallet, currentPage, arrow), row: 2)
                    .WithButton(">", ">", ButtonStyle.Primary, disabled: !(currentPage < maxPage), row: 2)
                    .WithButton(shopLocalization.Get("btn/exit"), "exit", ButtonStyle.Danger, row: 3)
                    .WithButton(shopLocalization.Get("btn/clear"), "clear", ButtonStyle.Secondary, row: 3)
                    .WithButton(shopLocalization.Get("btn/confirm"), "confirm", ButtonStyle.Success, row: 3)
                    .Build();
                await context.Interaction.ModifyOriginalResponseAsync(msg => { msg.Embed = embed; msg.Components = components; });
                await context.LastInteraction.TryDeferAsync();
                var interaction = await context.WaitForButton();
                if (interaction is null)
                {
                    await context.CurrentMessage!.RemoveButtonsAsync();
                    return "exit";
                }
                switch (interaction.Data.CustomId)
                {
                    case "category":
                        return interaction.Data.Values.First();
                    case "item":
                        arrow = int.Parse(interaction.Data.Values.First());
                        break;
                    case "<":
                        arrow = 0;
                        currentPage--;
                        break;
                    case ">":
                        arrow = 0;
                        currentPage++;
                        break;
                    case "exit":
                        embed = EmbedFactory.CreateUserEmbed(context.User, dbUser)
                            .WithAuthor(shopLocalization.Get("embed/exit/author"))
                            .Build();
                        await context.Interaction.ModifyOriginalResponseAsync(msg => {
                            msg.Embed = embed;
                            msg.Components = new ComponentBuilder().Build();
                        });
                        return "exit";
                    case "clear":
                        shop.ShopBasket.Clear();
                        break;
                    case "confirm":
                        embed = EmbedFactory.CreateUserEmbed(context.User, dbUser)
                            .WithAuthor(shopLocalization.Get("embed/exit/author"))
                            .WithDescription($"{namesLocalization.Get("gold")}: {dbUser.Wallet.Gold} -> " +
                            $"{shop.ShopBasket.Wallet.Gold + dbUser.Wallet.Gold} {EmojiService.Get("gold")}\n" +
                            $"{namesLocalization.Get("dispoints")}: {dbUser.Wallet.Dispoints} -> " +
                            $"{shop.ShopBasket.Wallet.Dispoints + dbUser.Wallet.Dispoints} {EmojiService.Get("dispoints")}")
                            .AddField(shopLocalization.Get("embed/confirm/results"),
                            GetBasketProductsString(shop, context.Language, 20))
                            .Build();
                        await context.Interaction.ModifyOriginalResponseAsync(msg => {
                            msg.Embed = embed;
                            msg.Components = new ComponentBuilder().Build();
                        });
                        await shop.Confirm(dbUser);
                        return "exit";
                    case "buy":
                        shop.Customization.Buy(currentPage, arrow);
                        break;
                }
            }
        }

        private static List<SelectMenuOptionBuilder> GetItemOptions(IEnumerable<IItemProduct> products, Language lang)
        {
            var result = new List<SelectMenuOptionBuilder>();
            var row = 0;
            foreach(var product in products)
            {
                var item = product.GetItem();
                var description = item.Info.GetDescription(lang);
                if (description.Length > 70)
                {
                    description = $"{description[..70]}...";
                }
                result.Add(new(item.Info.GetName(lang),
                    row.ToString(),
                    description
                    ));
                row++;
            }
            return result;
        }

        private static List<SelectMenuOptionBuilder> GetImageOptions(IEnumerable<ICustomizationProduct> products, Language lang)
        {
            var result = new List<SelectMenuOptionBuilder>();
            var row = 0;
            foreach (var product in products)
            {
                var item = product.GetImage();
                result.Add(new(item.Info.GetName(lang),
                    row.ToString()
                    ));
                row++;
            }
            return result;
        }

        private static string GetPurchasingProductsString(IDatabaseUser dbUser, IShop shop, int page, int arrow, Language lang)
        {
            var result = new List<string>();
            var currentRow = 0;
            foreach(var product in shop.Purchasing.GetProductsInPage(page))
            {
                result.Add($"{(currentRow == arrow ? EmojiService.Get("choosePoint") : string.Empty)}" +
                    $"{product.GetItem().Info.GetName(lang)} x{product.Amount} = {product.Price} {EmojiService.Get(product.MoneyType.GetLowerName())}");
                currentRow++;
            }
            return string.Join("\n", result);
        }

        private static string GetSellingProductsString(IDatabaseUser dbUser, IShop shop, int page, int arrow, Language lang)
        {
            var result = new List<string>();
            var currentRow = 0;
            foreach (var product in shop.Selling.GetProductsInPage(page))
            {
                result.Add($"{(currentRow == arrow ? EmojiService.Get("choosePoint") : string.Empty)}" +
                    $"{product.GetItem().Info.GetName(lang)} {dbUser.Inventory.GetItemAmount(product.Id) + shop.ShopBasket.GetItemAmount(product)}" +
                    $"/{product.Amount} = {product.Price} {EmojiService.Get(product.MoneyType.GetLowerName())}");
                currentRow++;
            }
            return string.Join("\n", result);
        }
        private static string GetCustomizationProductsString(IShop shop, int page, int arrow, Language lang)
        {
            var result = new List<string>();
            var currentRow = 0;
            foreach (var product in shop.Customization.GetProductsInPage(page))
            {
                result.Add((currentRow == arrow ? EmojiService.Get("choosePoint") : string.Empty) +
                    $"{product.GetImage().Info.GetName(lang)} = {product.Price} {EmojiService.Get(product.MoneyType.GetLowerName())}");
                currentRow++;
            }
            return string.Join("\n", result);
        }
        private static string GetBasketProductsString(IShop shop, Language lang, int limit = 10)
        {
            var result = new List<string>();
            foreach (var (item, amount) in shop.ShopBasket.GetItems())
            {
                if (result.Count >= limit)
                    break;
                result.Add($"{amount.WithSign()} {item.Info.GetName(lang)}");
            }
            foreach (var image in shop.ShopBasket.GetCustomizations())
            {
                if (result.Count >= limit)
                    break;
                result.Add(image.Info.GetName(lang));
            }
            return result.Count > 0 ? string.Join("\n", result) : _localizationPart.Get(lang).Get("embed/basket/empty");
        }

        private static List<SelectMenuOptionBuilder> GetShopOptions(List<ObjectId> shopsId, Language lang)
        {
            var result = new List<SelectMenuOptionBuilder>();
            foreach (var id in shopsId)
            {
                var shop = ShopDatabase.GetReadonlyShop(id);
                var description = shop.Info.GetDescription(lang);
                if (description.Length > 70)
                    description = $"{description[..70]}...";
                result.Add(new(shop.Info.GetName(lang), id.ToString(), description));
            }
            return result;
        }
    }
}
