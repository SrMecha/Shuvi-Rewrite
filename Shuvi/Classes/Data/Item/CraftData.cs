using MongoDB.Bson;
using Shuvi.Classes.Data.Requirements;
using Shuvi.Enums.Requirements;

namespace Shuvi.Classes.Data.Item
{
    public class CraftData
    {
        public ObjectId CraftedItemId { get; set; } = ObjectId.Empty;
        public RequirementsData Needs { get; set; } = new();
        public Dictionary<ObjectId, int> Items { get; set; } = new();
    }
}
