using Shuvi.Interfaces.Characteristics.Dynamic;

namespace Shuvi.Classes.Types.Characteristics.Dynamic
{
    public class NotRestorableCharacteristic : DynamicCharacteristic, INotRestorableCharacteristic
    {
        public int Now { get; protected set; }

        public NotRestorableCharacteristic(int max, int now) : base(max)
        {
            Now = now;
        }
        public void Add(int amount)
        {
            Now += amount;
            if (Now > Max)
                Now = Max;
        }
        public void Reduce(int amount)
        {
            Now -= amount;
            if (Now < 0)
                Now = 0;
        }
    }
}
