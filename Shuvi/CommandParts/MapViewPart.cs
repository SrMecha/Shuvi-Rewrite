using Discord;
using MongoDB.Bson;
using Shuvi.Classes.Extensions;
using Shuvi.Classes.Factories.CustomEmbed;
using Shuvi.Classes.Types.Interaction;
using Shuvi.Enums.Localization;
using Shuvi.Interfaces.Map;
using Shuvi.Interfaces.User;
using Shuvi.Services.StaticServices.Database;
using Shuvi.Services.StaticServices.Info;
using Shuvi.Services.StaticServices.Localization;
using Shuvi.Services.StaticServices.Map;

namespace Shuvi.CommandParts
{
    public static class MapViewPart
    {
        private static readonly LocalizationLanguagePart _localizationPart = LocalizationService.Get("mapViewPart");

        public static async Task Start(CustomInteractionContext context, IDatabaseUser dbUser)
        {
            var mapLocalization = _localizationPart.Get(context.Language);
            var regionOptions = GetRegionOptions(context.Language);
            while (true)
            {
                var embed = EmbedFactory.CreateUserEmbed(dbUser)
                    .WithAuthor(mapLocalization.Get("Embed/World/Author"))
                    .WithDescription($"{mapLocalization.Get("Embed/World/Region").Format(dbUser.Location.GetRegion().Info.GetName(context.Language))}\n" +
                    $"{mapLocalization.Get("Embed/World/Location").Format(dbUser.Location.GetLocation().Info.GetName(context.Language))}")
                    .WithImageUrl(WorldMap.Settings.PictureURL)
                    .Build();
                var components = new ComponentBuilder()
                    .WithSelectMenu("region", regionOptions, mapLocalization.Get("Select/Region/Name"))
                    .Build();
                await context.Interaction.ModifyOriginalResponseAsync(msg => { msg.Embed = embed; msg.Components = components; });
                await context.LastInteraction.TryDeferAsync();
                var interaction = await context.WaitForButton();
                if (interaction is null)
                {
                    await context.CurrentMessage!.RemoveButtonsAsync();
                    return;
                }
                await ViewRegionPart(context, dbUser, WorldMap.GetRegion(int.Parse(interaction.Data.Values.First())));
            }
        }

        public static async Task ViewRegionPart(CustomInteractionContext context, IDatabaseUser dbUser, IMapRegion region)
        {
            var mapLocalization = _localizationPart.Get(context.Language);
            var locationOptions = GetLocationOptions(region, context.Language);
            while (true)
            {
                var embed = EmbedFactory.CreateUserEmbed(dbUser)
                    .WithAuthor($"{mapLocalization.Get("World/Name")} | {region.Info.GetName(context.Language)}")
                    .WithDescription($"{region.Info.GetDescription(context.Language)}\n" +
                    $"{mapLocalization.Get("Embed/Region/RequiredRank").Format(region.NeededRank.GetName())}\n" +
                    $"{mapLocalization.Get("Embed/RecommendedRank").Format(region.RecomendedRank.GetName())}")
                    .AddField(mapLocalization.Get("Embed/Region/Locations"),
                    $"```{GetLocationsString(region, context.Language)}```")
                    .WithImageUrl(region.PictureURL)
                    .Build();
                var components = new ComponentBuilder()
                    .WithSelectMenu("location", locationOptions, mapLocalization.Get("Select/Location/Name"), row: 0)
                    .WithButton(mapLocalization.Get("Btn/Back"), "back", ButtonStyle.Danger, row: 1)
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
                    case "location":
                        await ViewLocationPart(context, dbUser, region, region.GetLocation(int.Parse(interaction.Data.Values.First())));
                        break;
                    case "back":
                        return;
                }
            }
        }

        public static async Task ViewLocationPart(CustomInteractionContext context, IDatabaseUser dbUser, IMapRegion region, IMapLocation location)
        {
            var mapLocalization = _localizationPart.Get(context.Language);
            var options = GetEnemyOptions(dbUser, location, context.Language);
            var canViewEnemies = options.Count > 0;
            if (!canViewEnemies)
                options.Add(new("EMPTY", "empty"));
            var category = "main";
            while (true)
            {
                var embed = EmbedFactory.CreateUserEmbed(dbUser)
                    .WithAuthor($"{region.Info.GetName(context.Language)} | {location.Info.GetName(context.Language)}")
                    .WithDescription($"{location.Info.GetDescription(context.Language)}\n" +
                    $"{mapLocalization.Get("Embed/RecommendedRank").Format(location.RecomendedRank.GetName())}")
                    .AddCategoryFields(category, dbUser, location, context.Language)
                    .WithImageUrl(location.PictureURL)
                    .Build();
                var componentBuilder = new ComponentBuilder()
                    .WithButton(mapLocalization.Get("Btn/Main"), "main", ButtonStyle.Success, disabled: category == "main", row: 0)
                    .WithButton(mapLocalization.Get("Btn/Enemies"), "enemies", ButtonStyle.Success, disabled: category == "enemies", row: 0)
                    .WithButton(mapLocalization.Get("Btn/Resources"), "resources", ButtonStyle.Success, disabled: category == "resources", row: 0);
                if (category == "enemies" && canViewEnemies)
                {
                    componentBuilder.WithSelectMenu("select", options, mapLocalization.Get("Select/Enemies/Name"), row: 1);
                }

                componentBuilder.WithButton(mapLocalization.Get("Btn/Back"), "back", ButtonStyle.Danger, row: 2);
                await context.Interaction.ModifyOriginalResponseAsync(msg => { msg.Embed = embed; msg.Components = componentBuilder.Build(); });
                await context.LastInteraction.TryDeferAsync();
                var interaction = await context.WaitForButton();
                if (interaction is null)
                {
                    await context.CurrentMessage!.RemoveButtonsAsync();
                    return;
                }
                switch (interaction.Data.CustomId)
                {
                    case "back":
                        return;
                    case "select":
                        await EnemyViewPart.Start(context, dbUser, EnemyDatabase.GetEnemy(new ObjectId(interaction.Data.Values.First())));
                        break;
                    default:
                        category = interaction.Data.CustomId;
                        break;
                }
            }
        }

        public static EmbedBuilder AddCategoryFields(this EmbedBuilder embedBuilder, string category,
            IDatabaseUser dbUser, IMapLocation location, Language lang)
        {
            var localization = _localizationPart.Get(lang);
            return category switch
            {
                "main" => embedBuilder.AddField(localization.Get("Embed/Location/Shops"),
                    $"```{GetShopsString(location, lang)}```",
                    true).AddField(localization.Get("Embed/Location/Dungeons"),
                    $"```{GetDungeonsString(location, lang)}```",
                    true),
                "enemies" => embedBuilder.AddField(localization.Get("Embed/Location/Enemies"),
                    $"```{GetEnemiesString(dbUser, location, lang)}```",
                    true),
                "resources" => embedBuilder.AddField(localization.Get("Embed/Location/Mine"),
                    $"```{GetMineString(location, lang)}```",
                    true),
                _ => embedBuilder
            };
        }

        public static List<SelectMenuOptionBuilder> GetRegionOptions(Language lang)
        {
            var result = new List<SelectMenuOptionBuilder>();
            var i = 0;
            foreach (var region in WorldMap.Regions)
            {
                var description = region.Info.GetDescription(lang);
                if (description.Length > 70)
                    description = $"{description[..70]}...";
                result.Add(new(region.Info.GetName(lang), i.ToString(), description));
                i++;
            }
            return result;
        }

        public static List<SelectMenuOptionBuilder> GetLocationOptions(IMapRegion region, Language lang)
        {
            var result = new List<SelectMenuOptionBuilder>();
            var i = 0;
            foreach (var location in region.Locations)
            {
                var description = location.Info.GetDescription(lang);
                if (description.Length > 70)
                    description = $"{description[..70]}...";
                result.Add(new($"{location.Info.GetName(lang)}「{location.RecomendedRank.GetName()}」", i.ToString(), description));
                i++;
            }
            return result;
        }

        public static string GetLocationsString(IMapRegion region, Language lang)
        {
            var result = new List<string>();
            foreach (var location in region.Locations)
                result.Add($"· {location.Info.GetName(lang)}「{location.RecomendedRank.GetName()}」");
            return result.Count < 1 ? _localizationPart.Get(lang).Get("DontHave") : string.Join("\n", result);
        }

        public static string GetShopsString(IMapLocation location, Language lang)
        {
            var result = new List<string>();
            foreach (var shopId in location.Shops)
                result.Add($"· {ShopDatabase.GetReadonlyShop(shopId).Info.GetName(lang)}");
            return result.Count < 1 ? _localizationPart.Get(lang).Get("DontHave") : string.Join("\n", result);
        }

        public static string GetEnemiesString(IDatabaseUser dbUser, IMapLocation location, Language lang)
        {
            var result = new List<string>();
            foreach (var (enemy, chance) in location.Enemies.GetChances())
            {
                result.Add($"· {(EnemyInfoService.CanViewEnemy(dbUser, enemy) ? enemy.Info.GetName(lang) : "???")} [{chance:F2}%]");
            }
            return result.Count < 1 ? _localizationPart.Get(lang).Get("DontHave") : string.Join("\n", result);
        }

        public static string GetDungeonsString(IMapLocation location, Language lang)
        {
            return _localizationPart.Get(lang).Get("InDevelopment");
        }

        public static string GetMineString(IMapLocation location, Language lang)
        {
            return _localizationPart.Get(lang).Get("InDevelopment");
        }

        public static List<SelectMenuOptionBuilder> GetEnemyOptions(IDatabaseUser dbUser, IMapLocation location, Language lang)
        {
            var result = new List<SelectMenuOptionBuilder>();
            foreach (var enemy in location.Enemies.GetEnemies())
                if (EnemyInfoService.CanViewEnemy(dbUser, enemy))
                    result.Add(new(enemy.Info.GetName(lang), enemy.Id.ToString()));
            return result;
        }
    }
}
