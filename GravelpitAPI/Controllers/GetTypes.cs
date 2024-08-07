using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace GravelpitAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetTypes : ControllerBase
    {
        [HttpGet]
        public Dictionary<string, string> Get()
        {
            var types = new Dictionary<string, string>();

            using (MySqlConnection connection = DatabaseConnection.GetConnection())
            {
                connection.Open();

                string query = $"SELECT * FROM types ORDER BY `types`.`id` ASC";

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            types.Add(reader.GetString("id"), reader.GetString("name"));
                        }
                    }
                }
                connection.Close();
            }

            return types;
        }
    }
}
