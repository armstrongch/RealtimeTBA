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

        //returns true if input was QUIT, otherwise returns false
        public bool ParseInput(string input, Player player, List<Location> locations)
        {
            if (input == "HELP")
            {
                Console.WriteLine("Type the name of an item to learn how you can interact with an item.");
                Console.WriteLine("Type the name of an action and an item to interact with that item.");
                Console.WriteLine("For example, type \"NAP BED\" to use the take a nap in the bed.");
                Console.WriteLine("Type the name of an exit to travel to a new location using that exit.");
                return false;
            }
            else if (input == "QUIT")
            {
                return true;
            }

            Location playerLocation = player.CurrentLocation;

            string[] locationItemNames = playerLocation.GetItemNames();
            string[] inventoryItemNames = player.GetItemNames();

            string[] availableItemNames = locationItemNames.Concat(inventoryItemNames).ToArray();

            //For each item in the world and in our pockets
            foreach (string itemName in availableItemNames)
            {
                //If input references the item
                if (input.Contains(itemName))
                {
                    //If item is in the world, get the world type actions.
                    string[] actionDescriptions = { };
                    string[] locationActionNames = { };
                    string[] inventoryActionNames = { };
                    if (locationItemNames.Contains(itemName))
                    {
                        actionDescriptions = actionDescriptions.Concat(playerLocation.GetItemActionNames(itemName, true)).ToArray();
                        locationActionNames = playerLocation.GetItemActionNames(itemName, false);
                    }
                    //If items in in our pockets, get the inventory type actions
                    if (inventoryItemNames.Contains(itemName))
                    {
                        actionDescriptions = actionDescriptions.Concat(player.GetItemActionNames(itemName, true)).ToArray();
                        inventoryActionNames = player.GetItemActionNames(itemName, false);
                    }
                    //Note: It's possible to show both. I.e. There is a rock on the ground and a rock in our pockets.

                    //If player input was itemName
                    if (itemName == input)
                    {
                        if (actionDescriptions.Length > 0)
                        {
                            Console.WriteLine("Here is what you can do with " + itemName + ":\r\n" + String.Join("\r\n", actionDescriptions));
                            return false;
                        }
                        else
                        {
                            Console.WriteLine("There is nothing you can do with " + itemName + ".");
                            return false;
                        }
                    }
                    else
                    {
                        foreach (string actionName in locationActionNames)
                        {
                            if (input.Contains(actionName))
                            {
                                playerLocation.DoItemAction(itemName, actionName);
                                return false;
                            }
                        }
                        
                        foreach (string actionName in inventoryActionNames)
                        {
                            if (input.Contains(actionName))
                            {
                                player.DoItemAction(itemName, actionName);
                                return false;
                            }
                        }
                    }
                }
            }

            string[] exitNames = playerLocation.GetExitNames();
            if (exitNames.Contains(input))
            {
                string targetLocationName = playerLocation.GetLocationNameFromExitName(input);
                Location targetLocation = locations.Where(x => x.Name == targetLocationName).FirstOrDefault();
                player.TravelToLocation(targetLocation);
                return false;
            }
            throw new Exception("Failed to parse input " + input);
        }
    }
}
