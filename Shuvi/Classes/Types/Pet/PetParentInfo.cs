using MongoDB.Bson;
using Shuvi.Interfaces.Enemy;
using Shuvi.Interfaces.Pet;
using Shuvi.Services.StaticServices.Database;

namespace Shuvi.Classes.Types.Pet
{
    public class PetParentInfo : IPetParentInfo
    {
        public ObjectId? ParentId { get; private set; }
        public bool HaveParent => ParentId is not null;

        public PetParentInfo(ObjectId? parentId)
        {
            ParentId = parentId;
        }
        public IDatabaseEnemy? GetParent()
        {
            if (!HaveParent)
                return null;
            return EnemyDatabase.GetEnemy((ObjectId)ParentId!);
        }
    }
}
