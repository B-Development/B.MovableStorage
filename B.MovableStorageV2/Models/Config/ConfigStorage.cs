using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B.MovableStorageV2.Models.Config
{
    public class ConfigStorage
    {
        public ushort StorageID { get; set; }
        public int AmountAllowed { get; set; }
        public Tool Tool { get; set; }

        public ConfigStorage()
        {
        }

        public ConfigStorage(ushort storage, int amount, Tool tool)
        {
            StorageID = storage;
            AmountAllowed = amount;
            Tool = tool;
        }
    }
}
