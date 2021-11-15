using Rocket.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B.MovableStorage
{
    public class Main : RocketPlugin<Config>
    {
        public static Main Instance;

        protected override void Load()
        {
            var harmony = new HarmonyLib.Harmony("bluebeard.movablestorage");
            harmony.PatchAll();

            Instance = this;
		}

        protected override void Unload()
        {
        }
    }
}
