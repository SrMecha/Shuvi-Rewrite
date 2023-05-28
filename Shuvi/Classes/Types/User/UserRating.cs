using Shuvi.Classes.Extensions;
using Shuvi.Classes.Types.Status;
using Shuvi.Enums.Localization;
using Shuvi.Enums.Rating;
using Shuvi.Interfaces.Status;
using Shuvi.Interfaces.User;
using Shuvi.Services.StaticServices.Event;
using Shuvi.Services.StaticServices.Localization;

namespace Shuvi.Classes.Types.User
{
    public class UserRating : IUserRating
    {
        private IDatabaseUser _dbUser;

        public int Points { get; private set; }
        public Rank Rank { get; private set; }

        public UserRating(IDatabaseUser dbUser, int rating)
        {
            _dbUser = dbUser;
            Points = rating;
            Rank = GetRank(rating);
        }
        public IRatingResult AddPoints(int amount, Rank rank, Language lang)
        {
            if (Rank > rank)
                return new RatingResult(Rank, Rank, LocalizationService.Get("status").Get(lang).Get("rating/low"));
            Points += amount;
            var rankBefore = Rank;
            Rank = GetRank(Points);
            if (rankBefore < Rank)
            {
                EventManager.InvokeOnPlayerRankUp(_dbUser, rankBefore, Rank);
                return new RatingResult(
                    rankBefore,
                    Rank,
                    string.Format(LocalizationService.Get("status").Get(lang).Get("rating/earnUp"), amount, Rank.GetName())
                    );
            }
            return new RatingResult(
                rankBefore,
                Rank,
                LocalizationService.Get("status").Get(lang).Get("rating/earn").Format(amount)
                );
        }
        public IRatingResult AddPoints(int amount, Language lang)
        {
            Points += amount;
            var rankBefore = Rank;
            Rank = GetRank(Points);
            if (rankBefore < Rank)
            {
                EventManager.InvokeOnPlayerRankUp(_dbUser, rankBefore, Rank);
                return new RatingResult(
                    rankBefore,
                    Rank,
                    string.Format(LocalizationService.Get("status").Get(lang).Get("rating/earnUp"), amount, Rank.GetName())
                    );
            }
            return new RatingResult(
                rankBefore,
                Rank,
                string.Format(LocalizationService.Get("status").Get(lang).Get("rating/earn"), amount)
                );
        }
        public Rank GetRank(int rating)
        {
            return rating switch
            {
                >= 5000 => Rank.SSS,
                >= 3500 => Rank.SS,
                >= 2000 => Rank.S,
                >= 1000 => Rank.A,
                >= 600 => Rank.B,
                >= 300 => Rank.C,
                >= 100 => Rank.D,
                _ => Rank.E
            };
        }
        public IRatingResult RemovePoints(int amount, Language lang)
        {
            Points -= amount;
            var rankBefore = Rank;
            Rank = GetRank(Points);
            if (rankBefore > Rank)
                return new RatingResult(
                    rankBefore,
                    Rank,
                    string.Format(LocalizationService.Get("status").Get(lang).Get("rating/lostDown"), amount, Rank.GetName())
                    );
            return new RatingResult(
                rankBefore,
                Rank,
                string.Format(LocalizationService.Get("status").Get(lang).Get("rating/lost"), amount)
                );
        }
        public IRatingResult SetPoints(int amount, Language lang)
        {
            Points = amount;
            var rankBefore = Rank;
            Rank = GetRank(Points);
            if (rankBefore != Rank)
                return new RatingResult(
                    rankBefore,
                    Rank,
                    string.Format(LocalizationService.Get("status").Get(lang).Get("rating/setChange"), amount, Rank.GetName())
                    );
            return new RatingResult(
                rankBefore,
                Rank,
                string.Format(LocalizationService.Get("status").Get(lang).Get("rating/set"), amount)
                );
        }
    }
}
