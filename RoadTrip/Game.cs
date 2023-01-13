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

        //New Game
        public Game(string gameTitle)
        {
            Console.WriteLine("Loading...");

            Location startingLocation = BuildWorld();
            
            Player = new Player(startingLocation, GetPlayerName(gameTitle));
            SaveGame();

            StartGame();
        }

        //Load Game
        public Game(string gameTitle, string saveGameLocation)
        {
            Console.WriteLine("Loading...");

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(saveGameLocation);

            Locations = LoadLocationList(xmlDoc);
            Player = LoadPlayer(xmlDoc);

            GameFilePath = saveGameLocation;

            StartGame();
        }

        private void StartGame()
        {
            ProcessInput(string.Empty);

            GameTime = 0;
            Timer = new PeriodicTimer(MillisecondsPerFrame);
            StartTimer();

            while (!Paused)
            {
                string input = GetInput();
                ProcessInput(input);
                SaveGame();
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
                    parser.ParseInput(input, Player, Locations);
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
            return playerName;
        }
    
        private void SaveGame()
        {
            XmlDocument xmlDoc = new XmlDocument();
            //<savedata>
            XmlNode rootNode = xmlDoc.CreateElement("savedata");
            xmlDoc.AppendChild(rootNode);

            //<player>
            XmlNode playerNode = xmlDoc.CreateElement("player");
            
            XmlAttribute playerName = xmlDoc.CreateAttribute("name");
            playerName.Value = Player.Name;
            playerNode.Attributes.Append(playerName);

            //<currentLocation>
            XmlNode currentLocation = xmlDoc.CreateElement("currentLocation");
            currentLocation.InnerText = Player.CurrentLocation.Name;
            playerNode.AppendChild(currentLocation);

            //<attributeList>
            XmlNode attributeList = xmlDoc.CreateElement("attributeList");
            for (int i = 0; i < Player.Attributes.Count; i += 1)
            {
                XmlNode attribute = xmlDoc.CreateElement("attribute");
                
                XmlAttribute attributeType = xmlDoc.CreateAttribute("type");
                ATTRIBUTES type = Player.Attributes.Keys.ElementAt(i);
                attributeType.Value = type.ToString();
                attribute.Attributes.Append(attributeType);
                
                XmlAttribute attributeValue = xmlDoc.CreateAttribute("value");
                attributeValue.Value = Player.Attributes[type].ToString();
                attribute.Attributes.Append(attributeValue);

                attributeList.AppendChild(attribute);
            }
            playerNode.AppendChild(attributeList);

            rootNode.AppendChild(playerNode);

            //<locationsList>
            XmlNode locationList = xmlDoc.CreateElement("locationList");
            rootNode.AppendChild(locationList);

            foreach (Location location in Locations)
            {
                //<location locationName="Name">
                XmlNode locationNode = xmlDoc.CreateElement("location");
                
                XmlAttribute locationName = xmlDoc.CreateAttribute("locationName");
                locationName.Value = location.Name;
                locationNode.Attributes.Append(locationName);

                //<description>
                XmlNode description = xmlDoc.CreateElement("description");
                description.InnerText = location.Description;
                locationNode.AppendChild(description);

                //<itemList>
                XmlNode itemList = xmlDoc.CreateElement("itemList");
                foreach(string itemName in location.GetItemNames())
                {
                    //<item>Item Name</item>
                    XmlNode item = xmlDoc.CreateElement("item");
                    item.InnerText = itemName;
                    itemList.AppendChild(item);
                }
                locationNode.AppendChild(itemList);

                //<exitList>
                XmlNode exitList = xmlDoc.CreateElement("exitList");
                foreach (string name in location.GetExitNames())
                {
                    //<exit exitName="Name">Location Name</exitName>
                    XmlNode exit = xmlDoc.CreateElement("exit");
                    XmlAttribute exitName = xmlDoc.CreateAttribute("exitName");
                    exitName.Value = name;
                    exit.Attributes.Append(exitName);
                    exit.InnerText = location.GetLocationNameFromExitName(name);
                    exitList.AppendChild(exit);
                }
                locationNode.AppendChild(exitList);

                locationList.AppendChild(locationNode);
            }

            xmlDoc.Save(GameFilePath);

        }

        private List<Location> LoadLocationList(XmlDocument xmlDoc)
        {
            List<Location> locationList = new List<Location>();

            XmlNode locationNodeList = xmlDoc.SelectSingleNode("savedata").SelectSingleNode("locationList");

            foreach (XmlNode locationNode in locationNodeList.ChildNodes)
            {
                string locationName = locationNode.Attributes.GetNamedItem("locationName").Value;
                string locationDesc = locationNode.SelectSingleNode("description").InnerText;
                
                List<Item> itemList = new List<Item>();
                foreach (XmlNode itemNode in locationNode.SelectSingleNode("itemList").ChildNodes)
                {
                    itemList.Add(ItemFactory.GenerateItem(itemNode.InnerText));
                }
                
                Dictionary<string, string> exitList = new Dictionary<string, string>();
                foreach (XmlNode exitNode in locationNode.SelectSingleNode("exitList").ChildNodes)
                {
                    exitList.Add(exitNode.Attributes.GetNamedItem("exitName").Value, exitNode.InnerText);
                }


                Location location = new Location(locationName, locationDesc, itemList, exitList);
                locationList.Add(location);
            }
            return locationList;
        }

        Player LoadPlayer(XmlDocument xmlDoc)
        {
            XmlNode playerNode = xmlDoc.SelectSingleNode("savedata").SelectSingleNode("player");
            string playerName = playerNode.Attributes.GetNamedItem("name").Value;
            string currentLocationName = playerNode.SelectSingleNode("currentLocation").InnerText;
            Location currentLocation = Locations.Where(x => x.Name == currentLocationName).FirstOrDefault();
            return new Player(currentLocation, playerName);
        }
    }
}
