using System;
using System.IO;
using System.Linq;
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
            
            switch (readline == null ? string.Empty : readline.ToUpper())
            {
                case "NEW":
                    Game game = new Game(GameTitle);
                    break;
                case "LOAD":
                    LoadGame();
                    break;
                case "QUIT":
                    Environment.Exit(0);
                    break;
                default:
                    ListOptions();
                    break;
            }
        }

        public void LoadGame()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), GameTitle);
            string[] saveNames = Directory.GetFiles(path);

            if (saveNames.Length > 0)
            {
                Console.WriteLine("Which saved game would you like to load?");
                foreach (string saveName in saveNames)
                {
                    string fileName = Path.GetFileNameWithoutExtension(saveName);
                    Console.WriteLine(fileName);
                }

                string? input = Console.ReadLine();
                string formattedInput = input == null ? string.Empty : input.ToUpper();
                foreach (string saveName in saveNames)
                {
                    if (Path.GetFileNameWithoutExtension(saveName).Equals(formattedInput))
                    {
                        Game game = new Game(GameTitle, saveName);
                        break;
                    }
                }
                Console.WriteLine("Could not find a saved game called: " + formattedInput);
                ListOptions();
            }
            else
            {
                Console.WriteLine("Could not find any saved games.");
                ListOptions();
            }
        }
    }
}
