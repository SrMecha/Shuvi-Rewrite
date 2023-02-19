using Shuvi.Classes.Types.Characteristics;
using Shuvi.Interfaces.Characteristics;
using Shuvi.Interfaces.Combat;
using Shuvi.Interfaces.Effect;

namespace Shuvi.Classes.Types.Effect
{
    public class Effects : IEffects
    {
        public List<IEffect> All { get; private set; }

        public Effects() 
        {
            All = new();
        }
        public IBonusesCharacteristics UpdateAll(ICombatEntity target)
        {
            var result = new BonusesCharacteristics();
            for (int i = All.Count - 1; i >= 0; i--)
            {
                All[i].Update(target);
                result.Add(All[i].Bonuses);
                if (All[i].TimeLeft <= 0)
                    All.RemoveAt(i);
            }
            return result;
        }
        public void Add(IEffect effect)
        {
            All.Add(effect);
        }
        public void Remove(int index)
        {
            if (index >= All.Count)
                return;
            All.RemoveAt(index);
        }
    }
}
