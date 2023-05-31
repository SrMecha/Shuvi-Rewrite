using MongoDB.Bson;
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
        public int TotalEnemyKilled { get; private set; }
        public Dictionary<ObjectId, int> EnemyKills { get; private set; }
        public int MaxRating { get; private set; }

        public UserStatistics(long createdAt, long liveTime, int deathCount, int dungeonComplite, Dictionary<ObjectId, int> enemyKills, int maxRating)
        {
            CreatedAt = createdAt;
            LiveTime = liveTime;
            DeathCount = deathCount;
            DungeonComplite = dungeonComplite;
            EnemyKills = enemyKills;
            TotalEnemyKilled = enemyKills.Values.Sum();
            MaxRating = maxRating;
        }
        public UserStatistics(UserStatisticsData data)
        {
            CreatedAt = data.CreatedAt;
            LiveTime = data.LiveTime;
            DeathCount = data.DeathCount;
            DungeonComplite = data.DungeonComplite;
            EnemyKills = data.EnemyKills;
            TotalEnemyKilled = data.EnemyKills.Values.Sum();
            MaxRating = data.MaxRating;
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
        public void UpdateMaxRating(int currentRating)
        {
            if (MaxRating < currentRating)
                MaxRating = currentRating;
        }
        public void RecordLiveTime()
        {
            LiveTime = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
        }
        public void AddEnemyKilled(ObjectId enemyId, int amount = 1)
        {
            TotalEnemyKilled += amount;
            if (EnemyKills.ContainsKey(enemyId))
                EnemyKills[enemyId] += amount;
            else
                EnemyKills[enemyId] = amount;
        }
        public int GetEnemyKills(ObjectId enemyId)
        {
            return EnemyKills.GetValueOrDefault(enemyId, 0);
        }
    }
}
