﻿using Shuvi.Enums.Characteristic;

namespace Shuvi.Interfaces.Characteristics.Static
{
    public interface IStaticCharacteristics
    {
        public int Strength { get; }
        public int Agility { get; }
        public int Luck { get; }
        public int Intellect { get; }
        public int Endurance { get; }

        public void Add(IStaticCharacteristics characteristics);
        public void Add(StaticCharacteristic characteristic, int amount);
        public void Reduce(IStaticCharacteristics characteristics);
        public void Reduce(StaticCharacteristic characteristic, int amount);
        public void Set(int strength, int agility, int luck, int intellect, int endurance);
        public IEnumerator<(StaticCharacteristic, int)> GetEnumerator();
    }
}
