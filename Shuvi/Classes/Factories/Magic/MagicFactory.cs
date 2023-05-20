using Shuvi.Enums.Magic;
using Shuvi.Enums.User;

namespace Shuvi.Classes.Factories.Magic
{
    public static class MagicFactory
    {
        private static readonly Dictionary<UserRace, List<MagicType>> _magic = new()
        {
            {UserRace.ExMachina, new() { MagicType.Losen } },

            {UserRace.Beastmen, new() { MagicType.Earth, MagicType.Wind, MagicType.Water, MagicType.Fire } }
        };

        public static MagicType GetRandomRaceMagic(UserRace race)
        {
            var magicTypes = _magic.GetValueOrDefault(race, new() { MagicType.None });
            return magicTypes[new Random().Next(magicTypes.Count)];
        }
    }
}
