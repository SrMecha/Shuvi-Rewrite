using MongoDB.Bson;
using Shuvi.Enums.Localization;
using Shuvi.Services.StaticServices.Database;
using System.Runtime.CompilerServices;

namespace Shuvi.Classes.Extensions
{
    public static class DictionaryExt
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static string GetItemsNeededInfo(this Dictionary<ObjectId, int> items, Dictionary<ObjectId, int> otherItems, Language lang)
        {
            var result = new List<string>();
            foreach (var (itemId, amount) in items)
                result.Add($"{ItemDatabase.GetItem(itemId).Info.GetName(lang)} {otherItems.GetValueOrDefault(itemId, 0)}/{amount}");
            return string.Join("\n", result);
        }
    }
}
