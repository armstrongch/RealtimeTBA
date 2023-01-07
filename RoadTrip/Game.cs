using System;
using System.Collections.Generic;
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

        public Game()
        {
            BuildWorld();
            ProcessInput(string.Empty);

            GameTime = 0;
            Timer = new PeriodicTimer(MillisecondsPerFrame);
            StartTimer();

            while (!Paused)
            {
                GetInput();
            }
        }

        private void GetInput()
        {
            string? readline = Console.ReadLine();
            string input = readline == null ? "" : readline.Trim().ToUpper();

            ProcessInput(input);
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
    }
}
