using Shuvi.Interfaces.Status;

namespace Shuvi.Classes.Types.Status
{
    public class RequirementResult : ActionResult, IRequirementResult
    {
        public bool IsMeetRequirements {get; private set;}

        public RequirementResult(bool isMeetRequirements, string description) : base(description)
        {
            IsMeetRequirements = isMeetRequirements;
        }
    }
}
