using MongoDB.Bson;
using Shuvi.Interfaces.Enemy;

namespace Shuvi.Interfaces.Rate
{
    public interface IEnemiesList : IRandomWithChance<ObjectId>
    {
        public IEnumerable<(IDatabaseEnemy, float)> GetChances();
        public IDatabaseEnemy GetEnemy(ObjectId id);
        public IEnumerable<IDatabaseEnemy> GetEnemies();
    }
}
