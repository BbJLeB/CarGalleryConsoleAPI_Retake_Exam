using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using CarGalleryConsoleAPI.Data.Model;

namespace CarGalleryConsoleAPI.DataAccess
{
    public class CarDbContext : DbContext
    {
        public virtual DbSet<Car> Cars { get; set; }


        public CarDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public CarDbContext(IConfigurationRoot configurationRoot)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
