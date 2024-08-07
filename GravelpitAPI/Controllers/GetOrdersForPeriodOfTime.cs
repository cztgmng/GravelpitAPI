using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace GravelpitAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetOrdersForPeriodOfTime : ControllerBase
    {
        [HttpGet]
        public List<NewOrder> Get(string client, DateTime start, DateTime end)
        {
            var orders = new List<NewOrder>();
            using (MySqlConnection connection = DatabaseConnection.GetConnection())
            {
                connection.Open();

                var existingClients = new GetClients().Get();

                string query = "SELECT orders.id, orders.date, clients.name AS client_name, types.name AS type_name, orders.amount, orders.settled, orders.added_date FROM orders JOIN clients ON orders.client_id = clients.id JOIN types ON orders.type_id = types.id WHERE client_id = @client_id AND date >= @start AND date <= @end ORDER BY types.id ASC, orders.date ASC";

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@client_id", existingClients.FirstOrDefault(kvp => kvp.Value == client).Key);
                    cmd.Parameters.AddWithValue("@start", start.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@end", end.ToString("yyyy-MM-dd"));

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string id = reader.GetString("id");
                            DateTime date = reader.GetDateTime("date");
                            string type = reader.GetString("type_name");
                            float amount = reader.GetFloat("amount");
                            int settled = reader.GetInt32("settled");

                            DateTime? added_date = reader.IsDBNull(reader.GetOrdinal("added_date")) ? null : reader.GetDateTime("added_date");

                            orders.Add(new NewOrder()
                            {
                                id = id,
                                client = client,
                                date = date,
                                type = type,
                                amount = amount,
                                settled = settled,
                                added_date = added_date
                            });
                        }
                    }
                }
                connection.Close();
            }

            return orders;
        }
    }
}
