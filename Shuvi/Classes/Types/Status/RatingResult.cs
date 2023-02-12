using Shuvi.Enums.Rating;
using Shuvi.Interfaces.Status;

namespace Shuvi.Classes.Types.Status
{
    public class RatingResult : ActionResult, IRatingResult
    {
        public bool IsRankChanged { get; private set; }
        public Rank RankBefore { get; private set; }
        public Rank RankAfter { get; private set; }

        public RatingResult(Rank rankBefore, Rank rankAfter, string description) : base(description)
        {
            RankBefore = rankBefore;
            RankAfter = rankAfter;
            IsRankChanged = rankBefore == rankAfter;
        }
    }
}
