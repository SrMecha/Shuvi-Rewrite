namespace Shuvi.Classes.Data.Statistics
{
    public sealed class UserStatisticsData
    {
        public long CreatedAt { get; set; } = 1;
        public long LiveTime { get; set; } = 1;
        public int DeathCount { get; set; } = 0;
        public int DungeonComplite { get; set; } = 0;
        public int EnemyKilled { get; set; } = 0;
        public int MaxRating { get; set; } = 0;
    }
}
