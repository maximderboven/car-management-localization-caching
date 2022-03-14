using Distances;
using Insurance.BL;
using Insurance.DAL;
using Insurance.DAL.EF;
using Insurance.Domain;
using Insurance.UI.CA.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using vlr = Resources.ViewLocalizationResources;

#nullable enable
namespace Insurance.UI.CA {
    internal class Program
    {
        private readonly IManager _manager;
        private readonly List<CultureInfo> _supportedCultures = new List<CultureInfo> {
            new CultureInfo ("en-US"),
            new CultureInfo ("fr-FR"),
            new CultureInfo ("nl-BE")
        };

        public static IServiceCollection GetServices () {
            IServiceCollection services = new ServiceCollection ();

            services.AddScoped<IDistanceLocalizer, DistanceLocalizer> ();

            // Old dependency injection
            services.AddDbContext<InsuranceDbContext> ();
            services.AddScoped<IRepository, Repository> ();
            services.AddScoped<IManager, Manager> ();
            services.AddScoped<Program> ();

            return services;
        }

        public Program(IManager manager)
        {
            UpdateCulture (_supportedCultures[0]);
            _manager = manager;
        }

        public static void Main(string[] args)
        {
            var services = GetServices();
            var serviceProvider = services.BuildServiceProvider();
            serviceProvider.GetService<Program> ()?.Run ();
        }

        private static void UpdateCulture (CultureInfo info) {
            Thread.CurrentThread.CurrentCulture = info;
            Thread.CurrentThread.CurrentUICulture = info;
        }

        private void Run()
        {
            byte n;
            do
            {
                Console.WriteLine ();
                Console.WriteLine (@$"- {vlr.Car_Insurance} -");
                Console.WriteLine (vlr.What_would_you_like_to_do);
                Console.WriteLine (@"==========================");
                PrintMenu();
                n = byte.Parse(Console.ReadLine() ?? "-1");
                Console.ResetColor();
                switch (n)
                {
                    case 0:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(vlr.Exit_Message);
                        break;
                    case 1:
                        PrintAllCars();
                        break;
                    case 2:
                        PrintEnumWithIndex();
                        PrintCarsByFuel(int.Parse(Console.ReadLine() ?? "1"));
                        break;

                    case 3:
                        PrintAllDrivers();
                        break;

                    case 4:
                        PrintDriversByDateOrName();
                        break;

                    case 5:
                        AddDriver();
                        break;

                    case 6:
                        AddCar();
                        break;

                    case 7:
                        AddDriverToCar();
                        break;

                    case 8:
                        RemoveDriverFromCar();
                        break;

                    case 9:
                        SwitchCulture();
                        break;

                    default:
                        Console.WriteLine($@"{n} {vlr.Not_A_Valid_Option}");
                        break;
                }
            } while (n != 0);
            //Environment.Exit(0);
            //Auto exit cuz of end program
        }

        private static void PrintMenu()
        {
            Console.WriteLine (vlr.Quit);
            Console.WriteLine (vlr.Show_All_Cars);
            Console.WriteLine (vlr.Show_Cars_By_Fuel);
            Console.WriteLine (vlr.Show_All_Drivers);
            Console.WriteLine (vlr.All_Drivers_Filtered);
            Console.WriteLine (vlr.Add_Driver);
            Console.WriteLine (vlr.Add_Car);
            Console.WriteLine (vlr.Add_Driver_To_Car);
            Console.WriteLine (vlr.Remove_Driver_From_Car);
            Console.WriteLine (vlr.Switch_Culture);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(vlr.Choice);
            Console.ResetColor();
        }

        private void PrintAllCars()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(vlr.All_Cars);
            Console.WriteLine (@"=========");
            foreach (var c in _manager.GetAllCarsWithGarage())
            {
                Console.WriteLine(c.GetInfo());
            }

            Console.ResetColor();
        }

        private static void PrintEnumWithIndex()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(vlr.Fuel + @" (");
            var enums = (Fuel[]) Enum.GetValues(typeof(Fuel));
            for (byte i = 0; i < enums.Length; i++)
            {
                Console.Write (@$"{i + 1}={enums[i]},");
            }

            Console.Write(@"\b): ");
        }

        private void PrintGaragesWithIndex()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            var garages = _manager.GetAllGarages().ToList();
            for (var i = 0; i < garages.Count; i++)
            {
                Console.WriteLine (@$"{i + 1} -> {garages[i].GetInfo()}");
            }
        }

        private void PrintCarsByFuel(int fuelType)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            foreach (var c in _manager.GetCarsBy((Fuel) fuelType - 1))
            {
                Console.WriteLine(c.GetInfo());
            }

            Console.ResetColor();
        }

        private void PrintAllDrivers()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(vlr.All_Drivers);
            Console.WriteLine (@"=========");
            foreach (var d in _manager.GetAllDriversWithCars())
            {
                Console.WriteLine(d.GetInfo(true));
            }

            Console.ResetColor();
        }

        private void PrintDriversByDateOrName()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(vlr.All_Drivers_Filtered);
            Console.WriteLine (@"=========");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(vlr.Enter_part_of_a_name_or_leave_blank_);
            Console.ResetColor();
            string? name = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(vlr.Enter_a_full_date__yyyy_mm_dd__or_leave_blank__);
            Console.ResetColor();
            DateTime.TryParse(Console.ReadLine(), out DateTime date);

            Console.ForegroundColor = ConsoleColor.Green;
            foreach (var d in _manager.GetAllDriversBy(name, date))
            {
                Console.WriteLine(d.GetInfo());
            }

            Console.ResetColor();
        }

        private void AddDriver()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(vlr.Add_a_driver);
            Console.WriteLine (@"=========");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(vlr.FirstName);
            Console.ResetColor();
            var firstname = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(vlr.lastName);
            Console.ResetColor();
            var lastname = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(vlr.Date_Of_Birth);
            Console.ResetColor();
            DateTime.TryParse(Console.ReadLine(), out DateTime dob);
            try
            {
                if (string.IsNullOrEmpty(firstname) || string.IsNullOrEmpty(lastname))
                    throw new Exception();
                _manager.AddDriver(firstname, lastname, dob);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(vlr.Add_Driver_Succes);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(vlr.Add_Driver_Failed);
                Console.WriteLine(e.Message);
            }

            Console.ResetColor();
        }

        private void AddCar () {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine (vlr.Add_a_Car);
            Console.WriteLine (@"=========");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write (@$"{vlr.PurchasePrice} ({vlr.Optional}): ");
            Console.ResetColor ();

            int.TryParse (Console.ReadLine (), out int pprice);

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write (@$"{vlr.Brand}:");
            Console.ResetColor();
            string? brand = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(@$"{vlr.Fuel}:");
            PrintEnumWithIndex();
            Console.ResetColor();
            int.TryParse(Console.ReadLine(), out int fuel);
            Fuel f = (Fuel) fuel - 1;

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(vlr.Amount_Of_Seats);
            Console.ResetColor();
            short.TryParse(Console.ReadLine(), out short amount);

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(vlr.Amount_of_miles_on_count_);
            Console.ResetColor();
            int.TryParse(Console.ReadLine(), out int miles);

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(vlr.AddCar_Choose_garage);
            PrintGaragesWithIndex();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(@$"{vlr.Garage}: ");
            Console.ResetColor();
            int.TryParse(Console.ReadLine(), out int garage);
            var g = _manager.GetAllGarages().ToList()[garage - 1];
            try
            {
                Console.ForegroundColor = ConsoleColor.Green;
                _manager.AddCar(pprice, brand, f, amount, miles, g);
                Console.WriteLine(vlr.Add_Car_Succes);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(vlr.Add_Car_Failed);
                Console.WriteLine(e.Message);
            }

            Console.ResetColor();
        }

        private void AddDriverToCar()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine (vlr.Add_Rental);
            Console.WriteLine (@"=========");
            Console.ResetColor();
            Console.WriteLine(vlr.Which_car_would_you_like_to_add_a_driver_to_);
            foreach (var car in _manager.GetAllCarsWithGarage())
            {
                Console.WriteLine(@$"[{car.NumberPlate}] {car.GetInfo()}");
            }

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(vlr.Enter_Car_ID);
            Console.ResetColor();
            int.TryParse(Console.ReadLine(), out int carid);

            Console.WriteLine(vlr.Which_driver_would_you_like_to_add_to_this_car);
            foreach (var d in _manager.GetAllDriversWithCars())
            {
                Console.Write(@$"[{d.SocialNumber}] {d.GetInfo(false)}");
            }

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(vlr.Please_enter_a_driver_ID);
            Console.ResetColor();
            int.TryParse(Console.ReadLine(), out int driverid);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(vlr.How_much_does_the_rental_cost);
            Console.ResetColor();
            double.TryParse(Console.ReadLine(), out double price);
            try
            {
                _manager.AddRental(new Rental(price, DateTime.Now, DateTime.Now.AddDays(3), _manager.GetCar(carid),
                    _manager.GetDriver(driverid)));
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(vlr.Failed);
                Console.ResetColor();
            }
        }

        private void RemoveDriverFromCar()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(vlr.Remove_Rental);
            Console.WriteLine (@"=========");
            Console.ResetColor();
            Console.WriteLine(vlr.Select_Car_Drive_Removal);
            foreach (var car in _manager.GetAllCarsWithGarage())
            {
                Console.WriteLine(@$"[{car.NumberPlate}] {car.GetInfo()}");
            }

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(vlr.Enter_Car_ID);
            Console.ResetColor();
            int.TryParse(Console.ReadLine(), out int carid);
            Console.WriteLine(vlr.Select_Driver_Car_Removal);
            try
            {
                foreach (var d in _manager.GetDriversOfCar(carid))
                {
                    Console.Write(@$"[{d.SocialNumber}] {d.GetInfo(false)}");
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(vlr.Driver_Not_Exist);
            }

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(vlr.Enter_Car_ID);
            Console.ResetColor();
            int.TryParse(Console.ReadLine(), out int driverid);
            try
            {
                _manager.RemoveRental(driverid, carid);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(vlr.Failed);
            }

            Console.ResetColor();
        }

        private void SwitchCulture () {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine (vlr.Possible_Cultures);
            int index = 0;
            _supportedCultures.ForEach (culture => {
                Console.Write (++index + @"): ");
                Console.WriteLine (vlr.ResourceManager.GetString (culture.Name));
            });
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write (vlr.Select_Culture);
            if (int.TryParse (Console.ReadLine(), out index) && 0 < index && index <= _supportedCultures.Count) {
                Console.ForegroundColor= ConsoleColor.Green;
                UpdateCulture (_supportedCultures [index - 1]);
                Console.Write (vlr.Updated_Culture);
                Console.WriteLine (vlr.ResourceManager.GetString (Thread.CurrentThread.CurrentUICulture.Name));
            } else {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine (vlr.Culture_Invalid);
            }
            Console.ResetColor ();
        }
    }
}