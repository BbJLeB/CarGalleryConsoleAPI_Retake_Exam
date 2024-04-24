using System.Text.Json;
using CarGalleryConsoleAPI.Business;
using CarGalleryConsoleAPI.Data.Model;

namespace CarGalleryConsoleAPI.DataAccess
{
    public class DatabaseSeeder
    {
        public static async Task SeedDatabaseAsync(CarDbContext context, CarsManager carsManager)
        {
            if (context.Cars.Count() == 0)
            {
                string jsonFilePath = Path.Combine("Data", "Seed", "cars.json");
                string jsonData = File.ReadAllText(jsonFilePath);

                var cars = JsonSerializer.Deserialize<List<Car>>(jsonData);

                if (cars != null)
                {
                    foreach (var car in cars)
                    {
                        if (!context.Cars.Any(c => c.CatalogNumber == car.CatalogNumber))
                        {
                            var newCar = new Car()
                            {
                                Make = car.Make,
                                Availability = car.Availability,
                                Color = car.Color,
                                Mileage = car.Mileage,
                                Model = car.Model,
                                Price = car.Price,
                                CatalogNumber = car.CatalogNumber,
                                Year = car.Year,
                            };
                            await carsManager.AddAsync(newCar);
                        }
                    }

                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
