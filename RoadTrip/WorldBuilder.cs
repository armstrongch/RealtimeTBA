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
            static string nap(string itemName)
            {
                return $"You take a quick nap in the {itemName}.";
            }
            static string sleep(string itemName)
            {
                return $"You take a long snooze in the {itemName}.";
            }

            Item bed = new Item("BED", new List<ItemAction>() {
                    new ItemAction("sleep", "Sleep in the bed.", ACTION_TYPE.WORLD, sleep),
                    new ItemAction("nap", "Nap in the bed.", ACTION_TYPE.WORLD, nap)
                });
            
            Location apartment = new Location(
                "Apartment", "The place where you live.",
                new List<Item>() { bed },
                new Dictionary<string, Location>());

            Locations.Add(apartment);

            Player = new Player(apartment);
        }
    }
}