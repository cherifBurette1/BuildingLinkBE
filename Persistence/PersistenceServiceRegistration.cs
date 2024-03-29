﻿using Application.Contracts.Factories;
using Application.Contracts.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Implementation.Factories.Implementations;
using Persistence.Implementation.Repositories;
using SQLitePCL;

namespace Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration _config)
        {
            services.AddRepositories();
            services.AddFactories();

            AutoCheckSqliteDb(_config);

            return services;
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IDriverRepository, DriverRepository>();
        }

        private static void AddFactories(this IServiceCollection services)
        {
            services.AddTransient<IDriverFactory, DriverFactory>();
        }

        private static void AutoCheckSqliteDb(IConfiguration _config)
        {
            raw.SetProvider(new SQLite3Provider_e_sqlite3());

            if (!File.Exists("DriversDb.db"))
            {
                using var connection = new SqliteConnection(_config.GetConnectionString("Sqlite-Default"));
                connection.Open();
                using var command = new SqliteCommand(@"
                         CREATE TABLE IF NOT EXISTS Driver (
                             Id TEXT PRIMARY KEY,
                             FirstName TEXT,
                             LastName TEXT,
                             Email TEXT,
                             PhoneNumber TEXT,
                             CreatedDate TEXT,
                             CreatedBy TEXT,
                             UpdatedDate TEXT,
                             UpdatedBy TEXT,
                             DeletedDate TEXT,
                             DeletedBy TEXT,
                             IsDeleted INTEGER);", connection);
                command.ExecuteNonQuery();

                InsertSeedData(connection);

                connection.Close();
            }
        }

        private static void InsertSeedData(SqliteConnection connection)
        {
            var seedData = new[]
            {
                new { Id = Guid.NewGuid().ToString(), FirstName = "Sherif", LastName = "ElGazzar", Email = "sherif@gmail.com", PhoneNumber = "123-456-7890", CreatedDate = DateTimeOffset.Now.ToString(), CreatedBy = "System" },
                new { Id = Guid.NewGuid().ToString(), FirstName = "Dina", LastName = "Sidky", Email = "dina@dina.com", PhoneNumber = "987-654-3210", CreatedDate = DateTimeOffset.Now.ToString(), CreatedBy = "System" }
            };
            try
            {
                foreach (var data in seedData)
                {
                    using var insertCommand = connection.CreateCommand();
                    insertCommand.CommandText = @"
            INSERT INTO Driver (Id, FirstName, LastName, Email, PhoneNumber, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy, DeletedDate, DeletedBy, IsDeleted)
            VALUES (@Id, @FirstName, @LastName, @Email, @PhoneNumber, @CreatedDate, @CreatedBy, @UpdatedDate, @UpdatedBy, @DeletedDate, @DeletedBy, @IsDeleted)";
                    insertCommand.Parameters.AddWithValue("@Id", data.Id);
                    insertCommand.Parameters.AddWithValue("@FirstName", data.FirstName);
                    insertCommand.Parameters.AddWithValue("@LastName", data.LastName);
                    insertCommand.Parameters.AddWithValue("@Email", data.Email);
                    insertCommand.Parameters.AddWithValue("@PhoneNumber", data.PhoneNumber);
                    insertCommand.Parameters.AddWithValue("@CreatedDate", data.CreatedDate);
                    insertCommand.Parameters.AddWithValue("@CreatedBy", data.CreatedBy);
                    insertCommand.Parameters.AddWithValue("@UpdatedDate", DBNull.Value);
                    insertCommand.Parameters.AddWithValue("@UpdatedBy", DBNull.Value);
                    insertCommand.Parameters.AddWithValue("@DeletedDate", DBNull.Value);
                    insertCommand.Parameters.AddWithValue("@DeletedBy", DBNull.Value);
                    insertCommand.Parameters.AddWithValue("@IsDeleted", 0);

                    insertCommand.ExecuteNonQuery();
                }
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
