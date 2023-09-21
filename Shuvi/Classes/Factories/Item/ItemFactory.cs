using Shuvi.Classes.Data.Item;
using Shuvi.Classes.Types.Item;
using Shuvi.Enums.Item;
using Shuvi.Interfaces.Items;

namespace Shuvi.Classes.Factories.Item
{
    public static class ItemFactory
    {
        public static IItem CreateItem(ItemData itemDocument)
        {
            return itemDocument.Type switch
            {
                ItemType.Simple => new SimpleItem(itemDocument),
                ItemType.Weapon => new EquipmentItem(itemDocument),
                ItemType.Helmet => new EquipmentItem(itemDocument),
                ItemType.Armor => new EquipmentItem(itemDocument),
                ItemType.Leggings => new EquipmentItem(itemDocument),
                ItemType.Boots => new EquipmentItem(itemDocument),
                ItemType.Potion => new PotionItem(itemDocument),
                ItemType.Chest => new ChestItem(itemDocument),
                ItemType.Amulet => new EquipmentItem(itemDocument),
                ItemType.Recipe => new RecipeItem(itemDocument),
                _ => new SimpleItem(itemDocument),
            };
        }
    }
}
