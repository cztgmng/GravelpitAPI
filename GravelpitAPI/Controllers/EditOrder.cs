using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace GravelpitAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EditOrder : ControllerBase
    {
        [HttpPost]
        public void Post([FromBody] NewOrder order)
        {
            using (MySqlConnection connection = DatabaseConnection.GetConnection())
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand($"INSERT IGNORE INTO `clients` (`id`, `name`) VALUES (NULL, '{order.client}')", connection))
                {
                    command.ExecuteNonQuery();
                }
                using (MySqlCommand command = new MySqlCommand($"INSERT IGNORE INTO `types` (`id`, `name`) VALUES (NULL, '{order.type}')", connection))
                {
                    command.ExecuteNonQuery();
                }

                var existingClients = new GetClients().Get();
                var existingTypes = new GetTypes().Get();

                string sql = "UPDATE `orders` SET `date` = @date, `client_id` = @client_id, `type_id` = @type_id, `amount` = @amount, `settled` = @settled WHERE `orders`.`id` = @id";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@date", order.date.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@client_id", existingClients.FirstOrDefault(kvp => kvp.Value == order.client).Key);
                    command.Parameters.AddWithValue("@type_id", existingTypes.FirstOrDefault(kvp => kvp.Value == order.type).Key);
                    command.Parameters.AddWithValue("@amount", order.amount.ToString().Replace(',', '.'));
                    command.Parameters.AddWithValue("@settled", order.settled);
                    command.Parameters.AddWithValue("@id", order.id);

                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

    }
}
