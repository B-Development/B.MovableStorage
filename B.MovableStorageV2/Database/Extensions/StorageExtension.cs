using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B.MovableStorageV2.Models.General_Models;
using Dapper;

namespace B.MovableStorageV2.Database.Extensions
{
    public static class StorageExtension
    {
        #region Add Storage

        public static void AddStorage(this DatabaseManager database, Storage storage)
        {
            const string sql =
                "INSERT INTO Storages (ID, StorageID, CurrentHolder, PosXInv, PosYInv, PageInv, Rotation) VALUES (@ID, @StorageID, @CurrentHolder, @PosXInv, @PosYInv, @PageInv, @Rotation);";

            using (var conn = database.Connection)
            {
                conn.Execute(sql,
                    new
                    {
                        storage.ID,
                        storage.StorageID,
                        storage.CurrentHolder,
                        storage.PosXInv,
                        storage.PosYInv,
                        storage.Rotation
                    });
            }

            foreach (var item in storage.Items)
            {
                Main.Instance.DatabaseManager.AddItem(item, storage.ID);
            }
        }

        #endregion

        #region Remove Storage

        public static void RemoveStorage(this DatabaseManager database, ulong holder, byte page, byte pos_x, byte pos_y,
            float rot)
        {
            const string get_record = "SELECT ID FROM Storages WHERE CurrentHolder = @Holder AND PosXInv = @PosX AND PosYInv = @PosY AND PageInv = @Page AND Rotation = @Rot;";

            using (var conn = database.Connection)
            {
                var record_id = conn.QuerySingle<string>(get_record, new { Holder = holder, PosX = pos_x, PosY = pos_y, Page = page, Rot = rot });
            }
        }

        #endregion

        #region Update Methods

        public static void UpdateHolder(this DatabaseManager database, string id, ulong holder)
        {
            const string sql = "UPDATE Storages SET CurrentHolder = @Holder WHERE ID = @ID;";

            using (var conn = database.Connection)
            {
                conn.Execute(sql, new { Holder = holder, ID = id });
            }
        }

        public static void UpdatePosX(this DatabaseManager database, string id, byte x)
        {
            const string sql = "UPDATE Storages SET PosXInv = @LocX WHERE ID = @ID;";

            using (var conn = database.Connection)
            {
                conn.Execute(sql, new { LocX = x, ID = id });
            }
        }

        public static void UpdatePosY(this DatabaseManager database, string id, byte y)
        {
            const string sql = "UPDATE Storages SET PosYInv = @LocY WHERE ID = @ID;";

            using (var conn = database.Connection)
            {
                conn.Execute(sql, new { LocY = y, ID = id });
            }
        }

        public static void UpdatePage(this DatabaseManager database, string id, byte page)
        {
            const string sql = "UPDATE Storages SET PageInv = @Page WHERE ID = @ID;";

            using (var conn = database.Connection)
            {
                conn.Execute(sql, new { Page = page, ID = id });
            }
        }

        #endregion
    }
}
