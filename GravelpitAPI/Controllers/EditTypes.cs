using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace GravelpitAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EditTypes : ControllerBase
    {
        [HttpPost]
        public void Post([FromBody] List<List<string>> types)
        {
            using (MySqlConnection connection = DatabaseConnection.GetConnection())
            {
                connection.Open();

                var existingTypes = new List<string>();
                var selectSql = "SELECT `id` FROM `types`";
                using (MySqlCommand selectCommand = new MySqlCommand(selectSql, connection))
                {
                    using (MySqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            existingTypes.Add(reader["id"].ToString());
                        }
                    }
                }

                var postedTypeIds = types.Select(t => t[0]).Where(id => !string.IsNullOrEmpty(id)).ToList();

                var typesToDelete = existingTypes.Except(postedTypeIds).ToList();

                foreach (var type in types)
                {
                    var id = type[0];
                    var name = type[1];

                    if (id == "")
                    {
                        var sql = $"INSERT INTO `types` (`id`, `name`) VALUES (NULL, @name)";
                        using (MySqlCommand command = new MySqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@name", name);
                            command.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        var sql = $"UPDATE `types` SET `name` = @name WHERE `types`.`id` = @id";
                        using (MySqlCommand command = new MySqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@name", name);
                            command.Parameters.AddWithValue("@id", id);
                            command.ExecuteNonQuery();
                        }
                    }
                }

                if (typesToDelete.Any())
                {
                    var deleteSql = $"DELETE FROM `types` WHERE `id` IN ({string.Join(",", typesToDelete.Select(id => $"'{id}'"))})";
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
