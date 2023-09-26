using Shuvi.Enums.Localization;
using Shuvi.Interfaces.Pet;
using Shuvi.Interfaces.Status;
using Shuvi.Interfaces.User;

namespace Shuvi.Interfaces.Requirements
{
    public interface IBaseRequirements
    {
        public int Strength { get; }
        public int Agility { get; }
        public int Luck { get; }
        public int Intellect { get; }
        public int Endurance { get; }
        public int Rank { get; }

        public IRequirementResult GetRequirementsInfo(Language lang);
        public IRequirementResult GetRequirementsInfo(Language lang, IDatabaseUser user);
        public IRequirementResult GetRequirementsInfo(Language lang, IDatabasePet pet);
        public bool IsMeetRequirement(string requirement, int amount, IDatabaseUser user);
        public bool IsMeetRequirement(string requirement, int amount, IDatabasePet pet);
        public IEnumerable<(string, int)> GetRequirements();
    }
}
