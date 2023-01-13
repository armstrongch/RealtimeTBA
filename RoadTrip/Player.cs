using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace RoadTrip
{
    public enum ATTRIBUTES
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

        public Dictionary<ATTRIBUTES, int> Attributes { get; private set; }

        public Player(Location startingLocation, string name)
        {
            CurrentLocation = startingLocation;
            Name = name;
            Attributes = new Dictionary<ATTRIBUTES, int>()
            {
                { ATTRIBUTES.CHARISMA, 0 },
                { ATTRIBUTES.BRAVERY, 0 },
                { ATTRIBUTES.STRENGTH, 0 },
                {  ATTRIBUTES.INTELLIGENCE, 0 }
            };
            
            throw new NotImplementedException("Add player attributes to loading!");
            throw new NotImplementedException("Add player inventory to saving and loading!");
            throw new NotImplementedException("Add player inventory action selection!");
        }

        public void TravelToLocation(Location newLocation)
        {
            Console.WriteLine("Travelling to: " + newLocation.Name);
            CurrentLocation = newLocation;
        }
    }
}
