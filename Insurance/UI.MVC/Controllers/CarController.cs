using System.Linq;
using Insurance.BL;
using Insurance.DAL.EF;
using Insurance.Domain;
using Microsoft.AspNetCore.Mvc;
using UI.MVC.Models;

namespace UI.MVC.Controllers
{
    public class CarController : Controller
    {
        private readonly IManager _manager;

        public CarController(IManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_manager.GetAllCars());
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewData["garages"] = _manager.GetAllGarages().ToList();
            return View();
        }

        [HttpGet]
        public IActionResult Details(int numberplate)
        {
            //ViewBag.drivers = _manager.GetDriversOfCar(numberplate);
            return View(_manager.GetCarWithDrivers(numberplate));
        }

        [HttpPost]
        public IActionResult Add(CarViewModel cvm)
        {
            if (!ModelState.IsValid)
            {
                ViewData["garages"] = _manager.GetAllGarages().ToList();
                return View();
            }
            //Hij pakt de Mileage niet goed omdat er een foute omzetting gebeurd door regio: , & .
            var car = _manager.AddCar(cvm.PurchasePrice, cvm.Brand, cvm.Fuel, cvm.Seats, cvm.Mileage,
                _manager.GetGarage(cvm.Garage));
            return RedirectToAction("Details", "Car", new {numberplate = car.NumberPlate});
        }
    }
}