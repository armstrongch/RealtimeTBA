using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using System.Xml;

namespace RoadTrip
{
    public partial class Game
    {
        public int GameTime { get; private set; }
        private PeriodicTimer Timer;
        private static TimeSpan MillisecondsPerFrame = TimeSpan.FromSeconds(1);
        private Parser parser = new Parser();
        private bool Paused = false;

        private List<Event> EventQueue = new List<Event>();
        private List<Location> Locations = new List<Location>();

        Player Player;
        string GameFilePath;

        public Game(string gameTitle)
        {
            Console.WriteLine("Loading...");

            Location startingLocation = BuildWorld();
            
            Player = new Player(startingLocation, GetPlayerName(gameTitle));

            ProcessInput(string.Empty);

            GameTime = 0;
            Timer = new PeriodicTimer(MillisecondsPerFrame);
            StartTimer();

            while (!Paused)
            {
                string input = GetInput();
                ProcessInput(input);
            }
        }

        // When Console.Readline returns NULL, this returns an empty string.
        // Otherwise, this returns the user input with Trim and ToUpper.
        private string GetInput()
        {
            string? readline = Console.ReadLine();
            string input = readline == null ? string.Empty : readline.Trim().ToUpper();
            return input;
        }

        private void ProcessInput(string input)
        {
            Console.WriteLine("*************************************************");

            if (input != string.Empty)
            {
                try
                {
                    parser.ParseInput(input, Player.CurrentLocation);
                }
                catch
                {
                    Console.WriteLine("\"" + input + "\" is not valid input!");
                }
            }

            ProcessScheduledEvents();

            PrintWorldStatus();

            Console.WriteLine("Type HELP for help.");
        }

        private async void StartTimer()
        {
            while (await Timer.WaitForNextTickAsync())
            {
                GameTime++;
            }
        }

        private void ProcessScheduledEvents()
        {
            if (EventQueue.Count > 0)
            {
                Event nextEvent = EventQueue.OrderBy(e => e.ScheduledTime).First();
                if (GameTime >= nextEvent.ScheduledTime)
                {
                    Console.WriteLine(nextEvent.Action());
                    EventQueue.Remove(nextEvent);
                    ProcessScheduledEvents();
                }
            }
        }

        private void PrintWorldStatus()
        {
            Location loc = Player.CurrentLocation;
            Console.WriteLine("Current Location: " + loc.Name.ToUpper() + " (" + loc.Description + ")");
            Console.WriteLine("Nearby Items: " + String.Join(", ", loc.GetItemNames()));
            Console.WriteLine("Nearby Exits: " + String.Join(", ", loc.GetExitNames()));
        }

        private string GetPlayerName(string gameTitle)
        {
            Console.WriteLine("What is your name?");
            string playerName = string.Empty;
            string saveGameDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), gameTitle);

            while (playerName == string.Empty)
            {
                playerName = GetInput();
                if (playerName != string.Empty)
                {
                    GameFilePath = Path.Combine(saveGameDirectory, playerName + ".sav");

                    if (File.Exists(GameFilePath))
                    {
                        string yn_input = string.Empty;
                        while (yn_input != "YES" && yn_input != "NO")
                        {
                            Console.WriteLine("A saved game for a player named " + playerName + " already exists.");
                            Console.WriteLine("Do you want to overwrite this? (Yes/No)");
                            yn_input = GetInput();
                        }
                        if (yn_input == "NO")
                        {
                            playerName = string.Empty;
                            Console.WriteLine("What is your name?");
                        }
                    }
                }
            }
            if (!File.Exists(GameFilePath))
            {
                if (!Directory.Exists(saveGameDirectory))
                {
                    Directory.CreateDirectory(saveGameDirectory);
                }
                File.Create(GameFilePath);
            }
            Console.WriteLine("Welcome, " + playerName);
            SaveGame();
            return playerName;
        }
    
        private void SaveGame()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode locationsRootNode = xmlDoc.CreateElement("locationList");
            xmlDoc.AppendChild(locationsRootNode);

            /*
            <locationList>
                <location name="Location Name">
                    <description>Location Description</description>
                    <itemList>
                        <item>Item Name</item>
                        <item>Item Name</item>
                    </itemList>
                </location>
                <location name="Location Name">
                    <description>Location Description</description>
                    <itemList>
                        <item>Item Name</item>
                        <item>Item Name</item>
                    </itemList>
                </location>
            </locationList>
            */

            foreach (Location location in Locations)
            {
                XmlNode locationNode = xmlDoc.CreateElement("location");
                
                XmlAttribute locationName = xmlDoc.CreateAttribute("name");
                locationName.Value = location.Name;
                locationNode.Attributes.Append(locationName);

                XmlNode description = xmlDoc.CreateElement("description");
                description.InnerText = location.Description;
                locationNode.AppendChild(description);

                XmlNode itemList = xmlDoc.CreateElement("itemList");
                foreach(string itemName in location.GetItemNames())
                {
                    XmlNode item = xmlDoc.CreateElement("item");
                    item.InnerText = itemName;
                    itemList.AppendChild(item);
                }
                locationNode.AppendChild(itemList);

                locationsRootNode.AppendChild(locationNode);
            }

            xmlDoc.Save(GameFilePath);

        }
    }
}
