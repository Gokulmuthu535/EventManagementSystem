using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using EventManagementSystem.Models;
using EventManagementSystem.Repository;

namespace EventManagementSystem.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task AddUserAsync(User user, string passwordHash)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("AddUser", connection) { CommandType = CommandType.StoredProcedure };

            command.Parameters.AddWithValue("@FirstName", user.FirstName);
            command.Parameters.AddWithValue("@LastName", user.LastName);
            command.Parameters.AddWithValue("@EmailAddress", user.EmailAddress);
            command.Parameters.AddWithValue("@Username", user.Username);
            command.Parameters.AddWithValue("@PasswordHash", passwordHash);
            command.Parameters.AddWithValue("@ConfirmPassword", user.ConfirmPassword);
            command.Parameters.AddWithValue("@DateofBirth", user.DateOfBirth);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("GetUserByUsername", connection) { CommandType = CommandType.StoredProcedure };

            command.Parameters.AddWithValue("@Username", username);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new User
                {
                    UserId = Convert.ToInt32(reader["UserId"]),
                    FirstName = reader["FirstName"].ToString(),
                    LastName = reader["LastName"].ToString(),
                    EmailAddress = reader["EmailAddress"].ToString(),
                    Username = reader["Username"].ToString(),
                    Password = reader["PasswordHash"].ToString() // Fetch password hash
                };
            }

            return null;
        }

        public async Task<List<User>> GetAllUsers()
        {
            var users = new List<User>();

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("GetUsers", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var userItem = new User
                            {
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                EmailAddress = reader["EmailAddress"].ToString(),
                                Username = reader["Username"].ToString(),
                                Password = reader["PasswordHash"].ToString(), // Fetch password hash
                                Address = reader["Address"].ToString(),
                                DateOfBirth = (DateTime)reader["DateofBirth"]
                            };
                            users.Add(userItem);
                        }
                    }
                }
            }

            return users;
        }
    }
}