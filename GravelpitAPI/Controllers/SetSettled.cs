using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.X509;

namespace GravelpitAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetSettled : ControllerBase
    {
        [HttpGet]
        public string Get(string client, DateTime start, DateTime end, string settled)
        {
            using (MySqlConnection connection = DatabaseConnection.GetConnection())
            {
                connection.Open();

                var existingClients = new GetClients().Get();

                string updateQuery = "UPDATE `orders` SET `settled` = @settled WHERE `client_id` = @client_id AND `date` >= @start AND `date` <= @end";
                using (MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection))
                {
                    updateCommand.Parameters.AddWithValue("@settled", settled);
                    updateCommand.Parameters.AddWithValue("@client_id", existingClients.FirstOrDefault(kvp => kvp.Value == client).Key);
                    updateCommand.Parameters.AddWithValue("@start", start.ToString("yyyy-MM-dd"));
                    updateCommand.Parameters.AddWithValue("@end", end.ToString("yyyy-MM-dd"));

                    updateCommand.ExecuteNonQuery();
                }

                connection.Close();
            }

            return "Values set.";
        }
    }
}
