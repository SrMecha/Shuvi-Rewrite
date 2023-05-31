using Shuvi.Enums.Premium;
using Shuvi.Interfaces.Premium;

namespace Shuvi.Classes.Types.Premium
{
    public class UserPremium : IUserPremium
    {
        public PremiumAbilities PremiumAbilities { get; private set; }
        public long PremiumExpires { get; private set; }
        public int MoneyDonated { get; private set; }

        public UserPremium(PremiumAbilities premiumAbilities, long premiumExpires, int moneyDonated)
        {
            PremiumAbilities = premiumAbilities;
            PremiumExpires = premiumExpires;
            MoneyDonated = moneyDonated;
        }

        public bool IsSupported()
        {
            return MoneyDonated > 0;
        }
        public bool HavePremium()
        {
            return ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds() <= PremiumExpires;
        }
        public void AddAbility(PremiumAbilities ability)
        {
            PremiumAbilities |= ability;
        }
        public void RemoveAbility(PremiumAbilities ability)
        {
            PremiumAbilities ^= ability;
        }
        public void AddMoneyDonate(int amount)
        {
            MoneyDonated += amount;
        }
        public void AddPremiumTime(long time)
        {
            var timeNow = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
            if (timeNow > PremiumExpires)
                PremiumExpires = timeNow + time;
            else
                PremiumExpires += time;
        }
        public bool HaveAbility(PremiumAbilities ability)
        {
            return PremiumAbilities.HasFlag(ability);
        }
    }
}
