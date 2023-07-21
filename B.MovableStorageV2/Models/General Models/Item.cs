using System.Data;

namespace B.MovableStorageV2.Models.General_Models
{
    public class Item
    {
        public string ID { get; set; }

        public ushort ItemID { get; set; }
        
        public byte LocationX { get; set; }
        public byte LocationY { get; set; }
        public float Rot { get; set; }
        public byte Amount { get; set; }
        public byte Quality { get; set; }
        public byte[] State { get; set; }

        public Item(string id, ushort item_id, byte location_x, byte location_y, float rot, byte amount, byte quality, byte[] state)
        {
            ID = id;
            ItemID = item_id;
            LocationX = location_x;
            LocationY = location_y;
            Rot = rot;
            Amount = amount;
            Quality = quality;
            State = state;
        }

        public Item()
        {
        }
    }
}
