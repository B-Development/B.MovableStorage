using Rocket.Unturned.Player;
using SDG.Framework.Utilities;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace B.MovableStorage.Helpers
{
    public class StorageHelper
    {
        public static InteractableStorage GetInteractableStorage(Player player)
        {
            var look = player.look;
            if (PhysicsUtility.raycast(new Ray(look.aim.position, look.aim.forward), out RaycastHit hit, Mathf.Infinity, RayMasks.BARRICADE_INTERACT))
            {
                var storage = hit.transform.GetComponent<InteractableStorage>();
                if (storage != null)
                {
                    return storage;
                }
            }

            return null;
        }

    }
}
