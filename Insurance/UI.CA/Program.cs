﻿using Distances;
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
using System.Text.RegularExpressions;
using System.Threading;
using vlr = Resources.ViewLocalizationResources;

namespace Insurance.UI.CA {
    internal class Program
    {
        private readonly IManager _manager;
        private List<CultureInfo> supportedCultures = new List<CultureInfo> {
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

        private void UpdateCulture (CultureInfo info) {
            Thread.CurrentThread.CurrentCulture = info;
            Thread.CurrentThread.CurrentUICulture = info;
        }

        public Program(IManager manager)
        {
            UpdateCulture (supportedCultures[0]);
            _manager = manager;
        }

        public static void Main(string[] args)
        {
            var services = GetServices();
            var serviceProvider = services.BuildServiceProvider();
            serviceProvider.GetService<Program> ()?.Run ();
        }

        private void Run()
        {
            byte n;
            do
            {
                Console.WriteLine ();
                Console.WriteLine ($"- {vlr.Car_Insurance} -");
                Console.WriteLine (vlr.What_would_you_like_to_do);
                Console.WriteLine ("==========================");
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
            Console.WriteLine("\nAll cars\n=========");
            foreach (var c in _manager.GetAllCarsWithGarage())
            {
                Console.WriteLine(c.GetInfo());
            }

            Console.ResetColor();
        }

        private static void PrintEnumWithIndex()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("Fuel (");
            var enums = (Fuel[]) Enum.GetValues(typeof(Fuel));
            for (byte i = 0; i < enums.Length; i++)
            {
                Console.Write(i + 1 + "=" + enums[i] + ",");
            }

            Console.Write("\b): ");
        }

        private void PrintGaragesWithIndex()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            var garages = _manager.GetAllGarages().ToList();
            for (var i = 0; i < garages.Count; i++)
            {
                Console.Write(i + 1 + " -> " + garages[i].GetInfo() + "\n");
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
            Console.WriteLine("\nAll drivers\n=========");
            foreach (var d in _manager.GetAllDriversWithCars())
            {
                Console.WriteLine(d.GetInfo(true));
            }

            Console.ResetColor();
        }

        private void PrintDriversByDateOrName()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nAll drivers with name and/or date of birth\n=========");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("Enter (part of) a name or leave blank:");
            Console.ResetColor();
            string? name = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("Enter a full date (yyyy/mm/dd) or leave blank: ");
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
            Console.WriteLine("\nAdd a driver\n=========\n");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("firstname: ");
            Console.ResetColor();
            var firstname = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("lastname: ");
            Console.ResetColor();
            var lastname = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("Date of birth (mm/dd/yyyy): ");
            Console.ResetColor();
            DateTime.TryParse(Console.ReadLine(), out DateTime dob);
            try
            {
                if (string.IsNullOrEmpty(firstname) || string.IsNullOrEmpty(lastname))
                    throw new Exception();
                _manager.AddDriver(firstname, lastname, dob);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Added new driver succesfully.");
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Unable to add new driver:");
                Console.WriteLine(e.Message);
            }

            Console.ResetColor();
        }

        private void AddCar()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nAdd a car\n=========\n");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("Purchase Price (optional): ");
            Console.ResetColor();

            int.TryParse(Console.ReadLine(), out int pprice);

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("Brand:");
            Console.ResetColor();
            string brand = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("Fuel:");
            PrintEnumWithIndex();
            Console.ResetColor();
            int.TryParse(Console.ReadLine(), out int fuel);
            Fuel f = (Fuel) fuel - 1;

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("Amount of seats:");
            Console.ResetColor();
            short.TryParse(Console.ReadLine(), out short amount);

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("Amount of miles on count:");
            Console.ResetColor();
            int.TryParse(Console.ReadLine(), out int miles);

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Choose garage:");
            PrintGaragesWithIndex();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("Garage: ");
            Console.ResetColor();
            int.TryParse(Console.ReadLine(), out int garage);
            var g = _manager.GetAllGarages().ToList()[garage - 1];
            try
            {
                Console.ForegroundColor = ConsoleColor.Green;
                _manager.AddCar(pprice, brand, f, amount, miles, g);
                Console.WriteLine("Added new car succesfully.");
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Unable to add new car:");
                Console.WriteLine(e.Message);
            }

            Console.ResetColor();
        }

        private void AddDriverToCar()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nAdd a new rental\n=========\n");
            Console.ResetColor();
            Console.WriteLine("\nWhich car would you like to add a driver to?");
            foreach (var car in _manager.GetAllCarsWithGarage())
            {
                Console.WriteLine(@$"[{car.NumberPlate}] {car.GetInfo()}");
            }

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("\nPlease enter an car ID: ");
            Console.ResetColor();
            int.TryParse(Console.ReadLine(), out int carid);

            Console.Write("Which driver would you like to add to this car?\n");
            foreach (var d in _manager.GetAllDriversWithCars())
            {
                Console.Write(@$"[{d.SocialNumber}] {d.GetInfo(false)}");
            }

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("\nPlease enter a driver ID: ");
            Console.ResetColor();
            int.TryParse(Console.ReadLine(), out int driverid);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("\nHow much does the rental cost: ");
            Console.Write("\nHow much does the rental cost: ");
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
                Console.Write("Something went wrong, please try again.");
                Console.ResetColor();
            }
        }

        private void RemoveDriverFromCar()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Remove a rental\n=========\n");
            Console.ResetColor();
            Console.WriteLine("\nWhich car would you like to remove a driver from?");
            foreach (var car in _manager.GetAllCarsWithGarage())
            {
                Console.WriteLine(@$"[{car.NumberPlate}] {car.GetInfo()}");
            }

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("\nPlease enter an car ID: ");
            Console.ResetColor();
            int.TryParse(Console.ReadLine(), out int carid);
            Console.WriteLine("\nWhich driver would you like to remove from this car?\n");
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
                Console.Write("This driver does not exist. Please try again.");
            }

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("\nPlease enter a driver ID: ");
            Console.ResetColor();
            int.TryParse(Console.ReadLine(), out int driverid);
            try
            {
                _manager.RemoveRental(driverid, carid);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Something went wrong, please try again.");
            }

            Console.ResetColor();
        }

        private void SwitchCulture () {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine (vlr.Possible_Cultures);
            int index = 0;
            supportedCultures.ForEach (culture => {
                Console.Write (++index + @"): ");
                Console.WriteLine (vlr.ResourceManager.GetString (culture.Name));
            });
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write ("Pick a culture: ");
            if (int.TryParse (Console.ReadLine(), out index) && 0 < index && index <= supportedCultures.Count) {
                Console.ForegroundColor= ConsoleColor.Green;
                UpdateCulture (supportedCultures [index - 1]);
                Console.Write ("Updated culture to ");
                Console.WriteLine (vlr.ResourceManager.GetString (Thread.CurrentThread.CurrentUICulture.Name));
            } else {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine ("That's not a valid culture, please try again.");
            }
            Console.ResetColor ();
        }
    }
}