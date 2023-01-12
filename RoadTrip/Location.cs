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
        private Dictionary<string, string> Exits = new Dictionary<string, string>();

        public string Name { get; private set; }
        public string Description { get; private set; }
        
        public Location(string name, string description, List<Item> items, Dictionary<string, string> exits)
        {
            Name = name;
            Description = description;
            Items = items;
            for (int i = 0; i < exits.Count; i++)
            {
                string exitName = exits.Keys.ElementAt(i);
                string locationName = exits[exitName];
                AddExit(exitName, locationName);
            }
        }

        public void AddExit(string exitName, string locationName)
        {
            Exits.Add(exitName.ToUpper(), locationName.ToUpper());
        }

        public string[] GetItemNames()
        {
            List<string> itemList = new List<string>();
            Items = Items.OrderBy(i => i.Name).ToList();
            
            foreach (Item i in Items)
            {
                itemList.Add(i.Name);
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

        public string[] GetItemActionNames(string itemName, bool includeDescriptions)
        {
            Item item = Items.First(x => x.Name.ToUpper() == itemName);
            return item.GetItemActionNames(ACTION_TYPE.WORLD, includeDescriptions);
        }

        public void DoItemAction(string itemName, string itemActionName)
        {
            Item item = Items.First(i => i.Name == itemName);
            item.DoItemAction(itemActionName);
        }

        public string GetLocationNameFromExitName(string exitName)
        {
            return Exits[exitName];
        }
    }
}
