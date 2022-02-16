using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Insurance.BL;
using Insurance.Domain;
using Microsoft.AspNetCore.Mvc;
using UI.MVC.Models;

namespace UI.MVC.Controllers.Api
{
    [ApiController] //of [FromBody] in de methode params
    [Route("api/[controller]")]
    public class DriversController : Controller
    {
        
        private readonly IManager _manager;

        public DriversController(IManager manager)
        {
            _manager = manager;
        }
        
        // GET
        [HttpGet("dropdown/{id:int}")]
        public IActionResult GetDropdown(int id)
        {
            var responses = _manager.GetCarsWithoutDriver(id);
            if (responses == null || !responses.Any())
                return NoContent();
            var carDTOS = new List<CarDTO>();
            foreach (var response in responses)
            {
                carDTOS.Add(new CarDTO()
                {
                    Brand = response.Brand,
                    NumberPlate = response.NumberPlate,
                    Fuel = response.Fuel.ToString(),
                    Seats = response.Seats,
                    Mileage = response.Mileage,
                    Purchaseprice = response.PurchasePrice
                });
            }
            return Ok(carDTOS);
        }
        
        [HttpGet("cars/{id:int}")]
        public IActionResult GetCars(int id)
        {
            var responses = _manager.GetCarsOfDriver(id);
            if (responses == null || !responses.Any())
                return NoContent();
            var carDTOS = new List<CarDTO>();
            foreach (var response in responses)
            {
                carDTOS.Add(new CarDTO()
                {
                    Brand = response.Brand,
                    NumberPlate = response.NumberPlate,
                    Fuel = response.Fuel.ToString(),
                    Seats = response.Seats,
                    Mileage = response.Mileage,
                    Purchaseprice = response.PurchasePrice
                });
            }
            return Ok(carDTOS);
        }
        
        // PUT:
        [HttpPost]
        public IActionResult Post(RentalDTO response)
        {
            if (!Validator.TryValidateObject(response,new ValidationContext(response),null, true))
                return BadRequest("Error");
            _manager.AddRental(new Rental(response.Price,response.StartDate,response.EndDate,_manager.GetCar(response.NumberPlate),_manager.GetDriver(response.Socialnumber)));
            return Ok();
        }
    }
}