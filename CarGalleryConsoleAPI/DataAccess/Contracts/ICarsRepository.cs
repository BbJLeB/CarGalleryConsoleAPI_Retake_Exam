using CarGalleryConsoleAPI.Data.Model;

namespace CarGalleryConsoleAPI.DataAccess.Contracts
{
    public interface ICarsRepository
    {
        Task<Car> GetCarByCatalogNumberAsync(string catalogNumber);
        Task<IEnumerable<Car>> GetAllCarsAsync();
        Task<IEnumerable<Car>> SearchCarsByModel(string model);
        Task AddCarAsync(Car car);
        Task UpdateCarAsync(Car car);
        Task DeleteCarAsync(string catalogNumber);
    }
}
