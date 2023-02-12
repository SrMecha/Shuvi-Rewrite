using Shuvi.Enums.Localization;
using Shuvi.Enums.Requirements;
using Shuvi.Interfaces.Requirements;
using Shuvi.Interfaces.User;

namespace Shuvi.Classes.Types.Requirements
{
    public class BaseRequirements : IBaseRequirements
    {
        protected Dictionary<BaseRequirement, int> _requirements = new();

        public BaseRequirements(Dictionary<BaseRequirement, int> requirements) 
        {
            _requirements = requirements;
        }
        public string GetRequirementsInfo(Language lang)
        {
            throw new NotImplementedException();
        }
        public string GetRequirementsInfo(Language lang, IDatabaseUser user)
        {
            throw new NotImplementedException();
        }
        public bool IsMeetRequirements(IDatabaseUser user)
        {
            throw new NotImplementedException();
        }
    }
}
