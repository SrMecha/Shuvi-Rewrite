using Shuvi.Enums.Premium;

namespace Shuvi.Interfaces.Premium
{
    public interface IUserPremium
    {
        public PremiumType Type { get; }
        public PremiumAbilities PremiumAbilities { get; }
        public long PremiumExpires { get; }
        public int MoneyDonated { get; }

        public bool HaveAbility(PremiumAbilities ability);
    }
}
