using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadTrip
{
    internal class Event
    {
        public int ScheduledTime { get; private set; }
        public Func<string> Action { get; private set; }
        
        public Event(int scheduledTime, Func<string> eventAction)
        {
            this.ScheduledTime = scheduledTime;
            this.Action = eventAction;
        }
    }
}
