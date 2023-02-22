using Shuvi.Interfaces.Drop;

namespace Shuvi.Interfaces.Combat
{
    public interface ICombatEnemy : ICombatEntity
    {
        public int RatingGet { get; }
        public IItemsDrop Drop { get; }
    }
}
