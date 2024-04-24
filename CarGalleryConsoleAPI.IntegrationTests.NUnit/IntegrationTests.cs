using CarGalleryConsoleAPI.Business;
using CarGalleryConsoleAPI.Business.Contracts;
using CarGalleryConsoleAPI.Data.Model;
using CarGalleryConsoleAPI.DataAccess;
using System.ComponentModel.DataAnnotations;
using Moq;
using CarGalleryConsoleAPI.DataAccess.Contracts;
using CarGalleryConsoleAPI.Business;
using Microsoft.EntityFrameworkCore;

namespace CarGalleryConsoleAPI.IntegrationTests.NUnit
{
    internal class IntegrationTests
    {
        private TestCarDbContext dbContext;
        private ICarsManager carsManager;

        [SetUp]
        public void SetUp()
        {
            this.dbContext = new TestCarDbContext();
            this.carsManager = new CarsManager(new CarsRepository(this.dbContext));
        }

        [TearDown]
        public void TearDown()
        {
            this.dbContext.Database.EnsureDeleted();
            this.dbContext.Dispose();
        }

        //positive test
        [Test]
        public async Task AddCarAsync_ShouldAddNewCar()
        {
            // Arrange
            var newCar = new Car
            {
                CatalogNumber = "ABCD7894EFGH",
                Make = "Toyota",
                Model = "Camry",
                Color = "Blue",
                Year = 2022,
                Mileage = 5000,
                Price = 25000,
                Availability = true
            };

            // Act
            await this.carsManager.AddAsync(newCar);

            var car = await dbContext.Cars.FirstOrDefaultAsync(p => p.CatalogNumber == newCar.CatalogNumber);

            // Assert
            Assert.That(car, Is.Not.Null);
            Assert.That(car.CatalogNumber, Is.EqualTo(newCar.CatalogNumber));
            Assert.That(car.Make, Is.EqualTo(newCar.Make));
            Assert.That(car.Model, Is.EqualTo(newCar.Model));
            Assert.That(car.Color, Is.EqualTo(newCar.Color));
            Assert.That(car.Year, Is.EqualTo(newCar.Year));
            Assert.That(car.Mileage, Is.EqualTo(newCar.Mileage));
            Assert.That(car.Price, Is.EqualTo(newCar.Price));
            Assert.That(car.Availability, Is.EqualTo(true));
        }

        //Negative test
        [Test]
        public async Task AddCarAsync_TryToAddCarWithInvalidCredentials_ShouldThrowException()
        {
            // Arrange

            // Act
            var exception = Assert.ThrowsAsync<ValidationException>(() => this.carsManager.AddAsync(new Car
            {
            }));



            // Assert
            Assert.That(exception.Message, Is.EqualTo("Invalid car!"));

        }

        [Test]
        public async Task DeleteCarAsync_WithValidCatalogNumber_ShouldRemoveCarFromDb()
        {
            // Arrange
            var newCar = new Car
            {
                CatalogNumber = "ABCD7894EFGH",
                Make = "Toyota",
                Model = "Camry",
                Color = "Blue",
                Year = 2022,
                Mileage = 5000,
                Price = 25000,
                Availability = true
            };

            // Act
            await this.carsManager.DeleteAsync(newCar.CatalogNumber);

            // Assert
            Assert.That(dbContext.Cars.Count(), Is.EqualTo(0));
            Assert.That(await dbContext.Cars.FirstOrDefaultAsync(p => p.CatalogNumber == newCar.CatalogNumber), Is.Null);
        }

        [TestCase("")]
        [TestCase("    ")]
        [TestCase(null!)]
        public async Task DeleteCarAsync_TryToDeleteWithNullOrWhiteSpaceCatalogNumber_ShouldThrowException(string nullOrWhitespaceNumber)
        {
            // Arrange

            // Act
            var exception = Assert.ThrowsAsync<ArgumentException>(() => carsManager.DeleteAsync(nullOrWhitespaceNumber));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("Catalog number cannot be empty."));
        }

        [Test]
        public async Task GetAllAsync_WhenCarsExist_ShouldReturnAllCars()
        {
            // Arrange
            var newCar = new Car
            {
                CatalogNumber = "ABCD7894EFGH",
                Make = "Toyota",
                Model = "Camry",
                Color = "Blue",
                Year = 2022,
                Mileage = 5000,
                Price = 25000,
                Availability = true
            };
            await this.carsManager.AddAsync(newCar);

            // Act
            var result = await this.carsManager.GetAllAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().CatalogNumber, Is.EqualTo(newCar.CatalogNumber));
            Assert.That(result.First().Make, Is.EqualTo(newCar.Make));
            Assert.That(result.First().Model, Is.EqualTo(newCar.Model));
            Assert.That(result.First().Color, Is.EqualTo(newCar.Color));
            Assert.That(result.First().Year, Is.EqualTo(newCar.Year));
            Assert.That(result.First().Mileage, Is.EqualTo(newCar.Mileage));
            Assert.That(result.First().Price, Is.EqualTo(newCar.Price));
            Assert.That(result.First().Availability, Is.EqualTo(true));
        }

        [Test]
        public async Task GetAllAsync_WhenNoCarsExist_ShouldThrowKeyNotFoundException()
        {
            // Arrange

            // Act
            var exception = Assert.ThrowsAsync<KeyNotFoundException>(() => carsManager.GetAllAsync());

            // Assert
            Assert.That(exception.Message, Is.EqualTo("No car found."));
        }

        [Test]
        public async Task SearchByModelAsync_WithExistingModel_ShouldReturnMatchingCars()
        {
            // Arrange
            var newCar = new Car
            {
                CatalogNumber = "ABCD7894EFGH",
                Make = "Toyota",
                Model = "Camry",
                Color = "Blue",
                Year = 2022,
                Mileage = 5000,
                Price = 25000,
                Availability = true
            };
            await this.carsManager.AddAsync(newCar);

            // Act
            var result = await this.carsManager.SearchByModelAsync(newCar.Model);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().CatalogNumber, Is.EqualTo(newCar.CatalogNumber));
            Assert.That(result.First().Make, Is.EqualTo(newCar.Make));
            Assert.That(result.First().Model, Is.EqualTo(newCar.Model));
            Assert.That(result.First().Color, Is.EqualTo(newCar.Color));
            Assert.That(result.First().Year, Is.EqualTo(newCar.Year));
            Assert.That(result.First().Mileage, Is.EqualTo(newCar.Mileage));
            Assert.That(result.First().Price, Is.EqualTo(newCar.Price));
            Assert.That(result.First().Availability, Is.EqualTo(true));
        }

        [Test]
        public async Task SearchByModelAsync_WithNonExistingModel_ShouldThrowKeyNotFoundException()
        {
            // Arrange

            // Act
            var exception = Assert.ThrowsAsync<KeyNotFoundException>(() => carsManager.SearchByModelAsync("Moskvich"));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("No car found with the given model."));
        }

        [Test]
        public async Task GetSpecificAsync_WithValidCatalogNumber_ShouldReturnCar()
        {
            // Arrange
            var newCar = new Car
            {
                CatalogNumber = "ABCD7894EFGH",
                Make = "Toyota",
                Model = "Camry",
                Color = "Blue",
                Year = 2022,
                Mileage = 5000,
                Price = 25000,
                Availability = true
            };
            await this.carsManager.AddAsync(newCar);

            // Act
            var result = await this.carsManager.GetSpecificAsync(newCar.CatalogNumber);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.CatalogNumber, Is.EqualTo(newCar.CatalogNumber));
            Assert.That(result.Make, Is.EqualTo(newCar.Make));
            Assert.That(result.Model, Is.EqualTo(newCar.Model));
            Assert.That(result.Color, Is.EqualTo(newCar.Color));
            Assert.That(result.Year, Is.EqualTo(newCar.Year));
            Assert.That(result.Mileage, Is.EqualTo(newCar.Mileage));
            Assert.That(result.Price, Is.EqualTo(newCar.Price));
            Assert.That(result.Availability, Is.EqualTo(true));
        }

        [Test]
        public async Task GetSpecificAsync_WithInvalidCatalogNumber_ShouldThrowKeyNotFoundException()
        {
            // Arrange

            // Act
            var exception = Assert.ThrowsAsync<KeyNotFoundException>(() => carsManager.GetSpecificAsync("ABCD7894"));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("No car found with catalog number: ABCD7894"));
        }

        [Test]
        public async Task UpdateAsync_WithValidCar_ShouldUpdateCar()
        {
            // Arrange
            var newCar = new Car
            {
                CatalogNumber = "ABCD7894EFGH",
                Make = "Toyota",
                Model = "Camry",
                Color = "Blue",
                Year = 2022,
                Mileage = 5000,
                Price = 25000,
                Availability = true
            };
            await this.carsManager.AddAsync(newCar);

            // Act
            var updatedCar = await dbContext.Cars.FirstOrDefaultAsync(p => p.CatalogNumber == newCar.CatalogNumber);
            updatedCar.Color = "Red";

            // Assert
            Assert.That(updatedCar, Is.Not.Null);
            Assert.That(updatedCar.CatalogNumber, Is.EqualTo(newCar.CatalogNumber));
            Assert.That(updatedCar.Make, Is.EqualTo(newCar.Make));
            Assert.That(updatedCar.Model, Is.EqualTo(newCar.Model));
            Assert.That(updatedCar.Color, Is.EqualTo("Red"));
            Assert.That(updatedCar.Year, Is.EqualTo(newCar.Year));
            Assert.That(updatedCar.Mileage, Is.EqualTo(newCar.Mileage));
            Assert.That(updatedCar.Price, Is.EqualTo(newCar.Price));
            Assert.That(updatedCar.Availability, Is.EqualTo(true));
        }

        [Test]
        public async Task UpdateAsync_WithInvalidCar_ShouldThrowValidationException()
        {
            // Arrange

            // Act
            var exception = Assert.ThrowsAsync<ValidationException>(() => carsManager.UpdateAsync(new Car()));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("Invalid car!"));
        }

    }
}
