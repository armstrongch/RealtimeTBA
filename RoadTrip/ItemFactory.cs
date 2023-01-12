using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadTrip
{   
    public class ItemFactory
    {
        public const string BedItemName = "BED";
        
        public static Item GenerateItem(string name)
        {
            switch (name)
            {
                case BedItemName:
                    return new Item(BedItemName, new List<ItemAction>() {
                        new ItemAction("sleep", "Sleep in the bed.", ACTION_TYPE.WORLD, sleep),
                        new ItemAction("nap", "Nap in the bed.", ACTION_TYPE.WORLD, nap)
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
    }
}
