﻿using MongoDB.Bson;
using Shuvi.Enums.Requirements;

namespace Shuvi.Classes.Data.Item
{
    public class CraftData
    {
        public bool Hidden { get; set; } = false;
        public Dictionary<BaseRequirement, int> Needs { get; set; } = new();
        public Dictionary<ObjectId, int> Items { get; set; } = new();
    }
}