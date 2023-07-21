using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace B.MovableStorageV2.Patches.Item
{
    [HarmonyPatch(typeof(UseableMelee))]
    [HarmonyPatch("fire")]
    internal class MeleeUsePatch
    {
        public static void Prefix(UseableMelee __instance)
        {

        }
    }
}
