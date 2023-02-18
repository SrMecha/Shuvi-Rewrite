using Shuvi.Classes.Data.Statistics;
using Shuvi.Interfaces.Statistics;

namespace Shuvi.Classes.Types.Statistics
{
    public class UserStatistics : IUserStatistics
    {
        public long CreatedAt { get; private set; }
        public long LiveTime { get; private set; }
        public int DeathCount { get; private set; }
        public int DungeonComplite { get; private set; }
        public int EnemyKilled { get; private set; }
        public int MaxRating { get; private set; }

        public UserStatistics(long createdAt, long liveTime, int deathCount, int dungeonComplite, int enemyKilled, int maxRating) 
        {
            CreatedAt = createdAt;
            LiveTime = liveTime;
            DeathCount = deathCount;
            DungeonComplite = dungeonComplite;
            EnemyKilled = enemyKilled;
            MaxRating = maxRating;
        }
        public UserStatistics(UserStatisticsData data)
        {
            CreatedAt = data.CreatedAt;
            LiveTime = data.LiveTime;
            DeathCount = data.DeathCount;
            DungeonComplite = data.DungeonComplite;
            EnemyKilled = data.EnemyKilled;
            MaxRating = data.MaxRating;
        }
        public void AddEnemyKilled(int amount = 1)
        {
            EnemyKilled += amount;
        }
        public void AddDeathCount(int amount = 1)
        {
            DeathCount += amount;
        }
        public void AddDungeonComplite(int amount = 1)
        {
            DungeonComplite += amount;
        }
        public void SetMaxRating(int amount)
        {
            MaxRating = amount;
        }
        public void RecordLiveTime()
        {
            LiveTime = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
        }
    }
}
