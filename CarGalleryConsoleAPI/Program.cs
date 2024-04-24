using CarGalleryConsoleAPI.Business;
using Microsoft.EntityFrameworkCore;
using CarGalleryConsoleAPI.DataAccess;

namespace CarGalleryConsoleAPI
{
    public class Program
    {
        public static async Task Main (string[] args)
        {
            try
            {
                var engine = new Engine();

                var contextFactory = new CarDbContextFactory();

                using var context = contextFactory.CreateDbContext(args);

                await context.Database.MigrateAsync();

                var carsRepository = new CarsRepository(context);
                var carsManager = new CarsManager(carsRepository);

                await DatabaseSeeder.SeedDatabaseAsync(context, carsManager);

                await engine.Run(carsManager);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured: {ex.Message}");
            }
        }
    }
}
