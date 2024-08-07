using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace GravelpitAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetClients : ControllerBase
    {
        [HttpGet]
        public Dictionary<string, string> Get()
        {
            var clients = new Dictionary<string, string>();
            using (MySqlConnection connection = DatabaseConnection.GetConnection())
            {
                connection.Open();

                string query = $"SELECT * FROM clients";

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            clients.Add(reader.GetString("id"), reader.GetString("name"));
                        }
                    }
                }
                connection.Close();
            }

            return clients;
        }
    }
}
