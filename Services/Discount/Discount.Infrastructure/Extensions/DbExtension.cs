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
                ApplyMigrations(config);
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

    private static void ApplyMigrations(IConfiguration config)
    {
        var retry = 5;
        while (retry > 0)
        {
            try
            {
                using var connection = new NpgsqlConnection(config.GetValue<string>("DatabaseSettings:ConnectionString"));
                connection.Open();

                using var cmd = new NpgsqlCommand();
                cmd.Connection = connection;

                cmd.CommandText = "DROP TABLE IF EXISTS Coupon";
                cmd.ExecuteNonQuery();

                cmd.CommandText =
                    @"CREATE TABLE Coupon(Id SERIAL PRIMARY KEY, ProductName VARCHAR(500) NOT NULL, Description TEXT, Amount INT)";

                cmd.ExecuteNonQuery();

                cmd.CommandText =
                    "INSERT INTO Coupon(ProductName, Description, Amount) VALUES ('Adidas Ultraboost', 'Shoe Discount', 40)";
                cmd.ExecuteNonQuery();

                cmd.CommandText =
                    "insert into Coupon(ProductName, Description, Amount) values ('Nike Air Max 270', 'Shoe Discount', 50)";
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                retry--;
                if (retry == 0)
                {
                    throw;
                }
                
                Thread.Sleep(2000);
            }
        }
    }
}