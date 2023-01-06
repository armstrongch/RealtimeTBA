using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadTrip
{
    public class Parser
    {
        public Parser()
        {
            
        }

        public void ParseInput(string input, Location location)
        {
            bool success = false;
            
            //Show more info about a requested item
            string[] locationItemNames = location.GetItemNames();
            foreach (string itemName in locationItemNames)
            {
                if ((itemName == input) && (!success))
                {
                    string[] itemActionNames = location.GetItemActionNames(itemName);
                    if (itemActionNames.Length > 0)
                    {
                        Console.WriteLine("Here is what you can do with " + itemName + ":\r\n" + String.Join("\r\n", itemActionNames));
                        success = true;
                    }    
                }
            }
        }
    }
}
