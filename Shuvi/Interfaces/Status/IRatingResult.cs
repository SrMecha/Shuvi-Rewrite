using Shuvi.Enums.Rating;

namespace Shuvi.Interfaces.Status
{
    public interface IRatingResult : IActionResult
    {
        public bool IsRankChanged { get; }
        public Rank RankBefore { get; }
        public Rank RankAfter { get; }
    }
}
