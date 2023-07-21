using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using B.MovableStorageV2.Models.Config;
using Rocket.API;

namespace B.MovableStorageV2
{
    public class Configuration : IRocketPluginConfiguration
    {
        public string ConnectionString { get; set; }

        [XmlArray("ActiveStorages")]
        [XmlArrayItem("Storage")]
        public List<ConfigStorage> StorageList { get; set; }

        public void LoadDefaults()
        {
            ConnectionString = "Server=127.0.0.1;Port=3306;Database=unturned;Uid=root;Password=Password123;";

            StorageList = new List<ConfigStorage>
            {
                new ConfigStorage(328, 2, new Tool(true, 138)),
                new ConfigStorage(366, 5, new Tool(false, 138))
            };
        }
    }
}
