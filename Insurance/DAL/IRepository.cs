using System;
using System.Collections.Generic;
using Insurance.Domain;
#nullable enable

namespace Insurance.DAL
{
    public interface IRepository
    {
        public Car ReadCar(int numberplate);
        public IEnumerable<Car> ReadAllCars();
        public IEnumerable<Car> ReadCarsOf(Fuel fuel);
        public void CreateCar(Car car);
        public void CreateGarage(Garage garage);
        public Driver ReadDriver(int socialnumber);
        public Garage ReadGarage(int id);
        public IEnumerable<Driver> ReadAllDrivers();
        public IEnumerable<Garage> ReadAllGarages();
        public IEnumerable<Driver> ReadDriversBy(string? name, DateTime? dateofbirth);
        public void CreateDriver(Driver driver);
        public IEnumerable<Car> ReadAllCarsWithGarage();
        public IEnumerable<Driver> ReadAllDriversWithCars();
        public void CreateRental(Rental rental);
        public void DeleteRental(int socialnumber, int numberplate);
        public IEnumerable<Driver> ReadDriversOfCar(int numberplate);
        public IEnumerable<Car> ReadCarsWithoutDriver(int socialnumber);
        public IEnumerable<Car> ReadCarsOfDriver(int socialnumber);
        public bool ChangeGarage(Garage garage);
        public Car ReadCarWithDrivers(int numberplate);
    }
}