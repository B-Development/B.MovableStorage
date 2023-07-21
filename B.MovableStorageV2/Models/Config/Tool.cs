using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B.MovableStorageV2.Models.Config
{
    public class Tool
    {
        public bool Enabled { get; set; }
        public ushort ToolID { get; set; }

        public Tool()
        {
        }

        public Tool(bool enabled, ushort tool)
        {
            Enabled = enabled;
            ToolID = tool;
        }
    }
}
