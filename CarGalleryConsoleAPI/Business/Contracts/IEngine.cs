namespace CarGalleryConsoleAPI.Business.Contracts
{
    public interface IEngine
    {
        Task Run(ICarsManager carsManager);
    }
}
