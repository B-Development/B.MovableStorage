using B.MovableStorage.Helpers;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Framework.Utilities;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace B.MovableStorage.Patch
{
    [HarmonyLib.HarmonyPatch(typeof(UseableMelee), "fire")]
    public static class Melee
    {
        [HarmonyLib.HarmonyPrefix]
        internal static void OnPlayerAttackInvoker(UseableMelee __instance)
        {
            var nativePlayer = __instance.player;
            var uPlayer = UnturnedPlayer.FromPlayer(nativePlayer);

            if (Main.Instance.Configuration.Instance.Tools.Contains(nativePlayer.equipment.asset.id))
            {

                if (PhysicsUtility.raycast(new Ray(nativePlayer.look.aim.position, nativePlayer.look.aim.forward), out RaycastHit ahit, Mathf.Infinity, RayMasks.BARRICADE_INTERACT))
                {
                    Interactable2SalvageBarricade barri = ahit.transform.GetComponent<Interactable2SalvageBarricade>();
                    if (barri != null)
                    {
                        BarricadeManager.tryGetInfo(ahit.transform, out byte x, out byte y, out ushort index, out ushort bindex, out BarricadeRegion BarricadeRegion);
                        var BarricadeIndex = BarricadeRegion.barricades[bindex];
                        var BarricadeID = BarricadeIndex.barricade.asset.id;
                        var Storage = StorageHelper.GetInteractableStorage(nativePlayer);

                        if (Main.Instance.Configuration.Instance.Storages.Contains(BarricadeID) && Storage != null && BarricadeIndex.owner == uPlayer.CSteamID.m_SteamID)
                        {
                            Storage.items.items.Clear();
                            BarricadeManager.destroyBarricade(BarricadeRegion, x, y, index, bindex);

                            ItemManager.dropItem(new Item(BarricadeID, true), uPlayer.Position, false, false, false);
                        }
                        else
                        {
                            UnturnedChat.Say("Storage Cannot be found");
                        }
                    }
                }
            }
        }
    }
}