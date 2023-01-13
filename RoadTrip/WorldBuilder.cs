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
            Item bed = ItemFactory.GenerateItem(ITEMS.BED);
            
            Location apartment = new Location(
                "APARTMENT", "The place where you live.",
                new List<Item>() { bed },
                new Dictionary<string, string>()
                {
                    { "APARTMENT BATHROOM", "APARTMENT BATHROOM" }
                });

            Location apartmentBathroom = new Location(
                "APARTMENT BATHROOM", "An adjoining closet containing a shower, toilet, and sink.",
                new List<Item>(),
                new Dictionary<string, string>()
                {
                    { "APARTMENT", "APARTMENT" }
                });

            Locations.Add(apartment);
            Locations.Add(apartmentBathroom);

            return apartment;
        }

        private void SetupPlayerInventory()
        {
            Player.PickUpItem(ItemFactory.GenerateItem(ITEMS.WALLET));
        }
    }
}