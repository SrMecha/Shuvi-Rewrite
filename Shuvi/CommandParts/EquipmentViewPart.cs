using Discord;
using MongoDB.Bson;
using MongoDB.Driver;
using Shuvi.Classes.Data.Pet;
using Shuvi.Classes.Data.User;
using Shuvi.Classes.Extensions;
using Shuvi.Classes.Factories.CustomEmbed;
using Shuvi.Classes.Types.Interaction;
using Shuvi.Enums.Item;
using Shuvi.Enums.Localization;
using Shuvi.Interfaces.Equipment;
using Shuvi.Interfaces.Items;
using Shuvi.Interfaces.Pet;
using Shuvi.Interfaces.User;
using Shuvi.Services.StaticServices.Database;
using Shuvi.Services.StaticServices.Localization;

namespace Shuvi.CommandParts
{
    public static class EquipmentViewPart
    {
        private static readonly LocalizationLanguagePart _localizationPart = LocalizationService.Get("equipmentViewPart");

        public static async Task Start(CustomInteractionContext context, IDatabaseUser dbUser, bool canEdit = false)
        {
            var equipmentLocalization = _localizationPart.Get(context.Language);
            var namesLocalization = LocalizationService.Get("names").Get(context.Language);
            while (context.LastInteraction is not null)
            {
                var embed = EmbedFactory.CreateUserEmbed(context.User, dbUser)
                    .WithAuthor(equipmentLocalization.Get("embed/view/equipment").Format(context.User.Username))
                    .WithDescription(GetEquipmentInfo(dbUser.Equipment, context.Language))
                    .Build();
                MessageComponent components;
                if (canEdit)
                    components = new ComponentBuilder()
                        .WithSelectMenu("takeOff", GetSelectMenuOptions(dbUser.Equipment, context.Language),
                        equipmentLocalization.Get("select/view/name"))
                        .WithButton(equipmentLocalization.Get("btn/exit"), "exit", ButtonStyle.Danger)
                        .Build();
                else
                    components = new ComponentBuilder()
                        .WithButton(equipmentLocalization.Get("btn/exit"), "exit", ButtonStyle.Danger)
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
                        var type = (ItemType)int.Parse(interaction.Data.Values.First());
                        dbUser.Equipment.SetEquipment(type, null);
                        await UserDatabase.UpdateUser(dbUser.Id,
                            new UpdateDefinitionBuilder<UserData>().Set(nameof(type), BsonNull.Value));
                        await interaction.SendInfo(equipmentLocalization.Get("embed/view/itemUnequipped"));
                        break;
                }

            }
        }
        public static async Task Start(CustomInteractionContext context, IDatabaseUser dbUser, IDatabasePet pet, bool canEdit = false)
        {
            var equipmentLocalization = _localizationPart.Get(context.Language);
            var namesLocalization = LocalizationService.Get("names").Get(context.Language);
            while (context.LastInteraction is not null)
            {
                var embed = EmbedFactory.CreateUserEmbed(context.User, dbUser)
                    .WithAuthor(equipmentLocalization.Get("embed/view/equipment").Format(pet.Name))
                    .WithDescription(GetEquipmentInfo(pet.Equipment, context.Language))
                    .Build();
                MessageComponent components;
                if (canEdit)
                    components = new ComponentBuilder()
                        .WithSelectMenu("takeOff", GetSelectMenuOptions(pet.Equipment, context.Language),
                        equipmentLocalization.Get("select/view/name"))
                        .WithButton(equipmentLocalization.Get("btn/exit"), "exit", ButtonStyle.Danger)
                        .Build();
                else
                    components = new ComponentBuilder()
                        .WithButton(equipmentLocalization.Get("btn/exit"), "exit", ButtonStyle.Danger)
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
                        var type = (ItemType)int.Parse(interaction.Data.CustomId);
                        dbUser.Equipment.SetEquipment(type, null);
                        await PetDatabase.UpdatePet(pet.Id,
                            new UpdateDefinitionBuilder<PetData>().Set(nameof(type), BsonNull.Value));
                        await interaction.SendInfo(equipmentLocalization.Get("embed/view/itemUnequipped"));
                        break;
                }

            }
        }
        private static string GetEquipmentInfo(IEquipment equipment, Language lang)
        {
            var result = new List<string>();
            foreach (var (type, item) in equipment.GetEquipmentsWithType())
                result.Add($"**{LocalizationService.Get("names").Get(lang).Get($"itemType/{type.GetLowerName()}")}:** " +
                    $"{(item is null ? _localizationPart.Get(lang).Get("embed/view/notHave") :
                    $"{item.Info.GetName(lang)}\n{item.Bonuses.GetBonusesInfo(lang)}")}");
            return string.Join("\n\n", result);
        }
        private static List<SelectMenuOptionBuilder> GetSelectMenuOptions(IEquipment equipment, Language lang)
        {
            var result = new List<SelectMenuOptionBuilder>();
            foreach (var (type, _) in equipment.GetIdsWithType())
                result.Add(new SelectMenuOptionBuilder(LocalizationService.Get("names").Get(lang).Get($"itemType/{type.GetLowerName()}"),
                    ((int)type).ToString()));
            return result;
        }
    }
}
