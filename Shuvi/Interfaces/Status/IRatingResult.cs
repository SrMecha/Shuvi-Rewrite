using Shuvi.Enums.Rating;

namespace Shuvi.Interfaces.Status
{
    public interface IRatingResult : IActionResult
    {
        public bool IsRankChanged { get; init; }
        public Rank RankBefore { get; init; }
        public Rank RankAfter { get; init; }
    }
}
