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

            if (Main.Instance.Configuration.Instance.Tool == nativePlayer.equipment.asset.id && Main.Instance.Configuration.Instance.HitStorageToPickup == true)
            {
                if (PhysicsUtility.raycast(new Ray(uPlayer.Player.look.aim.position, uPlayer.Player.look.aim.forward), out RaycastHit ahit, Mathf.Infinity, RayMasks.BARRICADE_INTERACT))
                {
                    Interactable2SalvageBarricade barri = ahit.transform.GetComponent<Interactable2SalvageBarricade>();
                    if (barri != null)
                    {
                        BarricadeManager.tryGetInfo(ahit.transform, out byte x, out byte y, out ushort index, out ushort bindex, out BarricadeRegion BarricadeRegion);
                        var BarricadeIndex = BarricadeRegion.barricades[bindex];
                        var BarricadeID = BarricadeIndex.barricade.asset.id;
                        var Storage = StorageHelper.GetInteractableStorage(uPlayer.Player);

                        var StoragesCount = Main.Instance.StorageCount(uPlayer.CSteamID.m_SteamID);
                        if (Main.Instance.Configuration.Instance.Storages.Contains(BarricadeID)
                               && Storage != null
                               && BarricadeIndex.owner == uPlayer.CSteamID.m_SteamID
                               && StoragesCount != Main.Instance.Configuration.Instance.AmountOfStorages
                               && Storage.items.items.Count != 0)
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
                        else
                        {
                            UnturnedChat.Say(uPlayer, "Storage Cannot be found", Color.red);
                        }
                    }
                }
            }
        }
    }
}