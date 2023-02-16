using Shuvi.Classes.Data.Item;
using Shuvi.Classes.Types.Item;
using Shuvi.Enums.Item;
using Shuvi.Interfaces.Items;
using System.ComponentModel;

namespace Shuvi.Classes.Factories.Item
{
    public static class ItemFactory
    {
        public static IItem CreateItem(ItemData itemDocument)
        {
            return itemDocument.Type switch
            {
                
            };
        }
    }
}
