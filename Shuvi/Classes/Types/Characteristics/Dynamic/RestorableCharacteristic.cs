using Shuvi.Interfaces.Characteristics.Dynamic;

namespace Shuvi.Classes.Types.Characteristics.Dynamic
{
    public class RestorableCharacteristic : DynamicCharacteristic, IRestorableCharacteristic
    {
        public long RegenTime { get; protected set; }
        protected int _pointRegenTime;

        public RestorableCharacteristic(int max, long regenTime, int pointRegenTime) : base(max)
        {
            RegenTime = regenTime;
            _pointRegenTime = pointRegenTime;
        }
        public int GetCurrent()
        {
            var result = (int)(Max - Math.Ceiling((float)GetRemainingRegenTime() / _pointRegenTime));
            if (result < 0)
                return 0;
            return result > Max ? Max : result;
        }
        public int GetRemainingRegenTime()
        {
            int result = (int)(RegenTime - ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds());
            return result > 0 ? result : 0;
        }
        public void Add(int amount)
        {
            RegenTime -= amount * _pointRegenTime;
        }
        public void Reduce(int amount)
        {
            if (((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds() > RegenTime)
                RegenTime = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds() + (amount * _pointRegenTime);
            else
                RegenTime += amount * _pointRegenTime;
        }
    }
}
