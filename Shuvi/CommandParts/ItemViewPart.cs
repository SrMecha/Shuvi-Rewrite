using Discord;
using MongoDB.Bson;
using MongoDB.Driver;
using Shuvi.Classes.Data.Pet;
using Shuvi.Classes.Data.User;
using Shuvi.Classes.Extensions;
using Shuvi.Classes.Factories.CustomEmbed;
using Shuvi.Classes.Types.Interaction;
using Shuvi.Classes.Types.Inventory;
using Shuvi.Classes.Types.Item;
using Shuvi.Enums.Item;
using Shuvi.Interfaces.User;
using Shuvi.Services.StaticServices.Database;
using Shuvi.Services.StaticServices.Localization;

namespace Shuvi.CommandParts
{
    public static class ItemViewPart
    {
        private static readonly LocalizationLanguagePart _localizationPart = LocalizationService.Get("itemViewPart");

        public static async Task Start(CustomInteractionContext context, IDatabaseUser dbUser, ObjectId itemId, bool canInteract)
        {
            switch (dbUser.Inventory.GetItem(itemId).Type)
            {
                case ItemType.Simple:
                    await ViewSimpleItem(context, dbUser, itemId, canInteract);
                    break;
                case ItemType.Weapon:
                    await ViewUserEquipmentItem(context, dbUser, itemId, canInteract);
                    break;
                case ItemType.Helmet:
                    await ViewUserEquipmentItem(context, dbUser, itemId, canInteract);
                    break;
                case ItemType.Armor:
                    await ViewUserEquipmentItem(context, dbUser, itemId, canInteract);
                    break;
                case ItemType.Leggings:
                    await ViewUserEquipmentItem(context, dbUser, itemId, canInteract);
                    break;
                case ItemType.Boots:
                    await ViewUserEquipmentItem(context, dbUser, itemId, canInteract);
                    break;
                case ItemType.Potion:
                    await ViewPotionItem(context, dbUser, itemId, canInteract);
                    break;
                case ItemType.Chest:
                    await ViewChestItem(context, dbUser, itemId, canInteract);
                    break;
                case ItemType.Amulet:
                    await ViewPetEquipmentItem(context, dbUser, itemId, canInteract);
                    break;
                case ItemType.Recipe:
                    await ViewRecipeItem(context, dbUser, itemId, canInteract);
                    break;
                default:
                    await ViewSimpleItem(context, dbUser, itemId, canInteract);
                    break;
            };
        }
        private static async Task ViewSimpleItem(CustomInteractionContext context, IDatabaseUser dbUser, ObjectId itemId, bool canInteract)
        {
            var viewLocalization = _localizationPart.Get(context.Language);
            var namesLocalization = LocalizationService.Get("names").Get(context.Language);
            var item = dbUser.Inventory.GetItem<SimpleItem>(itemId);
            while (context.LastInteraction is not null)
            {
                var embed = EmbedFactory.CreateUserEmbed(dbUser)
                    .WithAuthor(viewLocalization.Get("Embed/View/Author"))
                    .WithDescription($"{viewLocalization.Get("Embed/View/Name").Format(item.Info.GetName(context.Language))}\n" +
                    $"{viewLocalization.Get("Embed/View/Type").Format(namesLocalization.Get($"ItemType/{item.Type.GetName()}"))}\n" +
                    $"{viewLocalization.Get("Embed/View/Rank").Format(item.Rank.GetName())}\n" +
                    $"{viewLocalization.Get("Embed/View/Max").Format(item.Max.IsInfinity() ?
                    viewLocalization.Get("Embed/View/Infinity") : item.Max)}\n" +
                    $"{viewLocalization.Get("Embed/View/Have").Format(dbUser.Inventory.GetItemAmount(item.Id))}\n\n" +
                    $"{viewLocalization.Get("Embed/View/Description")}\n{item.Info.GetDescription(context.Language)}\n" +
                    $"{(item.CanTrade ? viewLocalization.Get("Embed/View/CanTrade") : viewLocalization.Get("Embed/View/CanNotTrade"))}\n" +
                    $"{(item.CanLoose ? viewLocalization.Get("Embed/View/CanLoose") : viewLocalization.Get("Embed/View/CanNotLoose"))}")
                    .WithFooter($"ID: {itemId}")
                    .Build();
                var components = new ComponentBuilder()
                    .WithButton(viewLocalization.Get("Btn/Exit"), "exit", ButtonStyle.Danger)
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
        private static async Task ViewUserEquipmentItem(CustomInteractionContext context, IDatabaseUser dbUser, ObjectId itemId, bool canInteract)
        {
            var viewLocalization = _localizationPart.Get(context.Language);
            var namesLocalization = LocalizationService.Get("names").Get(context.Language);
            var item = dbUser.Inventory.GetItem<EquipmentItem>(itemId);
            var requirements = item.Requirements.GetRequirementsInfo(context.Language, dbUser);
            while (context.LastInteraction is not null)
            {
                var isEquiped = item.Id == dbUser.Equipment.GetEquipmentId(item.Type);
                var embed = EmbedFactory.CreateUserEmbed(dbUser)
                    .WithAuthor(viewLocalization.Get("Embed/View/Author"))
                    .WithDescription($"{viewLocalization.Get("Embed/View/Name").Format(item.Info.GetName(context.Language))} " +
                    $"{(isEquiped ? $"({viewLocalization.Get("Embed/View/Equipped")})" : string.Empty)}\n" +
                    $"{viewLocalization.Get("Embed/View/Type").Format(namesLocalization.Get($"ItemType/{item.Type.GetName()}"))}\n" +
                    $"{viewLocalization.Get("Embed/View/Rank").Format(item.Rank.GetName())}\n" +
                    $"{viewLocalization.Get("Embed/View/Max").Format(item.Max.IsInfinity() ?
                    viewLocalization.Get("Embed/View/Infinity") : item.Max)}\n" +
                    $"{viewLocalization.Get("Embed/View/Have").Format(dbUser.Inventory.GetItemAmount(item.Id))}\n\n" +
                    $"{viewLocalization.Get("Embed/View/Description")}\n{item.Info.GetDescription(context.Language)}\n" +
                    $"{(item.CanTrade ? viewLocalization.Get("Embed/View/CanTrade") : viewLocalization.Get("Embed/View/CanNotTrade"))}\n" +
                    $"{(item.CanLoose ? viewLocalization.Get("Embed/View/CanLoose") : viewLocalization.Get("Embed/View/CanNotLoose"))}\n\n" +
                    $"{viewLocalization.Get("Embed/View/Bonuses")}\n{item.Bonuses.GetBonusesInfo(context.Language)}\n\n" +
                    $"{viewLocalization.Get("Embed/View/Requirements")}\n{requirements.Description}")
                    .WithFooter($"ID: {itemId}")
                    .Build();
                MessageComponent components;
                if (canInteract)
                    components = new ComponentBuilder()
                        .WithButton(viewLocalization.Get("Btn/Exit"), "exit", ButtonStyle.Danger)
                        .WithButton(viewLocalization.Get("Btn/Equip"), "equip", ButtonStyle.Success,
                        disabled: !requirements.IsMeetRequirements || !dbUser.Inventory.HaveItem(item.Id) || isEquiped)
                        .Build();
                else
                    components = new ComponentBuilder()
                        .WithButton(viewLocalization.Get("Btn/exit"), "exit", ButtonStyle.Danger)
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
                    case "equip":
                        if (!dbUser.Inventory.HaveItem(item.Id))
                        {
                            await interaction.SendError(viewLocalization.Get("Error/NotHaveItem"), context.Language);
                            continue;
                        }
                        dbUser.Equipment.SetEquipment(item.Type, item.Id);
                        await UserDatabase.UpdateUser(dbUser.Id,
                            new UpdateDefinitionBuilder<UserData>().Set(item.Type.GetNameOf(), item.Id));
                        await interaction.SendInfo(viewLocalization.Get("Embed/View/ItemEquipped"));
                        break;
                    default:
                        return;
                }
            }
        }
        private static async Task ViewPetEquipmentItem(CustomInteractionContext context, IDatabaseUser dbUser, ObjectId itemId, bool canInteract)
        {
            var viewLocalization = _localizationPart.Get(context.Language);
            var namesLocalization = LocalizationService.Get("names").Get(context.Language);
            var item = dbUser.Inventory.GetItem<EquipmentItem>(itemId);
            var pet = await dbUser.Pet.GetPet();
            var havePet = pet is not null;
            var requirements = havePet ? item.Requirements.GetRequirementsInfo(context.Language, pet!) : item.Requirements.GetRequirementsInfo(context.Language);
            while (context.LastInteraction is not null)
            {
                var isEquiped = havePet ? item.Id == pet!.Equipment.GetEquipmentId(item.Type) : false;
                var embed = EmbedFactory.CreateUserEmbed(dbUser)
                    .WithAuthor(viewLocalization.Get("Embed/View/Author"))
                    .WithDescription($"{viewLocalization.Get("Embed/View/Name").Format(item.Info.GetName(context.Language))} " +
                    $"{(isEquiped ? $"({viewLocalization.Get("Embed/View/Equipped")})" : string.Empty)}\n" +
                    $"{viewLocalization.Get("Embed/View/Type").Format(namesLocalization.Get($"ItemType/{item.Type.GetName()}"))}\n" +
                    $"{viewLocalization.Get("Embed/View/Rank").Format(item.Rank.GetName())}\n" +
                    $"{viewLocalization.Get("Embed/View/Max").Format(item.Max.IsInfinity() ?
                    viewLocalization.Get("Embed/View/Infinity") : item.Max)}\n" +
                    $"{viewLocalization.Get("Embed/View/Have").Format(dbUser.Inventory.GetItemAmount(item.Id))}\n\n" +
                    $"{viewLocalization.Get("Embed/View/Description")}\n{item.Info.GetDescription(context.Language)}\n" +
                    $"{(item.CanTrade ? viewLocalization.Get("Embed/View/CanTrade") : viewLocalization.Get("Embed/View/CanNotTrade"))}\n" +
                    $"{(item.CanLoose ? viewLocalization.Get("Embed/View/CanLoose") : viewLocalization.Get("Embed/View/CanNotLoose"))}\n\n" +
                    $"{viewLocalization.Get("Embed/View/Bonuses")}\n{item.Bonuses.GetBonusesInfo(context.Language)}\n\n" +
                    $"{viewLocalization.Get("Embed/View/Requirements")}\n{requirements.Description}")
                    .WithFooter($"ID: {itemId}")
                    .Build();
                MessageComponent components;
                if (canInteract)
                    components = new ComponentBuilder()
                        .WithButton(viewLocalization.Get("Btn/Exit"), "exit", ButtonStyle.Danger)
                        .WithButton(viewLocalization.Get("Btn/Equip"), "equip", ButtonStyle.Success,
                        disabled: !requirements.IsMeetRequirements || !dbUser.Inventory.HaveItem(item.Id) || isEquiped)
                        .Build();
                else
                    components = new ComponentBuilder()
                        .WithButton(viewLocalization.Get("Btn/Exit"), "exit", ButtonStyle.Danger)
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
                    case "equip":
                        if (!dbUser.Inventory.HaveItem(item.Id))
                        {
                            await interaction.SendError(viewLocalization.Get("Error/NotHaveItem"), context.Language);
                            continue;
                        }
                        pet!.Equipment.SetEquipment(item.Type, item.Id);
                        await PetDatabase.UpdatePet(pet!.Id,
                            new UpdateDefinitionBuilder<PetData>().Set(item.Type.GetNameOf(), item.Id));
                        await interaction.SendInfo(viewLocalization.Get("Embed/View/ItemEquipped"));
                        break;
                    default:
                        return;
                }
            }
        }
        private static async Task ViewPotionItem(CustomInteractionContext context, IDatabaseUser dbUser, ObjectId itemId, bool canInteract)
        {
            var viewLocalization = _localizationPart.Get(context.Language);
            var namesLocalization = LocalizationService.Get("names").Get(context.Language);
            var item = dbUser.Inventory.GetItem<PotionItem>(itemId);
            while (context.LastInteraction is not null)
            {
                var embed = EmbedFactory.CreateUserEmbed(dbUser)
                    .WithAuthor(viewLocalization.Get("Embed/View/Author"))
                    .WithDescription($"{viewLocalization.Get("Embed/View/Name").Format(item.Info.GetName(context.Language))}\n" +
                    $"{viewLocalization.Get("Embed/View/Type").Format(namesLocalization.Get($"ItemType/{item.Type.GetName()}"))}\n" +
                    $"{viewLocalization.Get("Embed/View/Rank").Format(item.Rank.GetName())}\n" +
                    $"{viewLocalization.Get("Embed/View/Max").Format(item.Max.IsInfinity() ?
                    viewLocalization.Get("Embed/View/Infinity") : item.Max)}\n" +
                    $"{viewLocalization.Get("Embed/View/Have").Format(dbUser.Inventory.GetItemAmount(item.Id))}\n\n" +
                    $"{viewLocalization.Get("Embed/View/Description")}\n{item.Info.GetDescription(context.Language)}\n" +
                    $"{(item.CanTrade ? viewLocalization.Get("Embed/View/CanTrade") : viewLocalization.Get("Embed/View/CanNotTrade"))}\n" +
                    $"{(item.CanLoose ? viewLocalization.Get("Embed/View/CanLoose") : viewLocalization.Get("Embed/View/CanNotLoose"))}\n\n" +
                    $"{viewLocalization.Get("Embed/View/Bonuses")}\n{item.GetRecoverInfo(context.Language)}")
                    .WithFooter($"ID: {itemId}")
                    .Build();
                MessageComponent components;
                if (canInteract)
                    components = new ComponentBuilder()
                        .WithButton(viewLocalization.Get("Btn/Exit"), "exit", ButtonStyle.Danger)
                        .WithButton(viewLocalization.Get("Btn/Use"), "use", ButtonStyle.Success, disabled: !dbUser.Inventory.HaveItem(item.Id, 1))
                        .Build();

                else
                    components = new ComponentBuilder()
                        .WithButton(viewLocalization.Get("Btn/Exit"), "exit", ButtonStyle.Danger)
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
                    case "use":
                        if (!dbUser.Inventory.HaveItem(item.Id))
                        {
                            await interaction.SendError(viewLocalization.Get("Error/NotHaveItem"), context.Language);
                            continue;
                        }
                        await item.Use(dbUser);
                        await interaction.SendInfo(viewLocalization.Get("Embed/View/PotionUsed"));
                        break;
                    default:
                        return;
                }
            }
        }
        private static async Task ViewChestItem(CustomInteractionContext context, IDatabaseUser dbUser, ObjectId itemId, bool canInteract)
        {
            var viewLocalization = _localizationPart.Get(context.Language);
            var namesLocalization = LocalizationService.Get("names").Get(context.Language);
            var item = dbUser.Inventory.GetItem<ChestItem>(itemId);
            while (context.LastInteraction is not null)
            {
                var embed = EmbedFactory.CreateUserEmbed(dbUser)
                    .WithAuthor(viewLocalization.Get("Embed/View/Author"))
                    .WithDescription($"{viewLocalization.Get("Embed/View/Name").Format(item.Info.GetName(context.Language))}\n" +
                    $"{viewLocalization.Get("Embed/View/Type").Format(namesLocalization.Get($"ItemType/{item.Type.GetName()}"))}\n" +
                    $"{viewLocalization.Get("Embed/View/Rank").Format(item.Rank.GetName())}\n" +
                    $"{viewLocalization.Get("Embed/View/Max").Format(item.Max.IsInfinity() ?
                    viewLocalization.Get("Embed/View/Infinity") : item.Max)}\n" +
                    $"{viewLocalization.Get("Embed/View/Have").Format(dbUser.Inventory.GetItemAmount(item.Id))}\n\n" +
                    $"{viewLocalization.Get("Embed/View/Description")}\n{item.Info.GetDescription(context.Language)}\n" +
                    $"{(item.CanTrade ? viewLocalization.Get("Embed/View/CanTrade") : viewLocalization.Get("Embed/View/CanNotTrade"))}\n" +
                    $"{(item.CanLoose ? viewLocalization.Get("Embed/View/CanLoose") : viewLocalization.Get("Embed/View/CanNotLoose"))}\n\n" +
                    $"{viewLocalization.Get("Embed/View/Chances")}\n{item.Drop.GetChancesInfo(dbUser.Characteristics.Luck, context.Language)}")
                    .WithFooter($"ID: {itemId}")
                    .Build();
                MessageComponent components;
                if (canInteract)
                    components = new ComponentBuilder()
                        .WithButton("x1", "1", ButtonStyle.Success, disabled: !dbUser.Inventory.HaveItem(item.Id, 1), row: 0)
                        .WithButton("x2", "2", ButtonStyle.Success, disabled: !dbUser.Inventory.HaveItem(item.Id, 2), row: 0)
                        .WithButton("x5", "5", ButtonStyle.Success, disabled: !dbUser.Inventory.HaveItem(item.Id, 5), row: 0)
                        .WithButton(viewLocalization.Get("Btn/Exit"), "exit", ButtonStyle.Danger, row: 1)
                        .Build();
                else
                    components = new ComponentBuilder()
                        .WithButton(viewLocalization.Get("Btn/Exit"), "exit", ButtonStyle.Danger)
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
                    case "1" or "2" or "5":
                        var drop = new DropInventory();
                        var amount = int.Parse(interaction.Data.CustomId);
                        for (int i = 0; i < amount; i++)
                        {
                            drop.AddInventory(item.Drop.GetDrop(dbUser.Characteristics.Luck));
                        }
                        dbUser.AddDrop(drop);
                        dbUser.Inventory.RemoveItem(itemId, amount);
                        await UserDatabase.UpdateUser(dbUser.Id, new UpdateDefinitionBuilder<UserData>()
                            .Set(x => x.Gold, dbUser.Wallet.Gold)
                            .Set(x => x.Dispoints, dbUser.Wallet.Dispoints)
                            .Set(x => x.Inventory, dbUser.Inventory.GetItemsDictionary()));
                        await interaction.SendInfo(drop.GetDropInfo(context.Language), viewLocalization.Get("Embed/View/Received"));
                        break;
                    default:
                        return;
                }
            }
        }
        private static async Task ViewRecipeItem(CustomInteractionContext context, IDatabaseUser dbUser, ObjectId itemId, bool canInteract)
        {
            var viewLocalization = _localizationPart.Get(context.Language);
            var namesLocalization = LocalizationService.Get("names").Get(context.Language);
            var item = dbUser.Inventory.GetItem<RecipeItem>(itemId);
            var requirements = item.Craft.Requirements.GetRequirementsInfo(context.Language, dbUser);
            while (context.LastInteraction is not null)
            {
                var maxCanCraft = item.Craft.GetMaxCraft(dbUser.Inventory);
                var embed = EmbedFactory.CreateUserEmbed(dbUser)
                    .WithAuthor(viewLocalization.Get("Embed/View/Author"))
                    .WithDescription($"{viewLocalization.Get("Embed/View/Name").Format(item.Info.GetName(context.Language))}\n" +
                    $"{viewLocalization.Get("Embed/View/Type").Format(namesLocalization.Get($"ItemType/{item.Type.GetName()}"))}\n" +
                    $"{viewLocalization.Get("Embed/View/Rank").Format(item.Rank.GetName())}\n" +
                    $"{viewLocalization.Get("Embed/View/Max").Format(item.Max.IsInfinity() ?
                    viewLocalization.Get("Embed/View/Infinity") : item.Max)}\n" +
                    $"{viewLocalization.Get("Embed/View/Have").Format(dbUser.Inventory.GetItemAmount(item.Id))}\n\n" +
                    $"{viewLocalization.Get("Embed/View/Description")}\n{item.Info.GetDescription(context.Language)}\n" +
                    $"{(item.CanTrade ? viewLocalization.Get("Embed/View/CanTrade") : viewLocalization.Get("Embed/View/CanNotTrade"))}\n" +
                    $"{(item.CanLoose ? viewLocalization.Get("Embed/View/CanLoose") : viewLocalization.Get("Embed/View/CanNotLoose"))}\n\n" +
                    $"{viewLocalization.Get("Embed/View/Requirements")}\n{requirements.Description}\n\n" +
                    $"{viewLocalization.Get("Embed/View/NeededItems")}\n" +
                    $"{item.Craft.Items.GetItemsDictionary().GetItemsNeededInfo(dbUser.Inventory.GetItemsDictionary(), context.Language)}\n" +
                    $"{viewLocalization.Get("Embed/View/CraftedItem").Format(item.Craft.GetCraftedItem().Info.GetName(context.Language))}")
                    .WithFooter($"ID: {itemId}")
                    .Build();
                MessageComponent components;
                if (canInteract)
                    components = new ComponentBuilder()
                        .WithButton("x1", "1", ButtonStyle.Success, disabled: !(maxCanCraft >= 1 && requirements.IsMeetRequirements), row: 0)
                        .WithButton("x2", "2", ButtonStyle.Success, disabled: !(maxCanCraft >= 2 && requirements.IsMeetRequirements), row: 0)
                        .WithButton("x5", "5", ButtonStyle.Success, disabled: !(maxCanCraft >= 5 && requirements.IsMeetRequirements), row: 0)
                        .WithButton($"MAX x{maxCanCraft}", $"+{maxCanCraft}", ButtonStyle.Success, 
                        disabled: !(maxCanCraft > 0 && requirements.IsMeetRequirements), row: 0)
                        .WithButton(viewLocalization.Get("Btn/Exit"), "exit", ButtonStyle.Danger, row: 1)
                        .WithButton(viewLocalization.Get("Btn/ViewItem"), "viewItem", ButtonStyle.Primary, row: 1)
                        .Build();
                else
                    components = new ComponentBuilder()
                        .WithButton(viewLocalization.Get("Btn/Exit"), "exit", ButtonStyle.Danger)
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
                    case "viewItem":
                        await Start(context, dbUser, item.Craft.GetCraftedItem().Id, false);
                        break;
                    default:
                        var craftAmount = int.Parse(interaction.Data.CustomId);
                        foreach (var (craftItemId, amount) in item.Craft.Items.GetItemsDictionary())
                            dbUser.Inventory.RemoveItem(craftItemId, craftAmount * amount);
                        dbUser.Inventory.AddItem(item.Craft.CraftedItemId, craftAmount);
                        break;
                }
            }
        }
    }
}
