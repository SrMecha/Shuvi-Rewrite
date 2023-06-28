using Discord.Interactions;
using MongoDB.Driver;
using Shuvi.Classes.Data.Settings;
using Shuvi.Services.StaticServices.Database;

namespace Shuvi.Services.StaticServices.Info
{
    public static class BotInfoService
    {
        public static string Version { get; private set; } = "VersionNotConfigured";
        public static string TosLink { get; private set; } = string.Empty;
        public static string CommandsDescription { get; private set; } = "Commands not loaded yet.";
        public static long PlayerCount { get; private set; } = -1;

        public static void Init(BotInfoData data)
        {
            Version = data.Version;
            TosLink = data.TosLink;
        }

        public static async Task SetVersion(string version)
        {
            Version = version;
            await SettingsDatabase.UpdateBotInfo(new UpdateDefinitionBuilder<BotInfoData>()
                .Set(x => x.Version, Version));
        }

        public static void StartUpdateTotalUsers()
        {
            _ = Task.Run(async () =>
            {
                while (true)
                {
                    PlayerCount = await UserDatabase.GetTotalUsers();
                    await Task.Delay(new TimeSpan(0, 5, 0));
                }
            });
        }
        public static void ConfigureCommandsDescription(IEnumerable<ModuleInfo> modulesInfo)
        {
            var result = new List<string>();
            foreach (var commands in modulesInfo)
            {
                foreach (var info in commands.SlashCommands)
                {
                    result.Add($"**/{info.Name}** " +
                        $"{string.Join(" ", info.Parameters.Select(x => x.IsRequired ? $"`<{x.Name}>`" : $"`[{x.Name}]`"))} - " +
                        $"{info.Description}");
                }
            }
            CommandsDescription = string.Join("\n", result);
        }
    }
}
