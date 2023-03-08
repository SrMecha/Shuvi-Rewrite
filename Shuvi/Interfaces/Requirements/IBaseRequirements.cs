using Shuvi.Enums.Localization;
using Shuvi.Interfaces.Pet;
using Shuvi.Interfaces.User;

namespace Shuvi.Interfaces.Requirements
{
    public interface IBaseRequirements
    {
        public string GetRequirementsInfo(Language lang);
        public string GetRequirementsInfo(Language lang, IDatabaseUser user);
        public string GetRequirementsInfo(Language lang, IDatabasePet pet);
        public bool IsMeetRequirements(IDatabaseUser user);
        public bool IsMeetRequirements(IDatabasePet pet);
    }
}
