using Shuvi.Interfaces.Enemy;
using Shuvi.Interfaces.User;

namespace Shuvi.Services.StaticServices.Info
{
    public static class EnemyInfoService
    {
        public static bool CanViewEnemyCharacteristics(IDatabaseUser user, IDatabaseEnemy enemy)
        {
            return (user.Characteristics.Intellect + user.Equipment.GetBonuses().Intellect) * (user.Statistics.GetEnemyKills(enemy.Id) + 1) >=
                ((int)enemy.Rank * 2 + 1) * 1;
        }
        public static bool CanViewEnemyAbilities(IDatabaseUser user, IDatabaseEnemy enemy)
        {
            return (user.Characteristics.Intellect + user.Equipment.GetBonuses().Intellect) * (user.Statistics.GetEnemyKills(enemy.Id) + 1) >=
                ((int)enemy.Rank * 2 + 1) * 2;
        }
        public static bool CanViewEnemyDrop(IDatabaseUser user, IDatabaseEnemy enemy)
        {
            return (user.Characteristics.Intellect + user.Equipment.GetBonuses().Intellect) * (user.Statistics.GetEnemyKills(enemy.Id) + 1) >=
                ((int)enemy.Rank * 2 + 1) * 4;
        }
        public static bool CanViewEnemyActionChances(IDatabaseUser user, IDatabaseEnemy enemy)
        {
            return (user.Characteristics.Intellect + user.Equipment.GetBonuses().Intellect) * (user.Statistics.GetEnemyKills(enemy.Id) + 1) >=
                ((int)enemy.Rank * 2 + 1) * 5;
        }
    }
}
