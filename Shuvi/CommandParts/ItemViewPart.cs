using Discord;
using MongoDB.Bson;
using MongoDB.Driver;
using Shuvi.Classes.Data.Pet;
using Shuvi.Classes.Data.User;
using Shuvi.Classes.Extensions;
using Shuvi.Classes.Factories.CustomEmbed;
using Shuvi.Classes.Types.Interaction;
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

        public static async Task Start(CustomInteractionContext context, IDatabaseUser dbUser, ObjectId itemId)
        {
            switch (dbUser.Inventory.GetItem(itemId).Type)
            {
                case ItemType.Simple:
                    await ViewSimpleItem(context, dbUser, itemId);
                    break;
                case ItemType.Weapon:
                    await ViewUserEquipmentItem(context, dbUser, itemId);
                    break;
                case ItemType.Helmet:
                    await ViewUserEquipmentItem(context, dbUser, itemId);
                    break;
                case ItemType.Armor:
                    await ViewUserEquipmentItem(context, dbUser, itemId);
                    break;
                case ItemType.Leggings:
                    await ViewUserEquipmentItem(context, dbUser, itemId);
                    break;
                case ItemType.Boots:
                    await ViewUserEquipmentItem(context, dbUser, itemId);
                    break;
                case ItemType.Potion:
                    await ViewPotionItem(context, dbUser, itemId);
                    break;
                case ItemType.Chest:
                    break;
                case ItemType.Amulet:
                    await ViewPetEquipmentItem(context, dbUser, itemId);
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
            var item = dbUser.Inventory.GetItem<SimpleItem>(itemId);
            while (context.LastInteraction is not null)
            {
                var embed = EmbedFactory.CreateUserEmbed(context.User, dbUser)
                    .WithAuthor(viewLocalization.Get("embed/view/author"))
                    .WithDescription($"{viewLocalization.Get("embed/view/name").Format(item.Info.GetName(context.Language))}\n" +
                    $"{viewLocalization.Get("embed/view/type").Format(namesLocalization.Get($"itemType/{item.Type.GetLowerName()}"))}\n" +
                    $"{viewLocalization.Get("embed/view/rank").Format(item.Rank.GetName())}\n" +
                    $"{viewLocalization.Get("embed/view/max").Format(item.Max.IsInfinity() ?
                    viewLocalization.Get("embed/view/infinity") : item.Max)}\n" +
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
        private static async Task ViewUserEquipmentItem(CustomInteractionContext context, IDatabaseUser dbUser, ObjectId itemId)
        {
            var viewLocalization = _localizationPart.Get(context.Language);
            var namesLocalization = LocalizationService.Get("names").Get(context.Language);
            var item = dbUser.Inventory.GetItem<EquipmentItem>(itemId);
            while (context.LastInteraction is not null)
            {
                var isEquiped = item.Id == dbUser.Equipment.GetEquipmentId(item.Type);
                var embed = EmbedFactory.CreateUserEmbed(context.User, dbUser)
                    .WithAuthor(viewLocalization.Get("embed/view/author"))
                    .WithDescription($"{viewLocalization.Get("embed/view/name").Format(item.Info.GetName(context.Language))} " +
                    $"{(isEquiped ? $"({viewLocalization.Get("embed/view/equipped")})" : string.Empty)}\n" +
                    $"{viewLocalization.Get("embed/view/type").Format(namesLocalization.Get($"itemType/{item.Type.GetLowerName()}"))}\n" +
                    $"{viewLocalization.Get("embed/view/rank").Format(item.Rank.GetName())}\n" +
                    $"{viewLocalization.Get("embed/view/max").Format(item.Max.IsInfinity() ?
                    viewLocalization.Get("embed/view/infinity") : item.Max)}\n" +
                    $"{viewLocalization.Get("embed/view/have").Format(dbUser.Inventory.GetItemAmount(item.Id))}\n\n" +
                    $"{viewLocalization.Get("embed/view/description")}\n{item.Info.GetDescription(context.Language)}\n" +
                    $"{(item.CanTrade ? viewLocalization.Get("embed/view/canTrade") : viewLocalization.Get("embed/view/canNotTrade"))}\n" +
                    $"{(item.CanLoose ? viewLocalization.Get("embed/view/canLoose") : viewLocalization.Get("embed/view/canNotLoose"))}\n\n" +
                    $"{viewLocalization.Get("embed/view/bonuses")}\n{item.Bonuses.GetBonusesInfo(context.Language)}\n\n" +
                    $"{viewLocalization.Get("embed/view/requirements")}\n{item.Requirements.GetRequirementsInfo(context.Language, dbUser)}")
                    .WithFooter($"ID: {itemId}")
                    .Build();
                var components = new ComponentBuilder()
                    .WithButton(viewLocalization.Get("btn/exit"), "exit", ButtonStyle.Danger)
                    .WithButton(viewLocalization.Get("btn/equip"), "equip", ButtonStyle.Success, 
                    disabled: !item.Requirements.IsMeetRequirements(dbUser) || !dbUser.Inventory.HaveItem(item.Id) || isEquiped)
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
                            await interaction.SendError(viewLocalization.Get("error/notHaveItem"), context.Language);
                            continue;
                        }
                        dbUser.Equipment.SetEquipment(item.Type, item.Id);
                        await UserDatabase.UpdateUser(dbUser.Id, 
                            new UpdateDefinitionBuilder<UserData>().Set(nameof(item.Type), item.Id));
                        await interaction.SendInfo(viewLocalization.Get("embed/view/itemEquipped"));
                        break;
                    default:
                        return;
                }
            }
        }
        private static async Task ViewPetEquipmentItem(CustomInteractionContext context, IDatabaseUser dbUser, ObjectId itemId)
        {
            var viewLocalization = _localizationPart.Get(context.Language);
            var namesLocalization = LocalizationService.Get("names").Get(context.Language);
            var item = dbUser.Inventory.GetItem<EquipmentItem>(itemId);
            var pet = await dbUser.Pet.GetPet();
            var havePet = pet is not null;
            var requirementsInfo = havePet ? item.Requirements.GetRequirementsInfo(context.Language, pet!)
                : item.Requirements.GetRequirementsInfo(context.Language);
            while (context.LastInteraction is not null)
            {
                var isEquiped = havePet ? item.Id == pet!.Equipment.GetEquipmentId(item.Type) : false;
                var embed = EmbedFactory.CreateUserEmbed(context.User, dbUser)
                    .WithAuthor(viewLocalization.Get("embed/view/author"))
                    .WithDescription($"{viewLocalization.Get("embed/view/name").Format(item.Info.GetName(context.Language))} " +
                    $"{(isEquiped ? $"({viewLocalization.Get("embed/view/equipped")})" : string.Empty)}\n" +
                    $"{viewLocalization.Get("embed/view/type").Format(namesLocalization.Get($"itemType/{item.Type.GetLowerName()}"))}\n" +
                    $"{viewLocalization.Get("embed/view/rank").Format(item.Rank.GetName())}\n" +
                    $"{viewLocalization.Get("embed/view/max").Format(item.Max.IsInfinity() ?
                    viewLocalization.Get("embed/view/infinity") : item.Max)}\n" +
                    $"{viewLocalization.Get("embed/view/have").Format(dbUser.Inventory.GetItemAmount(item.Id))}\n\n" +
                    $"{viewLocalization.Get("embed/view/description")}\n{item.Info.GetDescription(context.Language)}\n" +
                    $"{(item.CanTrade ? viewLocalization.Get("embed/view/canTrade") : viewLocalization.Get("embed/view/canNotTrade"))}\n" +
                    $"{(item.CanLoose ? viewLocalization.Get("embed/view/canLoose") : viewLocalization.Get("embed/view/canNotLoose"))}\n\n" +
                    $"{viewLocalization.Get("embed/view/bonuses")}\n{item.Bonuses.GetBonusesInfo(context.Language)}\n\n" +
                    $"{viewLocalization.Get("embed/view/requirements")}\n{requirementsInfo}")
                    .WithFooter($"ID: {itemId}")
                    .Build();
                var components = new ComponentBuilder()
                    .WithButton(viewLocalization.Get("btn/exit"), "exit", ButtonStyle.Danger)
                    .WithButton(viewLocalization.Get("btn/equip"), "equip", ButtonStyle.Success,
                    disabled: !item.Requirements.IsMeetRequirements(dbUser) || !dbUser.Inventory.HaveItem(item.Id) || isEquiped)
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
                            await interaction.SendError(viewLocalization.Get("error/notHaveItem"), context.Language);
                            continue;
                        }
                        pet!.Equipment.SetEquipment(item.Type, item.Id);
                        await PetDatabase.UpdatePet(pet!.Id,
                            new UpdateDefinitionBuilder<PetData>().Set(nameof(item.Type), item.Id));
                        await interaction.SendInfo(viewLocalization.Get("embed/view/itemEquipped"));
                        break;
                    default:
                        return;
                }
            }
        }
        private static async Task ViewPotionItem(CustomInteractionContext context, IDatabaseUser dbUser, ObjectId itemId)
        {
            var viewLocalization = _localizationPart.Get(context.Language);
            var namesLocalization = LocalizationService.Get("names").Get(context.Language);
            var item = dbUser.Inventory.GetItem<PotionItem>(itemId);
            while (context.LastInteraction is not null)
            {
                var embed = EmbedFactory.CreateUserEmbed(context.User, dbUser)
                    .WithAuthor(viewLocalization.Get("embed/view/author"))
                    .WithDescription($"{viewLocalization.Get("embed/view/name").Format(item.Info.GetName(context.Language))}\n" +
                    $"{viewLocalization.Get("embed/view/type").Format(namesLocalization.Get($"itemType/{item.Type.GetLowerName()}"))}\n" +
                    $"{viewLocalization.Get("embed/view/rank").Format(item.Rank.GetName())}\n" +
                    $"{viewLocalization.Get("embed/view/max").Format(item.Max.IsInfinity() ?
                    viewLocalization.Get("embed/view/infinity") : item.Max)}\n" +
                    $"{viewLocalization.Get("embed/view/have").Format(dbUser.Inventory.GetItemAmount(item.Id))}\n\n" +
                    $"{viewLocalization.Get("embed/view/description")}\n{item.Info.GetDescription(context.Language)}\n" +
                    $"{(item.CanTrade ? viewLocalization.Get("embed/view/canTrade") : viewLocalization.Get("embed/view/canNotTrade"))}\n" +
                    $"{(item.CanLoose ? viewLocalization.Get("embed/view/canLoose") : viewLocalization.Get("embed/view/canNotLoose"))}\n\n" +
                    $"{viewLocalization.Get("embed/view/bonuses")}\n{item.GetRecoverInfo(context.Language)}")
                    .WithFooter($"ID: {itemId}")
                    .Build();
                var components = new ComponentBuilder()
                    .WithButton(viewLocalization.Get("btn/exit"), "exit", ButtonStyle.Danger)
                    .WithButton(viewLocalization.Get("btn/use"), "use", ButtonStyle.Success, disabled: !dbUser.Inventory.HaveItem(item.Id, 1))
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
                            await interaction.SendError(viewLocalization.Get("error/notHaveItem"), context.Language);
                            continue;
                        }
                        await item.Use(dbUser);
                        await interaction.SendInfo(viewLocalization.Get("embed/view/potionUsed"));
                        break;
                    default:
                        return;
                }
            }
        }
    }
}
