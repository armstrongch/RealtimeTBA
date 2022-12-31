using System;
using System.Threading;
using System.Timers;

namespace RoadTrip
{
    public class Menu
    {
        public Menu()
        {
            Console.WriteLine("Welcome to [Insert Game Name Here].");
            ListOptions();
        }

        public void ListOptions()
        {
            Console.WriteLine("Enter \"NEW\" to start a new game.");
            Console.WriteLine("Enter \"LOAD\" to load a previously saved game.");
            Console.WriteLine("Enter \"QUIT\" to quit to desktop.");
            string input = Console.ReadLine().ToUpper();
            switch (input)
            {
                case "NEW":
                    Game game = new Game();
                    break;
                case "LOAD":
                    Console.WriteLine("TO-DO: Implement Saving/Loading!");
                    ListOptions();
                    break;
                case "QUIT":
                    break;
                default:
                    ListOptions();
                    break;
            }
        }
    }
}
