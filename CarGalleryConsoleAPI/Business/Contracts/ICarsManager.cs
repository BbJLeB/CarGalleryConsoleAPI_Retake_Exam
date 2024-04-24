using CarGalleryConsoleAPI.Data.Model;

namespace CarGalleryConsoleAPI.Business.Contracts
{
    public interface ICarsManager
    {
        Task AddAsync(Car Car);
        Task<IEnumerable<Car>> GetAllAsync();
        Task<IEnumerable<Car>> SearchByModelAsync(string model);
        Task<Car> GetSpecificAsync(string catalogNumber);
        Task UpdateAsync(Car Car);
        Task DeleteAsync(string catalogNumber);
    }
}
