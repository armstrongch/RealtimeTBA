using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadTrip
{   
    public enum ITEMS
    {
        BED,
        WALLET
    }
    
    public class ItemFactory
    {

        private static ITEMS stringToItemsEnum(string itemName)
        {
            return (ITEMS)Enum.Parse(typeof(ITEMS), itemName);
        }
        
        public static Item GenerateItem(string itemName)
        {
            ITEMS itemFromName = stringToItemsEnum(itemName);
            return GenerateItem(itemFromName);
        }
        
        public static Item GenerateItem(ITEMS item)
        {
            switch (item)
            {
                case ITEMS.BED:
                    return new Item(item.ToString(), new List<ItemAction>() {
                        new ItemAction("sleep", "Sleep in the bed.", ACTION_TYPE.WORLD, sleep),
                        new ItemAction("nap", "Nap in the bed.", ACTION_TYPE.WORLD, nap)
                    });
                case ITEMS.WALLET:
                    return new Item(item.ToString(), new List<ItemAction>() {
                        new ItemAction("examine", "Look in your wallet.", ACTION_TYPE.INVENTORY, examine)
                    });
                default:
                    throw new NotImplementedException();
            }
        }

        static string nap(string itemName)
        {
            return $"You take a quick nap in the {itemName}.";
        }
        static string sleep(string itemName)
        {
            return $"You take a long snooze in the {itemName}.";
        }

        static string examine(string itemName)
        {
            ITEMS item = stringToItemsEnum(itemName);

            switch (item)
            {
                case ITEMS.WALLET:
                    return $"You have $4 in your wallet.";
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
