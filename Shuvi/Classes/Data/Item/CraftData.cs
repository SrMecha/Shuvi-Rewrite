using MongoDB.Bson;
using Shuvi.Enums.Requirements;

namespace Shuvi.Classes.Data.Item
{
    public class CraftData
    {
        public ObjectId CraftedItemId { get; set; } = ObjectId.Empty;
        public Dictionary<BaseRequirement, int> Needs { get; set; } = new();
        public Dictionary<ObjectId, int> Items { get; set; } = new();
    }
}
