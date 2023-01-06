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
    
    public class ItemAction
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public ACTION_TYPE Type { get; private set; }
        public Func<string, string> Action { get; private set; }

        public ItemAction(string name, string description, ACTION_TYPE type, Func<string, string> action)
        {
            this.Name = name.ToUpper();
            this.Description = description;
            this.Type = type;
            this.Action = action;
        }
    }
}
