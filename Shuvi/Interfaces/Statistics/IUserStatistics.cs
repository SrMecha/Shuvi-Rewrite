using MongoDB.Bson;

namespace Shuvi.Interfaces.Statistics
{
    public interface IUserStatistics
    {
        public long CreatedAt { get; }
        public long LiveTime { get; }
        public int DeathCount { get; }
        public int DungeonComplite { get; }
        public int TotalEnemyKilled { get; }
        public Dictionary<ObjectId, int> EnemyKills { get; }
        public int MaxRating { get; }

        public void AddEnemyKilled(ObjectId enemyId, int amount = 1);
        public void AddDeathCount(int amount = 1);
        public void AddDungeonComplite(int amount = 1);
        public void SetMaxRating(int amount);
        public void RecordLiveTime();
        public int GetEnemyKills(ObjectId enemyId);
    }
}
