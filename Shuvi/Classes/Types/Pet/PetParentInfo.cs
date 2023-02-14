using MongoDB.Bson;
using Shuvi.Interfaces.Enemy;
using Shuvi.Interfaces.Pet;

namespace Shuvi.Classes.Types.Pet
{
    public class PetParentInfo : IPetParentInfo
    {
        public ObjectId? ParentId { get; private set; }
        public bool HaveParent
        {
            get { return ParentId is not null; }
        }

        public IDatabaseEnemy GetParent()
        {
            throw new NotImplementedException();
        }
    }
}
