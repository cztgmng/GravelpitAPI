using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Text;

namespace GravelpitAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddClient : ControllerBase
    {

        [HttpPost]
        public void Post([FromBody] string clientName)
        {
            using (MySqlConnection connection = DatabaseConnection.GetConnection())
            {
                connection.Open();

                var sql = new StringBuilder("INSERT INTO `clients` (`id`, `name`) VALUES (NULL, @client)");

                using (MySqlCommand command = new MySqlCommand(sql.ToString(), connection))
                {
                    command.Parameters.AddWithValue("@client", clientName);
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }
    }
}
