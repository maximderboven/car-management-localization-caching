using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Insurance.Domain;

namespace Insurance.DAL.EF
{
    internal static class InsuranceInitializer
    {
        private static bool _isInitialized;
        
        public static void Initialize(InsuranceDbContext context, bool rebuild)
        {
            if (_isInitialized)
                return;
            if (rebuild)
                context.Database.EnsureDeleted();
            if (context.Database.EnsureCreated())
                Seed(context);
            _isInitialized = true;
        }
        
        private static DateTime RandomDayAfter(DateTime date)
        {
            var range = (DateTime.Today - date).Days;           
            return date.AddDays(new Random().Next(range));
        }

        private static void Seed(InsuranceDbContext context)
        {
            var d0 = new Driver("Andy", "Kost", new DateTime(1994, 1, 5));
            var d1 = new Driver("Jilles", "Frieling", new DateTime(1983, 5, 17));
            var d2 = new Driver("Luite", "Poel", new DateTime(1958, 10, 12));
            var d3 = new Driver("Caroliene", "Karremans", new DateTime(1945, 8, 7));

            var g0 = new Garage("PSA retail", "Boomsesteenweg 894, 2610 Antwerpen", "038719811");
            var g1 = new Garage("Van Dessel", "Krijgsbaan 51, 2640 Mortsel", "034403236");
            
            var c0 = new Car(null, "Citroen", Fuel.Gas, 4, 49.54, g0);
            var c1 = new Car(10000, "Opel", Fuel.Gas, 6, 100.61, g0);
            var c2 = new Car(null, "Audi", Fuel.Oil, 5, 5000.87, g1);
            var c3 = new Car(35540, "BMW", Fuel.Lpg, 5, 691.8, g1);
            
            //cars toevoegen aan garage voor onderhoud.
            g0.Cars.Add(c0);
            g0.Cars.Add(c1);
            
            g1.Cars.Add(c2);
            g1.Cars.Add(c3);

            context.Garages.Add(g0);
            context.Garages.Add(g1);

            //toevoegen aan context
            context.Cars.Add(c0);
            context.Cars.Add(c1);
            context.Cars.Add(c2);
            context.Cars.Add(c3);

            context.Drivers.Add(d0);
            context.Drivers.Add(d1);
            context.Drivers.Add(d2);
            context.Drivers.Add(d3);

            //autoos toevoegen aan drivers en andersom adhv tussentabel
            context.Rentals.Add(new Rental((new Random().NextDouble() * 999) + 1, DateTime.Now, RandomDayAfter(DateTime.Now), c0, d3));
            
            context.Rentals.Add(new Rental((new Random().NextDouble() * 999) + 1, DateTime.Now, RandomDayAfter(DateTime.Now), c1, d1));
            context.Rentals.Add(new Rental((new Random().NextDouble() * 999) + 1, DateTime.Now, RandomDayAfter(DateTime.Now), c1, d0));
            
            context.Rentals.Add(new Rental((new Random().NextDouble() * 999) + 1, DateTime.Now, RandomDayAfter(DateTime.Now), c2, d0));
            context.Rentals.Add(new Rental((new Random().NextDouble() * 999) + 1, DateTime.Now, RandomDayAfter(DateTime.Now), c2, d2));
            
            context.Rentals.Add(new Rental((new Random().NextDouble() * 999) + 1, DateTime.Now, RandomDayAfter(DateTime.Now), c3, d0));
            context.Rentals.Add(new Rental((new Random().NextDouble() * 999) + 1, DateTime.Now, RandomDayAfter(DateTime.Now), c3, d1));
            context.Rentals.Add(new Rental((new Random().NextDouble() * 999) + 1, DateTime.Now, RandomDayAfter(DateTime.Now), c3, d2));


            context.SaveChanges();
            context.ChangeTracker.Clear();
        }
    }
}