using B.MovableStorage.Helpers;
using B.MovableStorage.Storage;
using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Framework.Utilities;
using SDG.Unturned;
using Steamworks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace B.MovableStorage
{
    public class Main : RocketPlugin<Config>
    {
        public DataStorage<List<Modals.Storage>> StorageDataStorage { get; private set; }

        private List<Modals.Storage> storageData;
        public static Main Instance;

        protected override void Load()
        {
            StorageDataStorage = new DataStorage<List<Modals.Storage>>(Directory, "StorageItems.json");
            ReloadStorageData();

            var harmony = new HarmonyLib.Harmony("bluebeard.movablestorage");
            harmony.PatchAll();
            
            Instance = this;

            BarricadeManager.onBarricadeSpawned += OnBarricadeSpawned;
            BarricadeManager.onSalvageBarricadeRequested += OnBarricadeSalvage;
        }

        private void OnBarricadeSalvage(CSteamID steamID, byte _x, byte _y, ushort _plant, ushort _index, ref bool shouldAllow)
        {

            var uPlayer = UnturnedPlayer.FromCSteamID(steamID);
            if (PhysicsUtility.raycast(new Ray(uPlayer.Player.look.aim.position, uPlayer.Player.look.aim.forward), out RaycastHit ahit, Mathf.Infinity, RayMasks.BARRICADE_INTERACT) && Main.Instance.Configuration.Instance.HitStorageToPickup == false)
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
                           && StoragesCount != Main.Instance.Configuration.Instance.AmountOfStorages)
                    {
                        List<Modals.Item> items = new List<Modals.Item>();
                        while (Storage.items.items.Count() > 0)
                        {
                            var firstItem = Storage.items.items.First();
                            items.Add(new Modals.Item(firstItem.item.id, firstItem.x, firstItem.y, firstItem.rot, firstItem.item.amount, firstItem.item.quality, firstItem.item.metadata));
                            Storage.items.items.RemoveAt(Storage.items.getIndex(Storage.items.items.First().x, Storage.items.items.First().y));
                        }
                        var storeData = new Modals.Storage()
                        {
                            StorageID = BarricadeID,
                            Owner = uPlayer.CSteamID.m_SteamID,
                            Items = items
                        };
                        Main.Instance.AddStorage(storeData);
                        BarricadeManager.destroyBarricade(BarricadeRegion, x, y, index, bindex);
                    }
                    else
                    {
                        UnturnedChat.Say(uPlayer, "Storage Cannot be found", Color.red);
                    }
                }
            }
        }

        private void OnBarricadeSpawned(BarricadeRegion region, BarricadeDrop drop)
        {
            var barricade = region.findBarricadeByInstanceID(drop.instanceID);
            var uPlayer = UnturnedPlayer.FromCSteamID(new Steamworks.CSteamID(barricade.owner));

            if (PhysicsUtility.raycast(new Ray(uPlayer.Player.look.aim.position, uPlayer.Player.look.aim.forward), out RaycastHit ahit, Mathf.Infinity, RayMasks.BARRICADE_INTERACT))
            {
                Interactable2SalvageBarricade barri = ahit.transform.GetComponent<Interactable2SalvageBarricade>();
                InteractableStorage storage = ahit.transform.GetComponent<InteractableStorage>();
                var MStorage = GetStorage(uPlayer.CSteamID.m_SteamID, barricade.barricade.id);
                if (barri != null && storage != null && MStorage != null)
                {
                    foreach (var item in MStorage.Items)
                    {
                        storage.items.addItem(item.location_x, item.location_y, item.rot, new Item(item.item, item.amount, item.quality, item.state));
                    }

                    RemoveStorage(uPlayer.CSteamID.m_SteamID, barricade.barricade.id);
                }
            }
        }
        #region Tests
        public void ReloadStorageData()
        {
            storageData = StorageDataStorage.Read();
            if (storageData == null)
            {
                storageData = new List<Modals.Storage>();
            }
        }
        public void AddStorage(Modals.Storage storage)
        {
            storageData.Add(storage);
            StorageDataStorage.Save(storageData);
        }
        public Modals.Storage GetStorage(ulong owner, ushort barricade)
        {
            return storageData.FirstOrDefault(x => x.StorageID == barricade && x.Owner == owner);
        }
        public int StorageCount(ulong owner)
        {
            var amount = storageData.Where(x => x.Owner == owner).Count();

            return amount;
        }
        public void RemoveStorage(ulong owner, ushort barricade)
        {
            storageData.Remove(storageData.FirstOrDefault(x => x.Owner == owner && x.StorageID == barricade));
            StorageDataStorage.Save(storageData);
        }
        #endregion Tests
        protected override void Unload()
        {
            BarricadeManager.onBarricadeSpawned -= OnBarricadeSpawned;
        }
    }
}
