using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Discount.Infrastructure.Extensions;

public static class DbExtension
{
    public static IHost MigrateDatabase<TContext>(this IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var config = services.GetRequiredService<IConfiguration>();
            var logger = services.GetRequiredService<ILogger<TContext>>();

            try
            {
                logger.LogInformation("Discount DB Migration Started");
                ApplyMigrations(config, logger);
                logger.LogInformation("Discount DB Migration Complete");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        return host;
    }

    private static void ApplyMigrations(IConfiguration config, ILogger logger)
    {
        var retry = 5;
        while (retry > 0)
        {
            try
            {
                logger.LogInformation("Attempting to connect to PostgreSQL...");
            
                using var connection = new NpgsqlConnection(config.GetValue<string>("DatabaseSettings:ConnectionString"));
                logger.LogInformation("Connection object created.");

                connection.Open();
                logger.LogInformation("Connected to PostgreSQL.");

                using var cmd = new NpgsqlCommand();
                cmd.Connection = connection;

                logger.LogInformation("Dropping existing Coupon table...");
                cmd.CommandText = "DROP TABLE IF EXISTS Coupon";
                cmd.ExecuteNonQuery();

                logger.LogInformation("Creating Coupon table...");
                cmd.CommandText =
                    @"CREATE TABLE Coupon(Id SERIAL PRIMARY KEY, ProductName VARCHAR(500) NOT NULL, Description TEXT, Amount INT)";
                cmd.ExecuteNonQuery();

                logger.LogInformation("Inserting default coupon data...");
                cmd.CommandText =
                    "INSERT INTO Coupon(ProductName, Description, Amount) VALUES ('Adidas Ultraboost', 'Shoe Discount', 40)";
                cmd.ExecuteNonQuery();

                cmd.CommandText =
                    "INSERT INTO Coupon(ProductName, Description, Amount) VALUES ('Nike Air Max 270', 'Shoe Discount', 50)";
                cmd.ExecuteNonQuery();

                logger.LogInformation("Discount DB Migration Completed Successfully.");
                break; // Exit loop if successful
            }
            catch (Exception e)
            {
                logger.LogError($"Error while migrating database: {e.Message}");
                retry--;

                if (retry == 0)
                {
                    throw;
                }

                logger.LogWarning($"Retrying connection in 2 seconds... Attempts left: {retry}");
                Thread.Sleep(2000);
            }
        }
    }

}