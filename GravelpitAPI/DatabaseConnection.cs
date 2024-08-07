using MySql.Data.MySqlClient;

namespace GravelpitAPI.Controllers
{
    public class DatabaseConnection
    {
        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection($"Server={staticVariables.ipAddress};Database={staticVariables.databaseName};User ID={staticVariables.databaseUserName};Password={staticVariables.databaseUserPassword};");
        }
    }
}
