using HarmonyLib;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B.MovableStorageV2.Patches.Inventory
{
    [HarmonyPatch(typeof(PlayerInventory))]
    [HarmonyPatch("ReceiveDropItem")]
    internal class ItemDropPatch
    {
        public static void Prefix(byte page, byte x, byte y, PlayerInventory __instance)
        {

        }
    }
}
