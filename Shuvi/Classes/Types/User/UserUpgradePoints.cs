using Shuvi.Classes.Data.User;
using Shuvi.Classes.Settings;
using Shuvi.Interfaces.Characteristics;
using Shuvi.Interfaces.User;

namespace Shuvi.Classes.Types.User
{
    public class UserUpgradePoints : IUserUpgradePoints
    {
        private IUserRating Rating { get; init; }
        private IUserCharacteristics Characteristics { get; init; }

        public UserUpgradePoints(IUserRating rating, IUserCharacteristics characteristics)
        {
            Rating = rating;
            Characteristics = characteristics;
        }
        public int GetPoints()
        {
            var tempUserData = new UserData();
            var pointsOccupied = 0;
            pointsOccupied += Characteristics.Strength + Characteristics.Agility +
                Characteristics.Luck + Characteristics.Endurance + Characteristics.Intellect - 5;
            pointsOccupied += (Characteristics.Mana.Max - tempUserData.MaxMana) / UserSettings.ManaPerUpPoint;
            pointsOccupied += (Characteristics.Health.Max - tempUserData.MaxHealth) / UserSettings.HealthPerUpPoint;
            return (int)Math.Ceiling((float)Rating.Points / UserSettings.RatingPerUpdgradePoint) - pointsOccupied;
        }
    }
}
