using Shuvi.Interfaces.Combat;

namespace Shuvi.Interfaces.Effect
{
    public interface IEffects
    {
        public List<IEffect> All { get; }

        public void UpdateAll(ICombatEntity target);
        public void Add(IEffect effect);
        public void Remove(int index);
    }
}
