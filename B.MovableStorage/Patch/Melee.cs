using B.MovableStorage.Helpers;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Framework.Utilities;
using SDG.Unturned;
using System.Collections.Generic;
using System.Linq;
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

            if (PhysicsUtility.raycast(new Ray(uPlayer.Player.look.aim.position, uPlayer.Player.look.aim.forward), out RaycastHit ahit, Mathf.Infinity, RayMasks.BARRICADE_INTERACT))
            {
                Interactable2SalvageBarricade barri = ahit.transform.GetComponent<Interactable2SalvageBarricade>();
                if (barri != null)
                {
                    BarricadeManager.tryGetInfo(ahit.transform, out byte x, out byte y, out ushort index, out ushort bindex, out BarricadeRegion BarricadeRegion);
                    var BarricadeIndex = BarricadeRegion.barricades[bindex];
                    var BarricadeID = BarricadeIndex.barricade.asset.id;
                    var Storage = StorageHelper.GetInteractableStorage(uPlayer.Player);
                    var StoragesCount = Main.Instance.StorageCount(uPlayer.CSteamID.m_SteamID, BarricadeID);

                    if (Main.Instance.Configuration.Instance.Storages.Any(storage => storage.StorageID == BarricadeID) && Storage != null)
                    {
                        var ConfigStorage = Main.Instance.Configuration.Instance.Storages.FirstOrDefault(storage => storage.StorageID == BarricadeID);
                        if (ConfigStorage.Tool.Enabled && Storage.items.items.Count != 0 && BarricadeIndex.owner == uPlayer.CSteamID.m_SteamID && StoragesCount != ConfigStorage.AmountAllowed && ConfigStorage.Tool.ToolID == nativePlayer.equipment.asset.id)
                        {
                            List<Modals.Item> items = new List<Modals.Item>();
                            while (Storage.items.items.Count() > 0)
                            {
                                var firstItem = Storage.items.items.First();
                                items.Add(new Modals.Item(firstItem.item.id, firstItem.x, firstItem.y, firstItem.rot, firstItem.item.amount, firstItem.item.quality, firstItem.item.metadata));
                                Storage.items.items.RemoveAt(Storage.items.getIndex(Storage.items.items.First().x, Storage.items.items.First().y));
                            }

                            var ID = StoragesCount + 1;
                            var storeData = new Modals.Storage()
                            {
                                Id = ID,
                                OwnerName = uPlayer.DisplayName,
                                StorageID = BarricadeID,
                                Owner = uPlayer.CSteamID.m_SteamID,
                                Items = items
                            };
                            Main.Instance.AddStorage(storeData);
                            BarricadeManager.destroyBarricade(BarricadeRegion, x, y, index, bindex);

                            ItemManager.dropItem(new Item(BarricadeID, true), uPlayer.Position, false, false, false);
                        }
                    }
                }
            }
        }
    }
}