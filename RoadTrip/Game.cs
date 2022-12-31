using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;

namespace RoadTrip
{
    public class Game
    {
        public int GameTime { get; private set; }
        private PeriodicTimer Timer;
        private static TimeSpan MillisecondsPerFrame = TimeSpan.FromSeconds(1);
        private List<Event> EventQueue = new List<Event>();
        private Parser parser = new Parser();
        private bool Paused = false;
        
        public Game()
        {
            GameTime = 0;
            Timer = new PeriodicTimer(MillisecondsPerFrame);
            StartTimer();

            //Test
            SetupTestData();

            while (!Paused)
            {
                GetInput();
            }
        }
        
        private void GetInput()
        {
            string? readline = Console.ReadLine();
            parser.ParseInput(readline == null ? "" : readline.ToUpper());
            ProcessScheduledEvents();
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

        private void SetupTestData()
        {
            Func<string> test = TestFunc;
            EventQueue.Add(new Event(GameTime + 10, test));
            EventQueue.Add(new Event(GameTime + 15, test));
            EventQueue.Add(new Event(GameTime + 15, test));
            EventQueue.Add(new Event(GameTime + 15, test));
        }

        private string TestFunc()
        {
            return "This is the test function!";
        }
    }
}
