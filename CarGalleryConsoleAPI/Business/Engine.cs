using CarGalleryConsoleAPI.Business.Contracts;
using CarGalleryConsoleAPI.Data.Model;

namespace CarGalleryConsoleAPI.Business
{
    public class Engine : IEngine
    {
        public async Task Run(ICarsManager carsManager)
        {
            bool exitRequested = false;
            while (!exitRequested)
            {
                Console.WriteLine($"{Environment.NewLine}Choose an option:");
                Console.WriteLine("1: Add Car");
                Console.WriteLine("2: Delete Car");
                Console.WriteLine("3: List All Cars");
                Console.WriteLine("4: Update Car");
                Console.WriteLine("5: Find Car by Model");
                Console.WriteLine("X: Exit");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await AddCar(carsManager);
                        break;
                    case "2":
                        await DeleteCar(carsManager);
                        break;
                    case "3":
                        await ListAllCars(carsManager);
                        break;
                    case "4":
                        await UpdateCar(carsManager);
                        break;
                    case "5":
                        await FindCarByModel(carsManager);
                        break;
                    case "X":
                    case "x":
                        exitRequested = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice, please try again.");
                        break;
                }

                static async Task AddCar(ICarsManager carsManager)
                {
                    Console.WriteLine("Adding a new car:");

                    Console.Write("Enter Make: ");
                    var make = Console.ReadLine();

                    Console.Write("Enter Model: ");
                    var model = Console.ReadLine();

                    Console.Write("Enter year of manufacture: ");
                    var year = int.Parse(Console.ReadLine());

                    Console.Write("Enter Color (leave blank for no color): ");

                    var color = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(color))
                    {
                        color = null;
                    }

                    Console.Write("Enter Mileage: ");
                    var mileage = int.Parse(Console.ReadLine());

                    Console.Write("Enter Price: ");
                    var price = decimal.Parse(Console.ReadLine());

                    Console.Write("Enter Car availability (write 'yes' for available or any other value for unavailable status): ");
                    var status = Console.ReadLine();

                    var IsAvailable = false;
                    if (status.ToLower() == "yes")
                    {
                        IsAvailable = true;
                    }
                    else
                    {
                        IsAvailable = false;
                    }


                    Console.Write("Enter Catalog Number: ");
                    var catalogNumber = Console.ReadLine();

                    var newCar = new Car
                    {
                        Year = year,
                        Make = make,
                        Model = model,
                        CatalogNumber = catalogNumber,
                        Color = color,
                        Mileage = mileage,
                        Price = price,
                        Availability = IsAvailable
                    };

                    await carsManager.AddAsync(newCar);
                    Console.WriteLine("Car added successfully.");
                }

                static async Task DeleteCar(ICarsManager carsManager)
                {
                    Console.Write("Enter Catalog Number to delete Car entity from the data: ");
                    string catalogNumber = Console.ReadLine();
                    await carsManager.DeleteAsync(catalogNumber);
                    Console.WriteLine("Car deleted successfully.");
                }

                static async Task ListAllCars(ICarsManager carsManager)
                {
                    var cars = await carsManager.GetAllAsync();
                    if (cars.Any())
                    {
                        foreach (var car in cars)
                        {
                            Console.WriteLine($"Catalog number: {car.CatalogNumber}, Make: {car.Make}, Model: {car.Model}, Year: {car.Year}, Color: {car.Color ?? "unknown"}, Mileage {car.Mileage}, PPrice {car.Price:f2}, Available: {(car.Availability ? "yes" : "no")}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No such a car.");
                    }
                }

                static async Task UpdateCar(ICarsManager carsManager)
                {
                    Console.Write("Enter catalog number of the car to update: ");
                    string catalogNumber = Console.ReadLine();
                    var carToUpdate = await carsManager.GetSpecificAsync(catalogNumber);
                    if (carToUpdate == null)
                    {
                        Console.WriteLine("Car not found.");
                        return;
                    }
                    
                    Console.Write("Enter new Make (leave blank to keep current): ");
                    var make = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(make))
                    {
                        carToUpdate.Make = make;
                    }

                    Console.Write("Enter new Model (leave blank to keep current): ");
                    var model = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(model))
                    {
                        carToUpdate.Model = model;
                    }

                    Console.Write("Enter new year of manufacture (leave blank to keep current): ");

                    if (int.TryParse(Console.ReadLine(), out int year))
                    {
                        carToUpdate.Year = year;
                    }
                    else
                    {
                        Console.WriteLine("Invalid year value! It will keep current value.");
                    }

                    Console.Write("Enter new Color (leave blank to keep current): ");
                    var color = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(color))
                    {
                        carToUpdate.Color = color;
                    }

                    Console.Write("Enter new Mileage value (leave blank to keep current): ");

                    if (int.TryParse(Console.ReadLine(), out int mileage))
                    {
                        carToUpdate.Mileage = mileage;
                    }
                    else
                    {
                        Console.WriteLine("Invalid mileage value! It will keep current value.");
                    }

                    Console.Write("Enter new Price value (leave blank to keep current): ");

                    if (decimal.TryParse(Console.ReadLine(), out decimal price))
                    {
                        carToUpdate.Price = price;
                    }
                    else
                    {
                        Console.WriteLine("Invalid price! It will keep current value.");
                    }

                    Console.Write("Is car available? (enter 'yes', 'no' or leave blank to keep current): ");
                    var status = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(status))
                    {
                        if(status.ToLower() == "yes")
                        {
                            carToUpdate.Availability = true;
                        }
                        else if (status.ToLower() == "no")
                        {
                            carToUpdate.Availability = false;
                        }
                        
                    }

                    await carsManager.UpdateAsync(carToUpdate);
                    Console.WriteLine("Car updated successfully.");
                }

                static async Task FindCarByModel(ICarsManager carsManager)
                {
                    Console.Write("Enter car model: ");
                    string model = Console.ReadLine();
                    var cars = await carsManager.SearchByModelAsync(model);

                    if (cars.Any())
                    {
                        foreach (var car in cars)
                        {
                            Console.WriteLine();
                            Console.WriteLine($"Make: {car.Make}, Model: {car.Model}, Year: {car.Year}, Mileage {car.Mileage}, Color: {car.Color ?? "unknown"}");
                            Console.WriteLine($"--Price: {car.Price:f2}");
                            Console.WriteLine($"---Available: {(car.Availability ? "yes" : "no")}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No car found with the given model.");
                    }
                }
            }
        }
    }
}
