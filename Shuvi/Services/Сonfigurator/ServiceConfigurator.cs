using Discord.WebSocket;
using MongoDB.Bson;
using MongoDB.Driver;
using Shuvi.Classes.Data.Customization;
using Shuvi.Classes.Data.Enemy;
using Shuvi.Classes.Data.Item;
using Shuvi.Classes.Data.Pet;
using Shuvi.Classes.Data.Shop;
using Shuvi.Classes.Data.User;
using Shuvi.Classes.Factories.Skill;
using Shuvi.Classes.Factories.Spell;
using Shuvi.Classes.Types.Skill.SkillList;
using Shuvi.Classes.Types.Spell.SpellList;
using Shuvi.Interfaces.Skill;
using Shuvi.Interfaces.Spell;
using Shuvi.Services.StaticServices.Check;
using Shuvi.Services.StaticServices.Database;
using Shuvi.Services.StaticServices.Emoji;
using Shuvi.Services.StaticServices.Info;
using Shuvi.Services.StaticServices.Localization;
using Shuvi.Services.StaticServices.Logs;
using Shuvi.Services.StaticServices.Map;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Shuvi.Services.Сonfigurator
{
    public static class ServiceConfigurator
    {
#if DEBUG
        private const string _databaseName = "ShuviTest";
#else
        private const string _databaseName = "ShuviTest";
#endif
        public static void Configure()
        {
            var mongoKey = GetEnviroment("MongoKey");
            if (mongoKey is null)
            {
                Console.WriteLine("Переменные среды не настроены (mongoKey)");
                return;
            }
            ConfigureDatabases(mongoKey!);
            EmojiService.Init();
            LocalizationService.Init();
            UserCheckService.Init(SettingsDatabase.LoadAdminsData());
            WorldMap.Init(SettingsDatabase.LoadMap());
            BotInfoService.Init(SettingsDatabase.LoadBotInfoData());
            ConfigureSpells();
            ConfigureSkills();
        }
        public static async Task ConfigureLogs(DiscordShardedClient client)
        {
            BotLogs.Init(await SettingsDatabase.LoadLogsData(), client);
        }
        private static void ConfigureDatabases(string mongoKey)
        {
            var _mongoClient = new MongoClient(mongoKey);
            EnemyDatabase.Init(_mongoClient.GetDatabase(_databaseName).GetCollection<EnemyData>("Enemies"));
            ImageDatabase.Init(_mongoClient.GetDatabase(_databaseName).GetCollection<ImageData>("Images"));
            ItemDatabase.Init(_mongoClient.GetDatabase(_databaseName).GetCollection<ItemData>("Items"));
            PetDatabase.Init(_mongoClient.GetDatabase(_databaseName).GetCollection<PetData>("Pets"));
            SettingsDatabase.Init(_mongoClient.GetDatabase(_databaseName).GetCollection<BsonDocument>("Settings"));
            UserDatabase.Init(_mongoClient.GetDatabase(_databaseName).GetCollection<UserData>("Users"));
            ShopDatabase.Init(_mongoClient.GetDatabase(_databaseName).GetCollection<ShopData>("Shops"));
        }
        private static void ConfigureSpells()
        {
            var validType = typeof(SpellBase).GetTypeInfo();
            Dictionary<string, ISpell> result = new();
            foreach (var definedType in Assembly.GetEntryAssembly()!.DefinedTypes.ToArray())
            {
                if (!validType.IsAssignableFrom(definedType) || !definedType.IsClass) continue;

                var resolver = (definedType.DeclaredConstructors.First().Invoke(Array.Empty<object>()) as ISpell)!;
                result[resolver.SpellName] = resolver;
            }
            SpellFactory.SetDictionary(new ReadOnlyDictionary<string, ISpell>(result));
        }
        private static void ConfigureSkills()
        {
            var validType = typeof(SkillBase).GetTypeInfo();
            Dictionary<string, ISkill> result = new();
            foreach (var definedType in Assembly.GetEntryAssembly()!.DefinedTypes.ToArray())
            {
                if (!validType.IsAssignableFrom(definedType) || !definedType.IsClass) continue;

                var resolver = (definedType.DeclaredConstructors.First().Invoke(Array.Empty<object>()) as ISkill)!;
                result[resolver.SkillName] = resolver;
            }
            SkillFactory.SetDictionary(new ReadOnlyDictionary<string, ISkill>(result));
        }
        public static string? GetEnviroment(string key)
        {
            return Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.User) ??
                Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Process) ??
                Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Machine);
        }
    }
}
