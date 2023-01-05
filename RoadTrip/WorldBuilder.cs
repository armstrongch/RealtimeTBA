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
        private void BuildWorld()
        {
            static string sleep_on(string itemName)
            {
                return $"You sleep on the {itemName}.";
            }
            
            Item bed = new Item("BED", new List<ItemAction>() {
                    new ItemAction("sleep", ACTION_TYPE.WORLD, sleep_on)
                });
            
            Location apartment = new Location(
                "Apartment", "The place where you live.",
                new List<Item>() { bed },
                new Dictionary<string, Location>());

            Locations.Add(apartment);
        }
    }
}