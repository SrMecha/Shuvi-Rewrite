using Shuvi.Enums.Localization;
using Shuvi.Interfaces.User;

namespace Shuvi.Interfaces.Requirements
{
    public interface IBaseRequirements
    {
        public string GetRequirementsInfo(Language lang);
        public string GetRequirementsInfo(Language lang, IDatabaseUser user);
        public bool IsMeetRequirements(IDatabaseUser user);
    }
}
