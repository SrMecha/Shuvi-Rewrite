using MongoDB.Bson;
using Shuvi.Interfaces.Pet;

namespace Shuvi.Classes.Types.Pet
{
    public class UserPetInfo : IUserPetInfo
    {
        public ObjectId? Id { get; private set; } = ObjectId.Empty;
        public bool HavePet { 
            get { return Id is not null; } 
        }

        public async Task<IDatabasePet?> GetPet()
        {
            throw new NotImplementedException();
        }
    }
}
