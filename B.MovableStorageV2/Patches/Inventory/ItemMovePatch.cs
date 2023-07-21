using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using SDG.Unturned;

namespace B.MovableStorageV2.Patches.Inventory
{
    [HarmonyPatch(typeof(PlayerInventory))]
    [HarmonyPatch("ReceiveDragItem")]
    internal class ItemMovePatch
    {
        public static void Prefix(byte page_0, byte x_0, byte y_0, byte page_1, byte x_1, byte y_1, byte rot_1, PlayerInventory __instance)
        {

        }
    }
}
