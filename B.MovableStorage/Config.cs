using Rocket.API;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace B.MovableStorage
{
    public class Config : IRocketPluginConfiguration
    {
        public int AmountOfStorages;
        public byte WidthOfAdminStorage;
        public byte HightOfAdminStorage;
        [XmlArray("Storages")]
        [XmlArrayItem("Storage")]
        public List<ConfigStorages> Storages;

        public void LoadDefaults()
        {
            AmountOfStorages = 3;
            WidthOfAdminStorage = 10;
            HightOfAdminStorage = 10;

            Storages = new List<ConfigStorages>()
            {
                new ConfigStorages(328, 2, new Tool(true, 138)),
                new ConfigStorages(366, 5, new Tool(false, 138))
            };
        }
    }

    public class ConfigStorages
    {
        public ushort StorageID { get; set; }
        public int AmountAllowed { get; set; }
        public Tool Tool { get; set; }

        public ConfigStorages()
        {
        }

        public ConfigStorages(ushort storage, int amount, Tool tool)
        {
            StorageID = storage;
            AmountAllowed = amount;
            Tool = tool;
        }
    }

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
