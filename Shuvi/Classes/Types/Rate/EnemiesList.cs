using MongoDB.Bson;
using Shuvi.Interfaces.Enemy;
using Shuvi.Interfaces.Rate;
using Shuvi.Services.StaticServices.Database;

namespace Shuvi.Classes.Types.Rate
{
    public class EnemiesList : RandomWithChance<ObjectId>, IEnemiesList
    {
        public EnemiesList(Dictionary<ObjectId, int> enemies) : base(enemies) { }
        public IEnumerable<(IDatabaseEnemy, float)> GetChances()
        {
            float all = _items.Values.Sum();
            foreach (var (enemyId, chance) in _items)
                yield return (GetEnemy(enemyId), (chance / all) * 100);
        }
        public IEnumerable<IDatabaseEnemy> GetEnemies()
        {
            foreach (var id in _items.Keys)
                yield return GetEnemy(id);
        }
        public IDatabaseEnemy GetEnemy(ObjectId id)
        {
            return EnemyDatabase.GetEnemy(id);
        }
    }
}
