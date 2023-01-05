using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadTrip
{   
    internal class Item
    {
        public string Name { get; private set; }
        private List<ItemAction> ItemActions;
        
        public Item(string name, List<ItemAction> ItemActions)
        {
            this.Name = name.ToUpper();
            this.ItemActions = ItemActions;
        }
    }
}
