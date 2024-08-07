using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace GravelpitAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeleteOrder : ControllerBase
    {
        [HttpPost]
        public void Post([FromBody] string id)
        {
            using (MySqlConnection connection = DatabaseConnection.GetConnection())
            {
                connection.Open();

                string sql = $"DELETE FROM `orders` WHERE `orders`.`id` = @id";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    command.ExecuteNonQuery();
                }

                connection.Close();
            }

            return;
        }
    }
}
