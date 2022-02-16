using System;
using System.Collections.Generic;
using System.Linq;
using Insurance.Domain;
using Microsoft.EntityFrameworkCore;

#nullable enable
namespace Insurance.DAL.EF
{
    public class Repository : IRepository
    {
        private readonly InsuranceDbContext _context;

        public Repository(InsuranceDbContext ctx)
        {
            _context = ctx;
        }

        public Car ReadCar(int numberplate)
        {
            //return _context.Cars.Find(numberplate);
            return _context.Cars.Single(c => c.NumberPlate.Equals(numberplate));
        }

        public IEnumerable<Car> ReadAllCars()
        {
            return _context.Cars.AsEnumerable();
        }

        public IEnumerable<Car> ReadCarsOf(Fuel fuel)
        {
            return _context.Cars.Include(c => c.Garage).Where(car => car.Fuel.Equals(fuel));
        }

        public void CreateCar(Car car)
        {
            _context.Cars.Add(car);
            _context.SaveChanges();
        }

        public void CreateGarage(Garage garage)
        {
            _context.Garages.Add(garage);
            _context.SaveChanges();
        }

        public Driver ReadDriver(int socialnumber)
        {
            return _context.Drivers.Single(d => d.SocialNumber.Equals(socialnumber));
        }

        public Garage ReadGarage(int id)
        {
            return _context.Garages.Single(d => d.Id.Equals(id));
        }

        public IEnumerable<Driver> ReadAllDrivers()
        {
            return _context.Drivers.AsEnumerable();
        }

        public IEnumerable<Garage> ReadAllGarages()
        {
            return _context.Garages.AsEnumerable();
        }

        public IEnumerable<Driver> ReadDriversBy(string? name, DateTime? dateofbirth)
        {
            IQueryable<Driver> filteredList = _context.Drivers;
            if (!string.IsNullOrEmpty(name))
            {
                filteredList =
                    filteredList.Where(d => (d.FirstName + " " + d.LastName).ToLower().Contains(name.ToLower()));
            }

            if (!dateofbirth.Equals(DateTime.MinValue))
            {
                filteredList = filteredList.Where(d => dateofbirth.Equals(d.DateOfBirth.Date));
            }

            return filteredList;

            /*/return _context.Drivers.Where(d =>
                ((name == "") || (d.FirstName + " " + d.LastName).ToLower().Contains(name.ToLower()))
                && (dateofbirth.Equals(default(DateTime)) || dateofbirth.Equals(d.DateOfBirth.Date))); */
        }

        public void CreateDriver(Driver driver)
        {
            _context.Drivers.Add(driver);
            _context.SaveChanges();
        }

        public IEnumerable<Car> ReadAllCarsWithGarage()
        {
            return _context.Cars.Include(c => c.Garage);
        }

        public IEnumerable<Driver> ReadAllDriversWithCars()
        {
            return _context.Drivers.Include(d => d.Rentals).ThenInclude(r => r.Car);
        }

        public void CreateRental(Rental rental)
        {
            _context.Rentals.Add(rental);
            _context.SaveChanges();
        }

        public void DeleteRental(int socialnumber, int numberplate)
        {
            var rental = _context.Rentals
                .Where(r => r.Car.NumberPlate == numberplate)
                .Single(r => r.Driver.SocialNumber == socialnumber);
            _context.Rentals.Remove(rental);
            _context.SaveChanges();
        }

        public IEnumerable<Driver> ReadDriversOfCar(int numberplate)
        {
            var car = _context.Cars.Include(c => c.Rentals).ThenInclude(r => r.Driver)
                .ThenInclude(d => d.Rentals).Single(c => c.NumberPlate == numberplate);
            return car.Rentals.Select(r => r.Driver).ToList();
        }

        public IEnumerable<Car> ReadCarsOfDriver(int socialnumber)
        {
            var driver = _context.Drivers.Include(d => d.Rentals).ThenInclude(r => r.Car)
                .ThenInclude(c => c.Rentals).Single(d => d.SocialNumber == socialnumber);
            return driver.Rentals.Select(r => r.Car).ToList();
        }

        public bool ChangeGarage(Garage garageDto)
        {
            var garage = ReadGarage(garageDto.Id);
            garage.Name = garageDto.Name;
            garage.Adress = garageDto.Adress;
            garage.Telnr = garageDto.Telnr;
            _context.SaveChanges();
            return true;
        }

        public Car ReadCarWithDrivers(int numberplate)
        {
            return _context.Cars.Include(c => c.Rentals).ThenInclude(r => r.Driver).Single(c => c.NumberPlate.Equals(numberplate));
        }

        public IEnumerable<Car> ReadCarsWithoutDriver(int socialnumber)
        {
            /*var driver = _context.Drivers.Include(d => d.Cars).ThenInclude(r => r.Car)
                .ThenInclude(c => c.Drivers).Single(d => d.SocialNumber != socialnumber);
            return driver.Cars.Select(r => r.Car).ToList();
            //car - rental - driver*/

            /*var allcars = _context.Cars.Include(c => c.Rentals).ThenInclude(r => r.Driver);
            Predicate<Rental> p = r => r.Driver.SocialNumber != socialnumber;
            allcars.Where(c => c.Rentals.Contains());*/

            // var car_all = _context.Cars.Distinct().ToList();
            // for (var i = 0; i < car_all.Count; i++)
            // {
            //     foreach (var car_taken in _context.Rentals.Include(r => r.Car).Include(r => r.Driver)
            //         .Where(r => r.Driver.SocialNumber == socialnumber).Select(r => r.Car))
            //     {
            //         if (car_all[i].NumberPlate == car_taken.NumberPlate)
            //         {
            //             car_all.RemoveAt(i);
            //         }
            //     }
            // }
            //return car_all;

            /* return _context.Rentals.Include(r => r.Car).Include(r => r.Driver)
                .Where(r => r.Driver.SocialNumber != socialnumber).Select(r => r.Car)
                .Concat(_context.Cars.Where(c => c.Rentals.Count == 0)).Distinct();*/

            return _context.Cars.Except(_context.Rentals.Include(r => r.Car).Include(r => r.Driver)
                .Where(r => r.Driver.SocialNumber == socialnumber).Select(r => r.Car));
        }
    }
}