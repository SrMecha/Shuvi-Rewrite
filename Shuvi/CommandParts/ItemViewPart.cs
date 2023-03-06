using Discord;
using MongoDB.Bson;
using Shuvi.Classes.Extensions;
using Shuvi.Classes.Factories.CustomEmbed;
using Shuvi.Classes.Types.Interaction;
using Shuvi.Classes.Types.Item;
using Shuvi.Enums.Item;
using Shuvi.Interfaces.User;
using Shuvi.Services.StaticServices.Localization;

namespace Shuvi.CommandParts
{
    public static class ItemViewPart
    {
        private static readonly LocalizationLanguagePart _localizationPart = LocalizationService.Get("itemViewPart");

        public static async Task Start(CustomInteractionContext context, IDatabaseUser dbUser, ObjectId itemId)
        {
            switch (dbUser.Inventory.GetItem(itemId).Type)
            {
                case ItemType.Simple:
                    await ViewSimpleItem(context, dbUser, itemId);
                    break;
                case ItemType.Weapon:
                    break;
                case ItemType.Helmet:
                    break;
                case ItemType.Armor:
                    break;
                case ItemType.Leggings:
                    break;
                case ItemType.Boots:
                    break;
                case ItemType.Potion:
                    break;
                case ItemType.Chest:
                    break;
                case ItemType.Amulet:
                    break;
                case ItemType.Recipe:
                    break;
                default:
                    await ViewSimpleItem(context, dbUser, itemId);
                    break;
            };
        }
        private static async Task ViewSimpleItem(CustomInteractionContext context, IDatabaseUser dbUser, ObjectId itemId)
        {
            var viewLocalization = _localizationPart.Get(context.Language);
            var namesLocalization = LocalizationService.Get("names").Get(context.Language);
            while (context.LastInteraction is not null)
            {
                var item = dbUser.Inventory.GetItem<SimpleItem>(itemId);
                var embed = EmbedFactory.CreateUserEmbed(context.User, dbUser)
                    .WithAuthor(viewLocalization.Get("embed/view/author"))
                    .WithDescription($"{viewLocalization.Get("embed/view/name").Format(item.Info.GetName(context.Language))}" +
                    $"{viewLocalization.Get("embed/view/type").Format(namesLocalization.Get($"itemType/{item.Type.GetLowerName()}"))}" +
                    $"{viewLocalization.Get("embed/view/rank").Format(item.Rank.GetName())}" +
                    $"{viewLocalization.Get("embed/view/max").Format(item.Max.IsInfinity() ? 
                    viewLocalization.Get("embed/view/infinity") : item.Max)}" +
                    $"{viewLocalization.Get("embed/view/have").Format(dbUser.Inventory.GetItemAmount(item.Id))}\n\n" +
                    $"{viewLocalization.Get("embed/view/description")}\n{item.Info.GetDescription(context.Language)}\n" +
                    $"{(item.CanTrade ? viewLocalization.Get("embed/view/canTrade") : viewLocalization.Get("embed/view/canNotTrade"))}\n" +
                    $"{(item.CanLoose ? viewLocalization.Get("embed/view/canLoose") : viewLocalization.Get("embed/view/canNotLoose"))}")
                    .WithFooter($"ID: {itemId}")
                    .Build();
                var components = new ComponentBuilder()
                    .WithButton(viewLocalization.Get("btn/exit"), "exit", ButtonStyle.Danger)
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
                        return;
                    default:
                        return;
                }
            }
        }
    }
}
