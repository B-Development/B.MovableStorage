using Rocket.API;
using System.Collections.Generic;

namespace B.MovableStorage
{
    public class Config : IRocketPluginConfiguration
    {
        public int AmountOfStorages;
        public bool HitStorageToPickup;
        public ushort Tool;
        public List<ushort> Storages;

        public void LoadDefaults()
        {
            AmountOfStorages = 3;
            HitStorageToPickup = false;
            Tool = 138;

            Storages = new List<ushort>()
            {
                328
            };
        }
    }
}
