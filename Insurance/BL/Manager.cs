#nullable enable
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Insurance.Domain;
using Insurance.DAL;

namespace Insurance.BL
{
    public class Manager : IManager
    {
        private readonly IRepository _repo;
        public Manager(IRepository repo)
        {
            _repo = repo;
        }

        public Car GetCar(int numberplate)
        {
            return _repo.ReadCar(numberplate);
        }

        public IEnumerable<Car> GetAllCars()
        {
            return _repo.ReadAllCars();
        }

        public IEnumerable<Car> GetCarsBy(Fuel fuel)
        {
            return _repo.ReadCarsOf(fuel);
        }

        public Car AddCar(long? purchasePrice, string brand, Fuel fuel, short seats, double mileage, Garage garage)
        {
            if (mileage < 0) throw new ValidationException("Miles need to be positive");
            var car = new Car(purchasePrice, brand, fuel, seats, mileage, garage);
            ValidateCar(car);
            _repo.CreateCar(car);
            return car;
        }

        public Driver GetDriver(int socialnumber)
        {
            return _repo.ReadDriver(socialnumber);
        }
        
        public Garage GetGarage(int id)
        {
            return _repo.ReadGarage(id);
        }

        public IEnumerable<Driver> GetAllDrivers()
        {
            return _repo.ReadAllDrivers();
        }

        public IEnumerable<Driver> GetAllDriversBy(string? name, DateTime? dateofbirth)
        {
            return _repo.ReadDriversBy(name,dateofbirth);
        }

        public IEnumerable<Garage> GetAllGarages()
        {
            return _repo.ReadAllGarages();
        }

        public Driver AddDriver(string firstName, string lastName,DateTime dateOfBirth)
        {
            var driver = new Driver(firstName,lastName,dateOfBirth);
            ValidateDriver(driver);
            _repo.CreateDriver(driver);
            return driver;
        }

        public IEnumerable<Car> GetCarsWithoutDriver(int socialnumber)
        {
            return _repo.ReadCarsWithoutDriver(socialnumber);
        }

        public IEnumerable<Car> GetCarsOfDriver(int socialnumber)
        {
            return _repo.ReadCarsOfDriver(socialnumber);
        }

        public bool ChangeGarage(Garage garage)
        {
            return _repo.ChangeGarage(garage);
        }

        public Car GetCarWithDrivers(int numberplate)
        {
            return _repo.ReadCarWithDrivers(numberplate);
        }

        public Garage AddGarage(string name, string adress, string telnr)
        {
            var garage = new Garage(name, adress, telnr);
            _repo.CreateGarage(garage);
            return garage;
        }

        public IEnumerable<Car> GetAllCarsWithGarage()
        {
            return _repo.ReadAllCarsWithGarage();
        }

        public IEnumerable<Driver> GetAllDriversWithCars()
        {
            return _repo.ReadAllDriversWithCars();
        }

        public void AddRental(Rental rental)
        {
            _repo.CreateRental(rental);
        }

        public void RemoveRental(int socialnumber, int numberplate)
        {
            _repo.DeleteRental(socialnumber,numberplate);
        }

        public IEnumerable<Driver> GetDriversOfCar(int numberplate)
        {
            return _repo.ReadDriversOfCar(numberplate);
        }

        //Validation: Car
        private void ValidateCar(Car c)
        {
            ICollection<ValidationResult> errors = new Collection<ValidationResult>();
            var valid = Validator.TryValidateObject(c, new ValidationContext(c), errors, validateAllProperties: true);
            StringBuilder errormessage = new("");
            foreach (var error in errors)
            {
                errormessage.Append(error).Append(Environment.NewLine);
            }
            if (!valid)
                throw new ValidationException(errormessage.ToString());
        }
        
        //Validation: Driver
        private void ValidateDriver(Driver d)
        {
            ICollection<ValidationResult> errors = new Collection<ValidationResult>();
            var valid = Validator.TryValidateObject(d, new ValidationContext(d), errors, validateAllProperties: true);
            StringBuilder errormessage = new("");
            foreach (var error in errors)
            {
                errormessage.Append(error).Append(Environment.NewLine);
            }

            if (!valid)
                throw new ValidationException(errormessage.ToString());
        }
    }
}