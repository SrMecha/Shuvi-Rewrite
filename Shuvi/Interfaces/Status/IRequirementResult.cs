namespace Shuvi.Interfaces.Status
{
    public interface IRequirementResult : IActionResult
    {
        public bool IsMeetRequirements { get; }
    }
}
