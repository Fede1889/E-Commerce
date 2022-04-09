using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using Polly;
using System;

namespace Discount.API.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<TContext>(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var configuration = services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILogger<TContext>>();

                try
                {
                    logger.LogInformation("Migrating postresql database.");

                    var retry = Policy.Handle<NpgsqlException>()
                            .WaitAndRetry(
                                retryCount: 5,
                                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), // 2,4,8,16,32 sc
                                onRetry: (exception, retryCount, context) =>
                                {
                                    logger.LogError($"Retry {retryCount} of {context.PolicyKey} at {context.OperationKey}, due to: {exception}.");
                                });

                    //if the postgresql server container is not created on run docker compose this
                    //migration can't fail for network related exception. The retry options for database operations
                    //apply to transient exceptions                    
                    
                    //retry.Execute(() => ExecuteMigrations(configuration));

                    logger.LogInformation("Migrated postresql database.");
                }
                catch (NpgsqlException ex)
                {
                    logger.LogError(ex, "An error occurred while migrating the postresql database");
                }
            }

            return host;
        }

        
        private static void ExecuteMigrations(IConfiguration configuration) {
            using var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            connection.Open();

            using var command = new NpgsqlCommand
            {
                Connection = connection
            };

            command.CommandText = "DROP TABLE IF EXISTS Catalog";
            command.ExecuteNonQuery();

            command.CommandText = @"CREATE TABLE Catalog(Id varchar(24) PRIMARY KEY, 
                                                                Name VARCHAR(50) NOT NULL,
                                                                Category varchar(50),
                                                                Description varchar(100),
                                                                Price real,
                                                                Quantity integer)";
            command.ExecuteNonQuery();

            command.CommandText = "INSERT INTO Catalog(Id,Name, Category, Description, Price, Quantity) VALUES('602d2149e773f2a3990b47f5', 'IPhone X', 'Smart Phone', 'Telefono',950.00,15);";
            command.ExecuteNonQuery();

            command.CommandText = "INSERT INTO Catalog(Id,Name, Category, Description, Price, Quantity) VALUES('602d2149e773f2a3990b47f6', 'Samsung 10', 'Smart Phone', 'Telefono',750.00,8);";
            command.ExecuteNonQuery();

            command.CommandText = "INSERT INTO Catalog(Id,Name, Category, Description, Price, Quantity) VALUES('602d2149e773f2a3990b47f7', 'Huawei Plus', 'Smart Phone', 'Telefono',650.00,3);";
            command.ExecuteNonQuery();

            command.CommandText = "INSERT INTO Catalog(Id,Name, Category, Description, Price, Quantity) VALUES('602d2149e773f2a3990b47f8', 'Xiaomi Mi 9', 'Smart Phone', 'Telefono',584.14,16);";
            command.ExecuteNonQuery();

            command.CommandText = "INSERT INTO Catalog(Id,Name, Category, Description, Price, Quantity) VALUES('602d2149e773f2a3990b47f9', 'HTC U11+ Plus', 'Smart Phone', 'Telefono',314.00,7);";
            command.ExecuteNonQuery();

            command.CommandText = "INSERT INTO Catalog(Id,Name, Category, Description, Price, Quantity) VALUES('602d2149e773f2a3990b47fa', 'Bosch MultiTalent', 'Home Kitchen', 'Robot Cucina',1542.15,9);";
            command.ExecuteNonQuery();

        }
    }
}
