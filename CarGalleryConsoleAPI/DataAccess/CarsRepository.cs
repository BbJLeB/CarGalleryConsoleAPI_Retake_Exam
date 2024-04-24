using Microsoft.EntityFrameworkCore;
using CarGalleryConsoleAPI.Data.Model;
using CarGalleryConsoleAPI.DataAccess.Contracts;
using CarGalleryConsoleAPI.DataAccess;

namespace CarGalleryConsoleAPI.DataAccess
{
    public class CarsRepository : ICarsRepository
    {
        private readonly CarDbContext context;

        public CarsRepository(CarDbContext context)
        {
            this.context = context;
        }
        public async Task AddCarAsync(Car car)
        {
            await context.Cars.AddAsync(car);
            await context.SaveChangesAsync();
        }

        public async Task DeleteCarAsync(string catalogNumber)
        {
            var car = await context.Cars.FirstOrDefaultAsync(c => c.CatalogNumber == catalogNumber);
            if (car != null)
            {
                context.Cars.Remove(car);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Car>> GetAllCarsAsync()
        {
            var cars = await context.Cars.ToListAsync();
            return cars;
        }

        public async Task<Car> GetCarByCatalogNumberAsync(string catalogNumber)
        {
            var car = await context.Cars.FirstOrDefaultAsync(c => c.CatalogNumber == catalogNumber);
            return car;
        }

        public async Task<IEnumerable<Car>> SearchCarsByModel(string model)
        {
            var car = await context.Cars.Where(c => c.Model == model).ToListAsync();
            return car;
        }

        public async Task UpdateCarAsync(Car car)
        {
            context.Cars.Update(car);
            await context.SaveChangesAsync();
        }
    }
}
