using Shuvi.Classes.Data.Settings;

namespace Shuvi.Services.StaticServices.Info
{
    public static class BotInfoService
    {
        public static string Version { get; set; } = "VersionNotConfigured";
        public static string TosLink { get; set; } = string.Empty;

        public static void Init(BotInfoData data)
        {
            Version = data.Version;
            TosLink = data.TosLink;
        }
    }
}
