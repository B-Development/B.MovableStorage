using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B.MovableStorageV2.Database;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Player;
using Steamworks;

namespace B.MovableStorageV2
{
    public class Main : RocketPlugin<Configuration>
    {
        public static Main Instance { get; set; }
        public DatabaseManager DatabaseManager { get; set; }

        protected override void Load()
        {
            Instance = this;

            DatabaseManager = new DatabaseManager(this);
            DatabaseManager.InitTables();
        }

        protected override void Unload()
        {
        }
    }
}
