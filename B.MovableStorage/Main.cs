using B.MovableStorage.Helpers;
using B.MovableStorage.Storage;
using Rocket.Core.Plugins;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Framework.Utilities;
using SDG.Unturned;
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

            UnturnedPlayerEvents.OnPlayerUpdateGesture += OnGesture;
        }

        private void OnGesture(UnturnedPlayer player, UnturnedPlayerEvents.PlayerGesture gesture)
        {
            if(gesture == UnturnedPlayerEvents.PlayerGesture.PunchLeft || gesture == UnturnedPlayerEvents.PlayerGesture.PunchRight)
            {
                if (PhysicsUtility.raycast(new Ray(player.Player.look.aim.position, player.Player.look.aim.forward), out RaycastHit ahit, Mathf.Infinity, RayMasks.BARRICADE_INTERACT))
                {
                    Interactable2SalvageBarricade barri = ahit.transform.GetComponent<Interactable2SalvageBarricade>();
                    if (barri != null)
                    {
                        BarricadeManager.tryGetInfo(ahit.transform, out byte x, out byte y, out ushort index, out ushort bindex, out BarricadeRegion barricadeR);
                        var BarricadeIndex = barricadeR.barricades[bindex];
                        var BarricadeID = BarricadeIndex.barricade.id;

                        var Storage = StorageHelper.GetInteractableStorage(player.Player);
                        var MStorage = GetStorage(player.CSteamID.m_SteamID, BarricadeID);
                        if (Storage != null && MStorage != null)
                        {
                            foreach (var item in MStorage.Items)
                            {
                                Storage.items.items.Add(new ItemJar(item.location_x, item.location_y, item.rot, new Item(item.item, item.amount, item.quality, item.state)));
                            }

                            RemoveStorage(player.CSteamID.m_SteamID, BarricadeID);
                        }
                    }
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
            UnturnedPlayerEvents.OnPlayerUpdateGesture -= OnGesture;
        }
    }
}
