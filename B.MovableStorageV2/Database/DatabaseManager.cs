using Dapper;
using MySql.Data.MySqlClient;
using Rocket.Core.Logging;

namespace B.MovableStorageV2.Database
{
    public class DatabaseManager
    {
        public readonly Main PluginInstance;

        public DatabaseManager(Main plugin_instance)
        {
            PluginInstance = plugin_instance;
        }

        public void InitTables()
        {
            try
            {
                Connection.Execute(DatabaseQuery.StorageTable);
                Connection.Execute(DatabaseQuery.ItemTable);
            }
            catch (MySqlException ex)
            {
                Logger.LogError("We have an issue with the database: ");
                Logger.LogException(ex);
            }
        }

        public MySqlConnection Connection =>
            new MySqlConnection(PluginInstance.Configuration.Instance.ConnectionString);
    }
}
