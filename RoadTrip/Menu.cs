using System;
using System.Threading;
using System.Timers;

namespace RoadTrip
{
    public class Menu
    {
        public const string GameTitle = "UNIQUE GAME NAME";

        public Menu()
        {
            Console.WriteLine("Welcome to " + GameTitle + ".");
            ListOptions();
        }

        public void ListOptions()
        {
            Console.WriteLine("Enter \"NEW\" to start a new game.");
            Console.WriteLine("Enter \"LOAD\" to load a previously saved game.");
            Console.WriteLine("Enter \"QUIT\" to quit to desktop.");
            string? readline = Console.ReadLine();
            
            
            switch (readline == null ? "" : readline.ToUpper())
            {
                case "NEW":
                    Game game = new Game(GameTitle);
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
