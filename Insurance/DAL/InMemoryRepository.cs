using System;
using System.Collections.Generic;
using System.Linq;
using Insurance.Domain;
#nullable enable
namespace Insurance.DAL
{
    public class InMemoryRepository : IRepository
    {
        private List<Driver> _drivers;
        private List<Car> _cars;
        private List<Garage> _garages;

        public InMemoryRepository()
        {
            //init lists
            _drivers = new List<Driver>();
            _cars = new List<Car>();
            _garages = new List<Garage>();
            Seed();
        }

        public IEnumerable<Car> ReadCarsWithoutDriver(int socialnumber)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Car> IRepository.ReadCarsOfDriver(int socialnumber)
        {
            throw new NotImplementedException();
        }

        public bool ChangeGarage(Garage garage)
        {
            throw new NotImplementedException();
        }

        public Car ReadCarWithDrivers(int numberplate)
        {
            throw new NotImplementedException();
        }

        public Car ReadCar(int numberplate)
        {
            return _cars.Single(car => car.NumberPlate.Equals(numberplate));
        }

        public IEnumerable<Car> ReadAllCars()
        {
            return _cars;
        }

        public IEnumerable<Car> ReadCarsOf(Fuel fuel)
        {
            return _cars.FindAll(car => car.Fuel.Equals(fuel)).AsEnumerable();
        }

        public void CreateCar(Car car)
        {
            if (_cars.Contains(car)) return;
            car.NumberPlate = _cars.Count + 1;
            _cars.Add(car);
        }

        public Driver ReadDriver(int socialnumber)
        {
            return _drivers.Single(driver => driver.SocialNumber.Equals(socialnumber));
        }
        
        public Garage ReadGarage(int id)
        {
            return _garages.Single(d=>d.Id.Equals(id));
        }

        public void CreateGarage(Garage garage)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Driver> ReadAllDrivers()
        {
            return _drivers.AsEnumerable();
        }

        public IEnumerable<Garage> ReadAllGarages()
        {
            return _garages.AsEnumerable();
        }

        public IEnumerable<Driver> ReadDriversBy(string? name, DateTime? dob)
        {
            IQueryable<Driver> filteredList = new EnumerableQuery<Driver>(_drivers);

            if (!String.IsNullOrWhiteSpace(name))
            {
                filteredList = filteredList.Where(d => (d.FirstName + " " + d.LastName).ToLower().Contains(name.ToLower()));
            }
            if (dob.HasValue)
            {
                filteredList = filteredList.Where(d => dob.Equals(d.DateOfBirth.Date));
            }

            return filteredList;
        }

        public void CreateDriver(Driver driver)
        {
            if (_drivers.Contains(driver)) return;
            driver.SocialNumber = _drivers.Count + 1;
            _drivers.Add(driver);
        }

        public IEnumerable<Car> ReadAllCarsWithGarage()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Driver> ReadAllDriversWithCars()
        {
            throw new NotImplementedException();
        }

        public void CreateRental(Rental bookAuthor)
        {
            throw new NotImplementedException();
        }

        public void DeleteRental(int socialnumber, int numberplate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Driver> ReadCarsOfDriver(int numberplate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Driver> ReadDriversOfCar(int numberplate)
        {
            throw new NotImplementedException();
        }

        private void Seed()
        {
            CreateDriver(new Driver("Andy", "Kost", new DateTime(1994, 1, 5)));
            CreateDriver(new Driver("Jilles", "Frieling", new DateTime(1983, 5, 17)));
            CreateDriver(new Driver("Luite", "Poel", new DateTime(1958, 10, 12)));
            CreateDriver(new Driver("Caroliene", "Karremans", new DateTime(1945, 8, 7)));

            var g1 = new Garage("PSA retail", "Boomsesteenweg 894", "+3238719811");
            var g2 = new Garage("Van Dessel", "Mortsel", "+3234403236");
            
            CreateCar(new Car(null, "Citroen", Fuel.Gas, 4, 0, g1));
            CreateCar(new Car(10000, "Opel", Fuel.Gas, 6, 0, g1));
            CreateCar(new Car(null, "Audi", Fuel.Oil, 5, 5000, g2));
            CreateCar(new Car(35540, "BMW", Fuel.Lpg, 5, 6000, g2));

            //cars toevoegen aan garage voor onderhoud.
            g1.Cars.Add(_cars[0]);
            g1.Cars.Add(_cars[1]);
            
            g2.Cars.Add(_cars[2]);
            g2.Cars.Add(_cars[3]);
            
            //toevoegen aan garage
            _garages.Add(g1);
            _garages.Add(g2);

        }
    }
}