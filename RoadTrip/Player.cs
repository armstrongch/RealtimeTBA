using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace RoadTrip
{
    public class Player
    {
        public Location CurrentLocation { get; private set; }
        public string Name { get; private set; }

        public Player(Location startingLocation, string name)
        {
            CurrentLocation = startingLocation;
            Name = name;
        }

        public void TravelToLocation(Location newLocation)
        {
            Console.WriteLine("Travelling to: " + newLocation.Name);
            CurrentLocation = newLocation;
        }
    }
}
