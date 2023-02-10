using Shuvi.Interfaces.Inventory;
using Shuvi.Interfaces.User;

namespace Shuvi.Interfaces.Combat
{
    public interface ICombatEnemy : ICombatEntity
    {
        public int RatingGet { get; }
        public string PictureUrl { get; }

        public IDropInventory GetDrop(IDatabaseUser user);
    }
}
