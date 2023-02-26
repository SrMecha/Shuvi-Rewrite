using Shuvi.Classes.Extensions;
using Shuvi.Classes.Types.Status;
using Shuvi.Enums.Localization;
using Shuvi.Enums.Rating;
using Shuvi.Interfaces.Status;
using Shuvi.Interfaces.User;
using Shuvi.Services.StaticServices.Localization;

namespace Shuvi.Classes.Types.User
{
    public class UserRating : IUserRating
    {
        private readonly Language _language;

        public int Points { get; private set; }
        public Rank Rank { get; private set; }

        public UserRating(int rating, Language lang = Language.Eng)
        {
            _language = lang;
            Points = rating;
            Rank = GetRank(rating);
        }
        public IRatingResult AddPoints(int amount, Rank rank)
        {
            if (Rank > rank)
                return new RatingResult(Rank, Rank, LocalizationService.Get("status").Get(_language).Get("raiting/low"));
            Points += amount;
            var rankBefore = Rank;
            Rank = GetRank(Points);
            if (rankBefore < Rank)
                return new RatingResult(
                    rankBefore,
                    Rank,
                    string.Format(LocalizationService.Get("status").Get(_language).Get("raiting/earnUp"), amount, Rank.GetString())
                    );
            return new RatingResult(
                rankBefore,
                Rank,
                string.Format(LocalizationService.Get("status").Get(_language).Get("raiting/earn"), amount)
                );
        }
        public IRatingResult AddPoints(int amount)
        {
            Points += amount;
            var rankBefore = Rank;
            Rank = GetRank(Points);
            if (rankBefore < Rank)
                return new RatingResult(
                    rankBefore,
                    Rank,
                    string.Format(LocalizationService.Get("status").Get(_language).Get("raiting/earnUp"), amount, Rank.GetString())
                    );
            return new RatingResult(
                rankBefore,
                Rank,
                string.Format(LocalizationService.Get("status").Get(_language).Get("raiting/earn"), amount)
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
        public IRatingResult RemovePoints(int amount)
        {
            Points -= amount;
            var rankBefore = Rank;
            Rank = GetRank(Points);
            if (rankBefore > Rank)
                return new RatingResult(
                    rankBefore,
                    Rank,
                    string.Format(LocalizationService.Get("status").Get(_language).Get("raiting/lostDown"), amount, Rank.GetString())
                    );
            return new RatingResult(
                rankBefore,
                Rank,
                string.Format(LocalizationService.Get("status").Get(_language).Get("raiting/lost"), amount)
                );
        }
        public IRatingResult SetPoints(int amount)
        {
            Points = amount;
            var rankBefore = Rank;
            Rank = GetRank(Points);
            if (rankBefore != Rank)
                return new RatingResult(
                    rankBefore,
                    Rank,
                    string.Format(LocalizationService.Get("status").Get(_language).Get("raiting/setChange"), amount, Rank.GetString())
                    );
            return new RatingResult(
                rankBefore,
                Rank,
                string.Format(LocalizationService.Get("status").Get(_language).Get("raiting/set"), amount)
                );
        }
    }
}
