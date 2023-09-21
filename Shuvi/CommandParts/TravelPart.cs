using Discord;
using MongoDB.Driver;
using Shuvi.Classes.Data.User;
using Shuvi.Classes.Extensions;
using Shuvi.Classes.Factories.CustomEmbed;
using Shuvi.Classes.Types.Interaction;
using Shuvi.Enums.Localization;
using Shuvi.Enums.Rating;
using Shuvi.Interfaces.Map;
using Shuvi.Interfaces.User;
using Shuvi.Services.StaticServices.Database;
using Shuvi.Services.StaticServices.Localization;
using Shuvi.Services.StaticServices.Map;

namespace Shuvi.CommandParts
{
    public static class TravelPart
    {
        private static readonly LocalizationLanguagePart _localizationPart = LocalizationService.Get("travelPart");

        public static async Task Start(CustomInteractionContext context, IDatabaseUser dbUser)
        {
            var travelLocalization = _localizationPart.Get(context.Language);
            var choosedRegion = dbUser.Location.RegionId;
            var choosedLocation = dbUser.Location.LocationId;
            var regionOptions = GetRegionOptions(dbUser.Rating.Rank, context.Language);
            while (true)
            {
                var energyCosts = GetEnergyCosts(dbUser.Location, choosedRegion, choosedLocation);
                var embed = EmbedFactory.CreateUserEmbed(dbUser)
                    .WithAuthor(travelLocalization.Get("Embed/Travel/Author"))
                    .WithDescription($"{travelLocalization.Get("Embed/Travel/EnergyLeft")
                    .Format(dbUser.Characteristics.Energy.GetCurrent(), dbUser.Characteristics.Energy.Max)}\n" +
                    $"{(energyCosts == 0 ? "\n" : $"{travelLocalization.Get("Embed/Travel/EnergyWillTake").Format(energyCosts)}\n\n")}" +
                    $"{travelLocalization.Get("Embed/Travel/Region")} {dbUser.Location.GetRegion().Info.GetName(context.Language)}" +
                    $"{(energyCosts == 2 ? $" -> {WorldMap.GetRegion(choosedRegion).Info.GetName(context.Language)}" : string.Empty)}\n" +
                    $"{travelLocalization.Get("Embed/Travel/Location")} {dbUser.Location.GetLocation().Info.GetName(context.Language)}" +
                    $"{(energyCosts > 0 ? $" -> {WorldMap.GetRegion(choosedRegion).GetLocation(choosedLocation).Info.GetName(context.Language)}"
                    : string.Empty)}")
                    .WithImageUrl(WorldMap.Settings.PictureURL)
                    .Build();
                var components = new ComponentBuilder()
                    .WithSelectMenu("region", regionOptions, travelLocalization.Get("Select/ChooseRegion/Name"), row: 0)
                    .WithSelectMenu("location", GetLocationOptions(dbUser.Rating.Rank, choosedRegion, context.Language),
                    travelLocalization.Get("Select/ChooseLocation/Name"), row: 1)
                    .WithButton(travelLocalization.Get("Btn/Exit"), "exit", ButtonStyle.Danger, row: 2)
                    .WithButton(travelLocalization.Get("Btn/Travel"), "travel", ButtonStyle.Success, disabled: energyCosts == 0, row: 2)
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
                    case "region":
                        choosedRegion = int.Parse(interaction.Data.Values.First());
                        choosedLocation = 0;
                        break;
                    case "location":
                        choosedLocation = int.Parse(interaction.Data.Values.First());
                        break;
                    case "exit":
                        embed = EmbedFactory.CreateUserEmbed(dbUser, false, false)
                                .WithAuthor(travelLocalization.Get("Embed/Travel/Author"))
                                .WithDescription(travelLocalization.Get("Embed/TravelCancelled"))
                                .Build();
                        await context.Interaction.ModifyOriginalResponseAsync(msg =>
                        {
                            msg.Embed = embed;
                            msg.Components = new ComponentBuilder().Build();
                        });
                        return;
                    case "travel":
                        embed = EmbedFactory.CreateUserEmbed(dbUser)
                                .WithAuthor(travelLocalization.Get("Embed/Travel/Author"))
                                .WithDescription($"{travelLocalization.Get("Embed/Travaled/Desc")}\n" +
                                $"{(energyCosts == 2 ? $"{travelLocalization.Get("Embed/Travel/Region")} " +
                                $"{dbUser.Location.GetRegion().Info.GetName(context.Language)} -> " +
                                $"{WorldMap.GetRegion(choosedRegion).Info.GetName(context.Language)}" : string.Empty)}\n" +
                                $"{(energyCosts > 0 ? $"{travelLocalization.Get("Embed/Travel/Location")} " +
                                $"{dbUser.Location.GetLocation().Info.GetName(context.Language)} -> " +
                                $"{WorldMap.GetRegion(choosedRegion).GetLocation(choosedLocation).Info.GetName(context.Language)}"
                                : string.Empty)}")
                                .WithImageUrl(WorldMap.GetRegion(choosedRegion).GetLocation(choosedLocation).PictureURL)
                                .Build();
                        await context.Interaction.ModifyOriginalResponseAsync(msg =>
                        {
                            msg.Embed = embed;
                            msg.Components = new ComponentBuilder().Build();
                        });
                        dbUser.Location.SetRegion(choosedRegion, choosedLocation);
                        dbUser.Characteristics.Energy.Reduce(energyCosts);
                        await UserDatabase.UpdateUser(dbUser.Id, new UpdateDefinitionBuilder<UserData>()
                            .Set(x => x.EnergyRegenTime, dbUser.Characteristics.Energy.RegenTime)
                            .Set(x => x.RegionId, choosedRegion)
                            .Set(x => x.LocationId, choosedLocation));
                        return;
                }
            }
        }

        public static int GetEnergyCosts(IUserLocation currentLocation, int regionId, int locationId)
        {
            return currentLocation.RegionId == regionId ? (currentLocation.LocationId == locationId ? 0 : 1) : 2;
        }

        public static List<SelectMenuOptionBuilder> GetRegionOptions(Rank rank, Language lang)
        {
            var result = new List<SelectMenuOptionBuilder>();
            var index = 0;
            foreach (var region in WorldMap.Regions)
            {
                var description = region.Info.GetDescription(lang);
                if (description.Length > 70)
                {
                    description = $"{description[..70]}...";
                }
                if (region.CanVisit(rank))
                    result.Add(new(region.Info.GetName(lang), index.ToString(), description));
                index++;
            }
            return result;
        }

        public static List<SelectMenuOptionBuilder> GetLocationOptions(Rank rank, int regionId, Language lang)
        {
            var result = new List<SelectMenuOptionBuilder>();
            var index = 0;
            foreach (var location in WorldMap.GetRegion(regionId).Locations)
            {
                var description = location.Info.GetDescription(lang);
                if (description.Length > 70)
                {
                    description = $"{description[..70]}...";
                }
                result.Add(new(location.Info.GetName(lang), index.ToString(), description));
                index++;
            }
            return result;
        }
    }
}
