using MongoDB.Bson;
using Shuvi.Enums.Pet;

namespace Shuvi.Classes.Data.User
{
    public class UserPetData
    {
        public ObjectId Id { get; set; } = ObjectId.Empty;
        public PetType Type { get; set; } = PetType.Simple;
    }
}
