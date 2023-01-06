using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadTrip
{
    public class Location
    {
        private List<Item> Items = new List<Item>();
        private Dictionary<string, Location> Exits = new Dictionary<string, Location>();

        public string Name { get; private set; }
        public string Description { get; private set; }
        
        public Location(string name, string description, List<Item> items, Dictionary<string, Location> exits)
        {
            Name = name;
            Description = description;
            Items = items;
            Exits = exits;
        }

        public string[] GetItemNames()
        {
            List<string> itemList = new List<string>();
            Items = Items.OrderBy(i => i.Name).ToList();
            string previousName = string.Empty;
            int previousNameCount = 1;
            
            foreach (Item i in Items)
            {
                if (i.Name == previousName)
                {
                    previousNameCount += 1;
                    itemList.Add(i.Name + " " + previousNameCount.ToString());
                }
                else
                {
                    previousName = i.Name;
                    previousNameCount = 1;
                    itemList.Add(i.Name);
                }
            }

            return itemList.ToArray();
        }

        public string[] GetExitNames()
        {
            List<string> exitNames = new List<string>();
            for (int i = 0; i < Exits.Count; i += 1)
            {
                exitNames.Add(Exits.Keys.ElementAt(i));
            }

            return exitNames.ToArray();
        }

        public string[] GetItemActionNames(string itemName)
        {
            //If itemName matches the name of an item at this location, return that items actions.
            //If itemName does not match an item at this location, return an empty array of strings.
            Item? item = Items.FirstOrDefault(x => x.Name.ToUpper() == itemName);
            return item != null ? item.GetItemActionNames() : Array.Empty<string>();
        }
    }
}
