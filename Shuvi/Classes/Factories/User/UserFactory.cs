using Shuvi.Classes.Data.Statistics;
using Shuvi.Classes.Data.User;
using Shuvi.Classes.Extensions;
using Shuvi.Enums.Localization;
using Shuvi.Enums.User;

namespace Shuvi.Classes.Factories.User
{
    public static class UserFactory
    {
        public static UserRace GenerateRandomUserRace()
        {
            var values = Enum.GetValues(typeof(UserRace));
            return (UserRace?)values.GetValue(new Random().Next(values.Length)) ?? UserRace.ExMachina;
        }
        public static UserSubrace GenerateRandomUserBreed(UserRace race)
        {
            var races = race.GetRaceBreeds();
            return (UserSubrace?)races.GetValue(new Random().Next(races.Length)) ?? UserSubrace.NoSubrace;
        }
        public static UserData CreateUser(ulong id)
        {
            var race = GenerateRandomUserRace();
            return new UserData
            {
                Id = id,
                Race = race,
                Subrace = GenerateRandomUserBreed(race),
                ManaRegenTime = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds(),
                HealthRegenTime = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds(),
                EnergyRegenTime = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds(),
                Statistics = new UserStatisticsData
                {
                    CreatedAt = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds(),
                    LiveTime = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds()
                }
            };
        }
    }
}
