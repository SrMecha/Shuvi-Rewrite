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
using Shuvi.Interfaces.Pet;
using Shuvi.Interfaces.User;
using Shuvi.Services.StaticServices.Database;
using Shuvi.Services.StaticServices.Localization;

namespace Shuvi.CommandParts
{
    public static class EquipmentViewPart
    {
        private static readonly LocalizationLanguagePart _localizationPart = LocalizationService.Get("equipmentViewPart");

        public static async Task Start(CustomInteractionContext context, IDatabaseUser dbUser, IUser user, bool canEdit = false)
        {
            var equipmentLocalization = _localizationPart.Get(context.Language);
            var namesLocalization = LocalizationService.Get("names").Get(context.Language);
            while (context.LastInteraction is not null)
            {
                var embed = EmbedFactory.CreateUserEmbed(dbUser)
                    .WithAuthor(equipmentLocalization.Get("Embed/View/Equipment").Format(user.Username))
                    .WithDescription(GetEquipmentInfo(dbUser.Equipment, context.Language))
                    .Build();
                MessageComponent components;
                if (canEdit)
                    components = new ComponentBuilder()
                        .WithSelectMenu(GetSelectMenuBuilder(dbUser.Equipment, context.Language))
                        .WithButton(equipmentLocalization.Get("Btn/Exit"), "exit", ButtonStyle.Danger)
                        .Build();
                else
                    components = new ComponentBuilder()
                        .WithButton(equipmentLocalization.Get("Btn/Exit"), "exit", ButtonStyle.Danger)
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
                        await interaction.SendInfo(equipmentLocalization.Get("Embed/View/ItemUnequipped"));
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
                var embed = EmbedFactory.CreateUserEmbed(dbUser)
                    .WithAuthor(equipmentLocalization.Get("Embed/View/Equipment").Format(pet.Name))
                    .WithDescription(GetEquipmentInfo(pet.Equipment, context.Language))
                    .Build();
                MessageComponent components;
                if (canEdit)
                    components = new ComponentBuilder()
                        .WithSelectMenu(GetSelectMenuBuilder(pet.Equipment, context.Language))
                        .WithButton(equipmentLocalization.Get("Btn/Exit"), "exit", ButtonStyle.Danger)
                        .Build();
                else
                    components = new ComponentBuilder()
                        .WithButton(equipmentLocalization.Get("Btn/Exit"), "exit", ButtonStyle.Danger)
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
                        await interaction.SendInfo(equipmentLocalization.Get("Embed/View/ItemUnequipped"));
                        break;
                }

            }
        }
        private static string GetEquipmentInfo(IEquipment equipment, Language lang)
        {
            var result = new List<string>();
            foreach (var (type, item) in equipment.GetEquipmentsWithType())
                result.Add($"**{LocalizationService.Get("names").Get(lang).Get($"ItemType/{type.GetName()}")}:** " +
                    $"{(item is null ? _localizationPart.Get(lang).Get("Embed/View/NotHave") :
                    $"{item.Info.GetName(lang)}\n{item.Bonuses.GetBonusesInfo(lang)}")}");
            return string.Join("\n\n", result);
        }
        private static SelectMenuBuilder GetSelectMenuBuilder(IEquipment equipment, Language lang)
        {
            var options = new List<SelectMenuOptionBuilder>();
            foreach (var equipmentItem in equipment.GetEquipments())
                if (equipmentItem is not null)
                    options.Add(new SelectMenuOptionBuilder($"{LocalizationService.Get("names").Get(lang).Get($"ItemType/{equipmentItem.Type.GetName()}")}: " +
                        $"{equipmentItem.Info.GetName(lang)}",
                        ((int)equipmentItem.Type).ToString()));
            if (options.Count < 1)
            {
                options.Add(new SelectMenuOptionBuilder("NOT FOUND", "NOT FOUND"));
                return new SelectMenuBuilder()
                    .WithPlaceholder(_localizationPart.Get(lang).Get("Select/View/DontHaveEquipment"))
                    .WithCustomId("takeOff")
                    .WithOptions(options)
                    .WithDisabled(true);
            }
            return new SelectMenuBuilder()
                .WithPlaceholder(_localizationPart.Get(lang).Get("Select/View/Name"))
                .WithCustomId("takeOff")
                .WithOptions(options);
        }
    }
}
