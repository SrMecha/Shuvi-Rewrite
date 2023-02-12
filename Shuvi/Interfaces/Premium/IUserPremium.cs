using Shuvi.Enums.Premium;

namespace Shuvi.Interfaces.Premium
{
    public interface IUserPremium
    {
        public PremiumAbilities PremiumAbilities { get; }
        public long PremiumExpires { get; }
        public int MoneyDonated { get; }

        public bool HavePremium();
        public bool IsSupported();
        public bool HaveAbility(PremiumAbilities ability);
        public void AddAbility(PremiumAbilities ability);
        public void RemoveAbility(PremiumAbilities ability);
        public void AddPremiumTime(long time);
        public void AddMoneyDonate(int amount);
    }
}
