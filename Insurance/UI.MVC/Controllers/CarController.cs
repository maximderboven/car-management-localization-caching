using System;
using System.Linq;
using System.Threading;
using Distances;
using Insurance.BL;
using Insurance.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using UI.MVC.Models;

namespace UI.MVC.Controllers
{
    public class CarController : Controller
    {
        private readonly IManager _manager;
        private readonly IDistanceLocalizer _distLocalizer;
        private readonly IDoubleParser _doubleParser;

        public CarController(IManager manager, IDistanceLocalizer localizer, IDoubleParser doubleParse)
        {
            _manager = manager;
            _distLocalizer = localizer;
            _doubleParser = doubleParse;
        }

        [HttpGet]
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Any, NoStore = false, VaryByHeader = "X-Culture")]
        public IActionResult Index()
        {
            HttpContext.Response.Headers.Add("X-Culture",Thread.CurrentThread.CurrentUICulture.ToString());
            return View(_manager.GetAllCars());
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewData["garages"] = _manager.GetAllGarages().ToList();
            return View();
        }

        [HttpGet]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Any, NoStore = false, VaryByQueryKeys = new string[] { "numberplate" })]
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

            double distance = _doubleParser.Parse (cvm.Mileage);
            double mileage = _distLocalizer.Delocalize (distance, DistanceUnit.Miles, DistanceUnit.Kilometers);
            mileage = Math.Round (mileage, 2);

            var car = _manager.AddCar(cvm.PurchasePrice, cvm.Brand, cvm.Fuel, cvm.Seats, mileage,
                _manager.GetGarage(cvm.Garage));
            return RedirectToAction("Details", "Car", new {numberplate = car.NumberPlate});
        }
    }
}