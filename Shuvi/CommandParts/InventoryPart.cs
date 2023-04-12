using Discord;
using MongoDB.Bson;
using Shuvi.Classes.Extensions;
using Shuvi.Classes.Factories.CustomEmbed;
using Shuvi.Classes.Types.Interaction;
using Shuvi.Enums.Localization;
using Shuvi.Interfaces.Inventory;
using Shuvi.Interfaces.User;
using Shuvi.Services.StaticServices.Localization;

namespace Shuvi.CommandParts
{
    public static class InventoryPart
    {
        private static readonly LocalizationLanguagePart _localizationPart = LocalizationService.Get("inventoryPart");

        public static async Task Start(CustomInteractionContext context, IDatabaseUser dbUser, IUser user, bool canInteract = true)
        {
            var haveItems = true;
            var maxPage = (dbUser.Inventory.Count + 9) / 10;
            if (maxPage < 1)
            {
                haveItems = false;
                maxPage = 1;
            }
            var pageNow = 0;
            var inventoryLocalization = _localizationPart.Get(context.Language);

            while (context.LastInteraction is not null)
            {
                var embed = EmbedFactory.CreateUserEmbed(context.User, dbUser)
                    .WithAuthor(inventoryLocalization.Get("embed/view/author").Format(user.Username))
                    .WithDescription(GetItemsInfo(dbUser.Inventory, pageNow, context.Language))
                    .WithFooter(inventoryLocalization.Get("embed/view/page").Format(pageNow + 1, maxPage))
                    .Build();
                var components = new ComponentBuilder()
                    .WithButton("<", "<", ButtonStyle.Primary, disabled: pageNow <= 0, row: 0)
                    .WithButton(inventoryLocalization.Get("btn/exit"), "exit", ButtonStyle.Danger, row: 0)
                    .WithButton(">", ">", ButtonStyle.Primary, disabled: pageNow >= maxPage - 1, row: 0)
                    .WithSelectMenu("choose", GetItemsSelectMenu(dbUser.Inventory, pageNow, context.Language), 
                    inventoryLocalization.Get("select/view/name"),disabled: !haveItems)
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
                    case "<":
                        pageNow--;
                        break;
                    case "exit":
                        return;
                    case ">":
                        pageNow++;
                        break;
                    case "choose":
                        await ItemViewPart.Start(context, dbUser, new ObjectId(interaction.Data.Values.First()), canInteract);
                        break;
                    default:
                        return;
                }
            }
        }
        private static string GetItemsInfo(IUserInventory inventory, int page, Language lang)
        {
            var result = new List<string>();
            for (int i = page * 10; i < inventory.Count && i < page * 10 + 10; i++)
                result.Add($"**#{i + 1}** {inventory.GetItemAt(i).Info.GetName(lang)} x{inventory.GetItemAmountAt(i)}");
            if (result.Count < 1)
                return _localizationPart.Get(lang).Get("embed/view/void");
            return string.Join("\n", result);
        }
        public static List<SelectMenuOptionBuilder> GetItemsSelectMenu(IUserInventory inventory, int page, Language lang)
        {
            var result = new List<SelectMenuOptionBuilder>();
            for (int i = page * 10; i < inventory.Count && i < page * 10 + 10; i++)
            {
                var item = inventory.GetItemAt(i);
                var itemDescription = item.Info.GetDescription(lang);
                if (itemDescription.Length > 70)
                {
                    itemDescription = $"{itemDescription[..70]}...";
                }
                result.Add(new SelectMenuOptionBuilder(
                    item.Info.GetName(lang),
                    item.Id.ToString(),
                    itemDescription
                    ));
            }
            if (result.Count < 1)
            {
                result.Add(new SelectMenuOptionBuilder("None", "None", "There is no items"));
            }
            return result;
        }
    }
}
