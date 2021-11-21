using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace B.MovableStorage.Commands
{
    public class AdminStorageCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "adminstorage";

        public string Help => String.Empty;

        public string Syntax => String.Empty;

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();

        public void Execute(IRocketPlayer caller, string[] command)
        {

            if (command.Length < 1)
            {
                UnturnedChat.Say(caller, "admincommand <view/list> <player> [id]", Color.red);
                return;
            }
            else
            {
                var ChatMessage = command[0].ToLower();
                var uPlayer = caller as UnturnedPlayer;

                UnturnedPlayer targetPlayer = UnturnedPlayer.FromName(command[1]);
                if (targetPlayer == null)
                {
                    if (ulong.TryParse(command[1], out ulong num))
                    {
                        Functions(num, uPlayer, command);
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    Functions(targetPlayer.CSteamID.m_SteamID, uPlayer, command);
                }
            }
        }

        public void Functions(ulong playerId, UnturnedPlayer uPlayer, string[] command)
        {
            var ChatMessage = command[0].ToLower();
            switch (ChatMessage)
            {
                case "view":
                    var lockerItems = new Items(7);
                    lockerItems.resize(Main.Instance.Configuration.Instance.WidthOfAdminStorage, Main.Instance.Configuration.Instance.HightOfAdminStorage);
                    var MStorage = Main.Instance.GetStorage(playerId, Convert.ToInt32(command[2]));

                    foreach (var item in MStorage.Items)
                    {
                        lockerItems.addItem(item.location_x, item.location_y, item.rot, new Item(item.item, item.amount, item.quality, item.state));
                    }
                    uPlayer.Player.inventory.updateItems(7, lockerItems);
                    uPlayer.Player.inventory.sendStorage();
                    break;
                case "list":
                    var PlayersStorages = Main.Instance.GetStorages(Convert.ToUInt64(playerId));
                    foreach (var storages in PlayersStorages)
                    {
                        UnturnedChat.Say(uPlayer, $"- ID: {storages.Id} | Player: {storages.OwnerName} | Storage: {storages.StorageID}");
                    }
                    break;
            }
        }
    }
}
