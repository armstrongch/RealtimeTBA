using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadTrip
{
    public enum ACTION_TYPE
    {
        WORLD = 0,
        INVENTORY = 1,
    }
    
    internal class ItemAction
    {
        public string Name { get; private set; }
        public ACTION_TYPE Type { get; private set; }
        public Func<string, string> Action { get; private set; }

        public ItemAction(string name, ACTION_TYPE type, Func<string, string> action)
        {
            this.Name = name;
            this.Type = type;
            this.Action = action;
        }
    }
}
