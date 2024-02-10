using Application.Contracts.Repositories;
using Application.Features.Drivers.Commands.CreateDriver;
using Application.Features.Drivers.Commands.EditDriver;
using Domain.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System.Data.Common;

namespace Persistence.Implementation.Repositories
{
    public class DriverRepository : IDriverRepository
    {
        private readonly IConfiguration _configuration;

        public DriverRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Guid> CreateDriver(CreateDriverCommand driver)
        {
            var query = @"
                INSERT INTO Driver (Id, FirstName, LastName, Email, PhoneNumber, CreatedDate, CreatedBy, IsDeleted)
                VALUES (@Id, @FirstName, @LastName, @Email, @PhoneNumber, @CreatedDate, 'Test User', 0)";

            var parameters = new Dictionary<string, object>
            {
                {"@Id", Guid.NewGuid().ToString()},
                {"@FirstName", driver.FirstName},
                {"@LastName", driver.LastName},
                {"@Email", driver.Email},
                {"@PhoneNumber", driver.PhoneNumber},
                {"@CreatedDate", DateTimeOffset.Now.ToString()},
                {"@CreatedBy", "Test User"}
            };

            await using var connection = await GetOpenedConnection();
            await using var command = connection.CreateCommand();
            command.CommandText = query;
            AddParametersToCommand(command, parameters);
            await command.ExecuteNonQueryAsync();
            await connection.CloseAsync();

            return Guid.Parse(parameters["@Id"].ToString());
        }

        public async Task<bool> UpdateDriver(EditDriverCommand driver)
        {
            var query = @"
                UPDATE Driver
                SET FirstName = COALESCE(@FirstName, FirstName),
                    LastName = COALESCE(@LastName, LastName),
                    Email = COALESCE(@Email, Email),
                    PhoneNumber = COALESCE(@PhoneNumber, PhoneNumber),
                    UpdatedDate = @UpdatedDate,
                    UpdatedBy = @UpdatedBy
                WHERE Id = @Id";

            var parameters = new Dictionary<string, object>
            {
                {"@FirstName", driver.FirstName},
                {"@LastName", driver.LastName},
                {"@Email", driver.Email},
                {"@PhoneNumber", driver.PhoneNumber},
                {"@Id", driver.Id.ToString()},
                {"@UpdatedDate", DateTimeOffset.Now.ToString()},
                {"@UpdatedBy", "Test User"}
            };

            return await ExecuteSqlNonQuery(query, parameters) > 0;
        }

        public async Task<bool> IsDriverExist(Guid id)
        {
            var query = "SELECT EXISTS(SELECT 1 FROM Driver WHERE Id = @Id AND IsDeleted = 0)";

            var parameters = new Dictionary<string, object> { { "@Id", id.ToString() } };

            await using var connection = await GetOpenedConnection();
            await using var command = connection.CreateCommand();
            command.CommandText = query;
            AddParametersToCommand(command, parameters);

            var result = await command.ExecuteScalarAsync();
            await connection.CloseAsync();
            return result != null && Convert.ToInt32(result) > 0;
        }

        public async Task<Driver> GetDriver(Guid id)
        {
            var query = "SELECT * FROM Driver WHERE Id = @Id AND IsDeleted = 0";

            var parameters = new Dictionary<string, object> { { "@Id", id.ToString() } };

            await using var connection = await GetOpenedConnection();
            await using var command = connection.CreateCommand();
            command.CommandText = query;
            AddParametersToCommand(command, parameters);

            await using var reader = await command.ExecuteReaderAsync();
            Driver result = null;

            while (await reader.ReadAsync())
                result = MapDriverFromReader(reader);

            await connection.CloseAsync();

            return result;
        }

        public async Task<List<Driver>> GetAllDrivers()
        {
            var query = "SELECT * FROM Driver WHERE IsDeleted = 0";

            await using var connection = await GetOpenedConnection();
            await using var command = connection.CreateCommand();
            command.CommandText = query;

            var drivers = new List<Driver>();
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var driver = MapDriverFromReader(reader);
                drivers.Add(driver);
            }
            await connection.CloseAsync();
            return drivers;
        }

        public async Task<bool> DeleteDriver(Guid id)
        {
            var query = "UPDATE Driver SET IsDeleted = 1, DeletedBy = 'Test User' WHERE Id = @Id";

            var parameters = new Dictionary<string, object> { { "@Id", id.ToString() } };
            var result = await ExecuteSqlNonQuery(query, parameters);
            return result > 0;
        }

        #region private methods
        private async Task<int> ExecuteSqlNonQuery(string query, Dictionary<string, object> parameters)
        {
            await using var connection = await GetOpenedConnection();
            await using var command = connection.CreateCommand();
            command.CommandText = query;
            AddParametersToCommand(command, parameters);
            var result = await command.ExecuteNonQueryAsync();
            await connection.CloseAsync();
            return result;
        }

        private Driver MapDriverFromReader(DbDataReader reader)
        {
            return new Driver
            {
                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber"))
            };
        }

        private void AddParametersToCommand(DbCommand command, Dictionary<string, object> parameters)
        {
            foreach (var (key, value) in parameters)
            {
                var parameter = command.CreateParameter();
                parameter.ParameterName = key;
                parameter.Value = value;
                command.Parameters.Add(parameter);
            }
        }

        private async Task<SqliteConnection> GetOpenedConnection()
        {
            var connection = new SqliteConnection(_configuration.GetConnectionString("Sqlite-Default"));
            await connection.OpenAsync();
            return connection;
        }
        #endregion
    }
}
