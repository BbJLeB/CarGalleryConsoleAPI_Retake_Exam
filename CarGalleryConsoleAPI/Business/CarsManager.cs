using System.ComponentModel.DataAnnotations;
using CarGalleryConsoleAPI.Business.Contracts;
using CarGalleryConsoleAPI.Data.Model;
using CarGalleryConsoleAPI.DataAccess.Contracts;

namespace CarGalleryConsoleAPI.Business
{
    public class CarsManager : ICarsManager
    {
        private readonly ICarsRepository repository;

        public CarsManager(ICarsRepository repository)
        {
            this.repository = repository;
        }

        public static Task AddCarAsync(Car car)
        {
            throw new NotImplementedException();
        }

        

        public Task DeleteAsync(string catalogNumber)
        {
            if (string.IsNullOrWhiteSpace(catalogNumber))
            {
                throw new ArgumentException("Catalog number cannot be empty.");
            }

            return repository.DeleteCarAsync(catalogNumber);
        }

        public async Task<IEnumerable<Car>> GetAllAsync()
        {
            var cars = await repository.GetAllCarsAsync();

            if (!cars.Any())
            {
                throw new KeyNotFoundException("No car found.");
            }

            return cars;
        }

        public async Task<Car> GetSpecificAsync(string catalogNumber)
        {
            if (string.IsNullOrWhiteSpace(catalogNumber))
            {
                throw new ArgumentException("Catalog number cannot be empty.");
            }

            var car = await repository.GetCarByCatalogNumberAsync(catalogNumber);

            if (car == null)
            {
                throw new KeyNotFoundException($"No car found with catalog number: {catalogNumber}");
            }

            return car; 
        }

        public async Task<IEnumerable<Car>> SearchByModelAsync(string model)
        {
            if (string.IsNullOrWhiteSpace(model))
            {
                throw new ArgumentException("Car model cannot be empty.");
            }

            var cars = await repository.SearchCarsByModel(model);

            if (cars == null || !cars.Any())
            {
                throw new KeyNotFoundException("No car found with the given model.");
            }

            return cars;
        }

        public async Task UpdateAsync(Car car)
        {
            if (!IsValid(car))
            {
                throw new ValidationException("Invalid car!");
            }

            await repository.UpdateCarAsync(car);
        }

        private bool IsValid(Car car)
        {
            if (car == null)
            {
                return false;
            }

            var validateResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(car);

            if (!Validator.TryValidateObject(car, validationContext, validateResults, true))
            {
                foreach (var validationResult in validateResults)
                {
                    Console.WriteLine($"Validation Error: {validationResult.ErrorMessage}");
                }
                return false;
            }

            return true;
        }
    }
}
