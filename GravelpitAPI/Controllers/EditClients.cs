using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace GravelpitAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EditClients : ControllerBase
    {
        [HttpPost]
        public void Post([FromBody] List<List<string>> clients)
        {
            using (MySqlConnection connection = DatabaseConnection.GetConnection())
            {
                connection.Open();

                var existingClients = new List<string>();
                var selectSql = "SELECT `id` FROM `clients`";
                using (MySqlCommand selectCommand = new MySqlCommand(selectSql, connection))
                {
                    using (MySqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            existingClients.Add(reader["id"].ToString());
                        }
                    }
                }

                var postedClientIds = clients.Select(t => t[0]).Where(id => !string.IsNullOrEmpty(id)).ToList();

                var clientsToDelete = existingClients.Except(postedClientIds).ToList();

                foreach (var client in clients)
                {
                    var id = client[0];
                    var name = client[1];

                    if (id == "")
                    {
                        var sql = $"INSERT INTO `clients` (`id`, `name`) VALUES (NULL, @name)";
                        using (MySqlCommand command = new MySqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@name", name);
                            command.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        var sql = $"UPDATE `clients` SET `name` = @name WHERE `clients`.`id` = @id";
                        using (MySqlCommand command = new MySqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@name", name);
                            command.Parameters.AddWithValue("@id", id);
                            command.ExecuteNonQuery();
                        }
                    }
                }

                if (clientsToDelete.Any())
                {
                    var deleteSql = $"DELETE FROM `clients` WHERE `id` IN ({string.Join(",", clientsToDelete.Select(id => $"'{id}'"))})";
                    using (MySqlCommand deleteCommand = new MySqlCommand(deleteSql, connection))
                    {
                        deleteCommand.ExecuteNonQuery();
                    }
                }

                connection.Close();
            }
        }
    }
}
