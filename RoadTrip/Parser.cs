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
            if (input == "HELP")
            {
                Console.WriteLine("Type the name of an item to learn how you can interact with an item.");
                Console.WriteLine("Type the name of an action and an item to interact with that item.");
                Console.WriteLine("For example, type \"NAP BED\" to use the take a nap in the bed.");
                Console.WriteLine("Type the name of an exit to travel to a new location using that exit.");
                return;
            }

            string[] locationItemNames = location.GetItemNames();
            foreach (string itemName in locationItemNames)
            {
                if (input.Contains(itemName))
                {
                    if (itemName == input)
                    {
                        string[] itemActionDescriptions = location.GetItemActionNames(itemName, true);
                        if (itemActionDescriptions.Length > 0)
                        {
                            Console.WriteLine("Here is what you can do with " + itemName + ":\r\n" + String.Join("\r\n", itemActionDescriptions));
                            return;
                        }
                    }
                    else
                    {
                        string[] itemActionNames = location.GetItemActionNames(itemName, false);
                        foreach (string itemActionName in itemActionNames)
                        {
                            if (input.Contains(itemActionName))
                            {
                                location.DoItemAction(itemName, itemActionName);
                                return;
                            }
                        }
                    }
                }
            }

            throw new Exception("Failed to parse input " + input);
        }
    }
}
