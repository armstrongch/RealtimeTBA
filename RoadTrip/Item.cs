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

        public string[] GetItemActionNames(ACTION_TYPE type, bool includeDescriptions)
        {
            List<string> itemActionNames = new List<string>();
            foreach (ItemAction action in ItemActions.Where(a => a.Type == type).ToList())
            {
                string itemActionName = action.Name;
                if (includeDescriptions)
                {
                    itemActionName += " (" + action.Description + ")";
                }
                itemActionNames.Add(itemActionName);
            }

            return itemActionNames.ToArray();
        }
    }
}
