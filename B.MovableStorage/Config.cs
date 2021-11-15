using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B.MovableStorage
{
    public class Config : IRocketPluginConfiguration
    {
        public int AmountOfStorages;
        public List<ushort> Tools;
        public List<ushort> Storages;

        public void LoadDefaults()
        {
            AmountOfStorages = 3;
            Tools = new List<ushort>()
            {
                138
            };

            Storages = new List<ushort>()
            {
                328
            };
        }
    }
}
