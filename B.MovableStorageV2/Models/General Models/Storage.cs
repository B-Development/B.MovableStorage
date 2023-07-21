using System;
using System.Collections.Generic;

namespace B.MovableStorageV2.Models.General_Models
{
    public class Storage
    {
        public string ID { get; set; }
        public ushort StorageID { get; set; }

        public ulong CurrentHolder { get; set; }
        public byte PosXInv { get; set; }
        public byte PosYInv { get; set; }
        public byte PageInv { get; set; }
        public float Rotation { get; set; }

        public List<Item> Items { get; set; }

        public Storage(string id, ushort storage_id, ulong current_holder, byte pos_x_inv, byte pos_y_inv, byte page_inv, float rotation, List<Item> items)
        {
            ID = id;
            StorageID = storage_id;
            CurrentHolder = current_holder;
            PosXInv = pos_x_inv;
            PosYInv = pos_y_inv;
            PageInv = page_inv;
            Rotation = rotation;
            Items = items;
        }

        public Storage(Guid id, ushort storage_id, ulong current_holder, byte pos_x_inv, byte pos_y_inv, byte page_inv, float rotation, List<Item> items) : this(id.ToString(), storage_id, current_holder, pos_x_inv, pos_y_inv, page_inv, rotation, items)
        {
        }

        public Storage()
        {
        }
    }
}
