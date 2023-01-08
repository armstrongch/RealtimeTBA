﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;

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
            while (playerName == string.Empty)
            {
                string input = GetInput();
                if (playerName != string.Empty)
                {
                    GameFilePath = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        gameTitle + "_" + Player.Name + ".sav");

                    if (File.Exists(GameFilePath))
                    {
                        input = string.Empty;
                        while (input != "YES" && input != "NO")
                        {
                            Console.WriteLine("A saved game for a player named " + playerName + " already exists.)";
                            Console.WriteLine("Do you want to overwrite this? (Yes/No)");
                            input = GetInput();
                        }
                        if (input == "NO")
                        {
                            playerName = string.Empty;
                        }
                        else
                        {
                            throw new NotImplementedException("To-Do: Finish implementing this!");
                        }
                    }
                }
            }
            Console.WriteLine("Welcome, " + playerName);
            return playerName;
        }
    
        private void SaveGame()
        {
            var fileName 


        }
    }
}
