using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace RoadTrip
{
    public enum SKILLS
    {
        CHARISMA,
        BRAVERY,
        STRENGTH,
        INTELLIGENCE
    }

    public class Player
    {
        public Location CurrentLocation { get; private set; }
        public string Name { get; private set; }
        public List<Item> Inventory { get; private set; }

        public Dictionary<SKILLS, int> SkillValues { get; private set; }

        public Player(Location startingLocation, string name)
        {
            CurrentLocation = startingLocation;
            Name = name;
            SkillValues = new Dictionary<SKILLS, int>()
            {
                { SKILLS.CHARISMA, 0 },
                { SKILLS.BRAVERY, 0 },
                { SKILLS.STRENGTH, 0 },
                { SKILLS.INTELLIGENCE, 0 }
            };
            Inventory = new List<Item>();
        }

        public void TravelToLocation(Location newLocation)
        {
            Console.WriteLine("Travelling to: " + newLocation.Name);
            CurrentLocation = newLocation;
        }

        public void UpdateSkillValue(string attributeName, int value)
        {
            SKILLS skill = SKILLS.CHARISMA;
            switch(attributeName.ToUpper())
            {
                case "BRAVERY": skill = SKILLS.BRAVERY; break;
                case "STRENGTH": skill = SKILLS.STRENGTH; break;
                case "INTELLIGENCE": skill = SKILLS.INTELLIGENCE; break;
                case "CHARISMA": skill = SKILLS.CHARISMA; break;
                default: throw new NotImplementedException();
            }
            UpdateSkillValue(skill, value);
        }

        public void UpdateSkillValue(SKILLS skill, int value)
        {
            SkillValues[skill] = value;
        }

        public void PickUpItem(Item item)
        {
            Inventory.Add(item);
        }

        public string[] GetItemNames()
        {
            List<string> itemList = new List<string>();
            Inventory = Inventory.OrderBy(i => i.Name).ToList();

            foreach (Item i in Inventory)
            {
                itemList.Add(i.Name);
            }

            return itemList.ToArray();
        }

        public string[] GetItemActionNames(string itemName, bool includeDescriptions)
        {
            Item item = Inventory.First(x => x.Name.ToUpper() == itemName);
            return item.GetItemActionNames(ACTION_TYPE.INVENTORY, includeDescriptions);
        }

        public void DoItemAction(string itemName, string itemActionName)
        {
            Item item = Inventory.First(i => i.Name == itemName);
            item.DoItemAction(itemActionName);
        }
    }
}
