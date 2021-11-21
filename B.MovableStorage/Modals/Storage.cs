using System.Collections.Generic;

namespace B.MovableStorage.Modals
{
    public class Storage
    {
        public int Id { get; set; }
        public ushort StorageID { get; set; }
        public List<Item> Items { get; set; }
        public ulong Owner { get; set; }
        public string OwnerName { get; set; }

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
