using Shuvi.Enums.Money;
using Shuvi.Interfaces.User;

namespace Shuvi.Classes.Types.User
{
    public class UserWallet : IUserWallet
    {
        public int Gold { get; private set; } = 0;
        public int Dispoints { get; private set; } = 0;

        public UserWallet() { }
        public UserWallet(int gold, int dispoints)
        {
            Gold = gold;
            Dispoints = dispoints;
        }
        public void Add(MoneyType type, int amount)
        {
            switch (type)
            {
                case MoneyType.Gold:
                    Gold += amount;
                    break;
                case MoneyType.Dispoints:
                    Dispoints += amount;
                    break;
            }
        }
        public void Add(IUserWallet wallet)
        {
            Gold += wallet.Gold;
            Dispoints += wallet.Dispoints;
        }
        public int Get(MoneyType type)
        {
            return type switch
            {
                MoneyType.Gold => Gold,
                MoneyType.Dispoints => Dispoints,
                _ => 0
            };
        }
        public void Reduce(MoneyType type, int amount)
        {
            switch (type)
            {
                case MoneyType.Gold:
                    Gold -= amount;
                    break;
                case MoneyType.Dispoints:
                    Dispoints -= amount;
                    break;
            }
        }
        public void Set(MoneyType type, int amount)
        {
            switch (type)
            {
                case MoneyType.Gold:
                    Gold = amount;
                    break;
                case MoneyType.Dispoints:
                    Dispoints = amount;
                    break;
            }
        }
    }
}
