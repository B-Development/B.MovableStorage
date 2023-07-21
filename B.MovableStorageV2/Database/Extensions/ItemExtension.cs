using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B.MovableStorageV2.Models.General_Models;
using Dapper;

namespace B.MovableStorageV2.Database.Extensions
{
    public static class ItemExtension
    {
        public static void AddItem(this DatabaseManager database, Item item, string storage_link)
        {
            const string sql =
                "INSERT INTO Items (ID, ItemID, LocationX, LocationY, Rot, Amount, Quality, State, StorageLink) VALUES (@ID, @ItemID, @LocationX, @LocationY, @Rot, @Amount, @Quality, @State, @StorageLink);";

            using (var conn = database.Connection)
            {
                conn.Execute(sql,
                    new
                    {
                        item.ID,
                        item.ItemID,
                        item.LocationX,
                        item.LocationY,
                        item.Rot,
                        item.Amount,
                        State = item.State.ToString(),
                        StorageLink = storage_link
                    });
            }
        }

        public static void RemoveItem(this DatabaseManager database, string storage_link)
        {
            const string sql = "DELETE FROM Items WHERE StorageLink = @Link;";

            using (var conn = database.Connection)
            {
                conn.Execute(sql, new { Link = storage_link });
            }
        }
    }
}
