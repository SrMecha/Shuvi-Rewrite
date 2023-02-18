using MongoDB.Bson;
using Shuvi.Interfaces.Pet;
using Shuvi.Services.StaticServices.Database;

namespace Shuvi.Classes.Types.Pet
{
    public class UserPetInfo : IUserPetInfo
    {
        public ObjectId? Id { get; private set; } = ObjectId.Empty;
        public bool HavePet => Id is not null;

        public UserPetInfo(ObjectId? id)
        {
            Id = id;
        }
        public async Task<IDatabasePet?> GetPet()
        {
            return HavePet ? await PetDatabase.GetPet((ObjectId)Id!) : null;
        }
    }
}
