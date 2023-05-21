using Shuvi.Enums.User;

namespace Shuvi.Classes.Factories.Profession
{
    public static class ProfessionFactory
    {
        private static readonly Dictionary<UserRace, List<UserProfession>> _professions = new()
        {
            {UserRace.ExMachina, new() { UserProfession.Prufer, UserProfession.Kampfer } },

            {UserRace.Beastmen, new() { UserProfession.Hunter } }
        };
        public static List<UserProfession> GetProfessionsByRace(UserRace race)
        {
            return _professions.GetValueOrDefault(race, new() { UserProfession.NoProfession });
        }
    }
}
