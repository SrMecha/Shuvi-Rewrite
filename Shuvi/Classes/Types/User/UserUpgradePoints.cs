using Shuvi.Classes.Data.User;
using Shuvi.Classes.Settings;
using Shuvi.Interfaces.Characteristics;
using Shuvi.Interfaces.User;

namespace Shuvi.Classes.Types.User
{
    public class UserUpgradePoints : IUserUpgradePoints
    {
        public UserUpgradePoints()
        {

        }
        public int GetPoints(IDatabaseUser dbUser)
        {
            var tempUserData = new UserData();
            var pointsOccupied = 0;
            pointsOccupied += dbUser.Characteristics.Strength + dbUser.Characteristics.Agility +
                dbUser.Characteristics.Luck + dbUser.Characteristics.Endurance + dbUser.Characteristics.Intellect - 5;
            pointsOccupied += (dbUser.Characteristics.Mana.Max - tempUserData.MaxMana) / UserSettings.ManaPerUpPoint;
            pointsOccupied += (dbUser.Characteristics.Health.Max - tempUserData.MaxHealth) / UserSettings.HealthPerUpPoint;
            return (int)Math.Ceiling((float)dbUser.Rating.Points / UserSettings.RatingPerUpdgradePoint) - pointsOccupied;
        }
    }
}
