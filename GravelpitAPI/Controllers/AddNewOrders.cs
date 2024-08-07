using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Text;

namespace GravelpitAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddNewOrders : ControllerBase
    {
        [HttpPost]
        public void Post([FromBody] List<NewOrder> orders)
        {
            using (MySqlConnection connection = DatabaseConnection.GetConnection())
            {
                connection.Open();

                List<string> clients = new();
                List<string> types = new();

                foreach (var order in orders)
                {
                    if (!clients.Contains(order.client))
                        clients.Add(order.client);

                    if (!types.Contains(order.type))
                        types.Add(order.type);
                }

                foreach (var client in clients)
                {
                    using (MySqlCommand command = new MySqlCommand($"INSERT IGNORE INTO `clients` (`id`, `name`) VALUES (NULL, '{client}')", connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                foreach (var type in types)
                {
                    using (MySqlCommand command = new MySqlCommand($"INSERT IGNORE INTO `types` (`id`, `name`) VALUES (NULL, '{type}')", connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                var existingClients = new GetClients().Get();
                var existingTypes = new GetTypes().Get();

                var sql = new StringBuilder("INSERT INTO `orders` (`date`, `client_id`, `type_id`, `amount`, `added_date`) VALUES ");
                var parameters = new List<MySqlParameter>();

                for (int i = 0; i < orders.Count; i++)
                {
                    var order = orders[i];
                    sql.Append($"(@date{i}, @client_id{i}, @type_id{i}, @amount{i}, @added_date{i}),");

                    parameters.Add(new MySqlParameter($"@date{i}", order.date.ToString("yyyy-MM-dd")));
                    parameters.Add(new MySqlParameter($"@client_id{i}", existingClients.FirstOrDefault(kvp => kvp.Value == order.client).Key));
                    parameters.Add(new MySqlParameter($"@type_id{i}", existingTypes.FirstOrDefault(kvp => kvp.Value == order.type).Key));
                    parameters.Add(new MySqlParameter($"@amount{i}", order.amount.ToString().Replace(',', '.')));
                    parameters.Add(new MySqlParameter($"@added_date{i}", order.added_date.HasValue ? (object)order.added_date.Value.ToString("yyyy-MM-dd HH:mm:ss") : DBNull.Value));
                }

                if (orders.Count > 0)
                {
                    sql.Length--;
                }

                using (MySqlCommand command = new MySqlCommand(sql.ToString(), connection))
                {
                    command.Parameters.AddRange(parameters.ToArray());
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }

            return;
        }
    }
}
