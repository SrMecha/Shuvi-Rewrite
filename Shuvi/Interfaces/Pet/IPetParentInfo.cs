using MongoDB.Bson;
using Shuvi.Interfaces.Enemy;

namespace Shuvi.Interfaces.Pet
{
    public interface IPetParentInfo
    {
        public ObjectId? ParentId { get; }
        public bool HaveParent { get; }

        public IDatabaseEnemy GetParent();
    }
}
