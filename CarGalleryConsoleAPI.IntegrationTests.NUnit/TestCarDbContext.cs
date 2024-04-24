using CarGalleryConsoleAPI.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CarGalleryConsoleAPI.IntegrationTests.NUnit
{
    public class TestCarDbContext : CarDbContext
    {
        public TestCarDbContext()
            : base(new ConfigurationBuilder().AddInMemoryCollection().Build())
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase("TestDatabase");
            }
        }
    }
}
