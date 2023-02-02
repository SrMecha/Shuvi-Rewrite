using MongoDB.Bson;

namespace Shuvi.Classes.Data.Drop
{
    public sealed class EveryDropData
    {
        public ObjectId Id { get; set; } = ObjectId.Empty;
        public float Chance { get; set; } = 0f;
        public int Max { get; set; } = 0;
    }
}
