﻿namespace B.MovableStorage.Modals
{
    public class Item
    {
        public ushort item;
        public byte location_x;
        public byte location_y;
        public byte rot;
        public byte amount;
        public byte quality;
        public byte[] state;

        public Item()
        {
        }

        public Item(ushort Item, byte Location_X, byte Location_Y, byte Rot, byte Amount, byte Quality, byte[] State)
        {
            item = Item;
            location_x = Location_X;
            location_y = Location_Y;
            rot = Rot;
            amount = Amount;
            quality = Quality;
            state = State;
        }
    }
}
