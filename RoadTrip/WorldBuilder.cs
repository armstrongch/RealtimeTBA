using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Timers;

namespace RoadTrip
{
    public partial class Game
    {
        //Returns the player's starting location.
        private Location BuildWorld()
        {
            Item bed = ItemFactory.GenerateItem("BED");
            
            Location apartment = new Location(
                "Apartment", "The place where you live.",
                new List<Item>() { bed },
                new Dictionary<string, Location>());

            Locations.Add(apartment);

            return apartment;
        }
    }
}