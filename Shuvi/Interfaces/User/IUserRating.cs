using Shuvi.Enums.Localization;
using Shuvi.Enums.Rating;
using Shuvi.Interfaces.Status;

namespace Shuvi.Interfaces.User
{
    public interface IUserRating
    {
        public int Points { get; }
        public Rank Rank { get; }
        public Rank GetRank(int rating);
        public IRatingResult AddPoints(int amount, Rank rank, Language lang);
        public IRatingResult AddPoints(int amount, Language lang);
        public IRatingResult RemovePoints(int amount, Language lang);
        public IRatingResult SetPoints(int amount, Language lang);
    }
}
