using Shuvi.Enums.Money;

namespace Shuvi.Interfaces.User
{
    public interface IUserWallet
    {
        public int Gold { get; }
        public int Dispoints { get; }

        public void Add(MoneyType type, int amount);
        public void Add(IUserWallet wallet);
        public void Add(Dictionary<MoneyType, int> money);
        public void Reduce(MoneyType type, int amount);
        public void Set(MoneyType type, int amount);
        public int Get(MoneyType type);
    }
}
