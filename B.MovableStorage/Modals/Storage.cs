using System.Collections.Generic;

namespace B.MovableStorage.Modals
{
    public class Storage
    {
        public ushort StorageID;
        public List<Item> Items;
        public ulong Owner;

        public Storage()
        {
        }

        public Storage(ushort storageID, List<Item> items, ulong owner)
        {
            StorageID = storageID;
            Items = items;
            Owner = owner;
        }
    }
}
