using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadTrip
{   
    public class Item
    {
        public string Name { get; private set; }
        private List<ItemAction> ItemActions;
        
        public Item(string name, List<ItemAction> ItemActions)
        {
            this.Name = name.ToUpper();
            this.ItemActions = ItemActions;
        }

        public string[] GetItemActionNames()
        {
            List<string> itemActionNames = new List<string>();
            for (int i = 0; i < ItemActions.Count; i += 1)
            {
                itemActionNames.Add(ItemActions[i].Name + " (" + ItemActions[i].Description + ")");
            }

            return itemActionNames.ToArray();
        }
    }
}
