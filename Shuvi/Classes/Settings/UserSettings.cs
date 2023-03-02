namespace Shuvi.Classes.Settings
{
    public static class UserSettings
    {
        public static uint StandartColor { get; } = 0xf0732f;
        public static int StandartMana { get; } = 10;
        public static int StandartHealth { get; } = 100;
        public static int StandartEnergy { get; } = 10;
        public static int HealthPointRegenTime { get; } = 20;
        public static int ManaPointRegenTime { get; } = 30;
        public static int EnergyPointRegenTime { get; } = 120;

        public static int HealthPerUpPoint { get; } = 3;
        public static int ManaPerUpPoint { get; } = 3;
        public static int RatingPerUpdgradePoint { get; } = 5;
        public static int EndurancePerEnergy { get; } = 10;

        public static int EnergyDisplayMax { get; } = 5;
        public static int HealthDisplayMax { get; } = 5;
        public static int ManaDisplayMax { get; } = 5;
    }
}
