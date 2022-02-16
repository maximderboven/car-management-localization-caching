using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Insurance.BL;
using Insurance.Domain;
using Microsoft.AspNetCore.Mvc;
using UI.MVC.Models;

namespace UI.MVC.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class GaragesController : ControllerBase
    {
        private readonly IManager _manager;

        public GaragesController(IManager manager)
        {
            _manager = manager;
        }
        
        // [HttpGet]
        // public IActionResult Get(int? id)
        // {
        //     return id.HasValue ? GetWithId((int)id) : GetAll();
        // }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            var response = _manager.GetGarage(id);
            if (response == null)
                return NoContent();

            var garageDto = new NewGarageDTO()
            {
                Id = response.Id,
                Telnr = response.Telnr,
                Adress = response.Adress,
                Name = response.Name
            };
            
            return Ok(garageDto);
        }
        [HttpGet]
        public IActionResult Get()
        {
            var responses = _manager.GetAllGarages();
            if (responses == null || !responses.Any())
                return NoContent();
            
            var garageDto = new List<NewGarageDTO>();
            foreach (var response in responses)
            {
                garageDto.Add(new NewGarageDTO()
                {
                    Id = response.Id,
                    Telnr = response.Telnr,
                    Adress = response.Adress,
                    Name = response.Name
                });
            }
            
            return Ok(garageDto);
        }
        
        // POST: api/TicketResponses
        [HttpPost]
        public IActionResult Post(NewGarageDTO response)
        {
            var g = _manager.AddGarage(response.Name,response.Adress,response.Telnr);
            //if (g == null)
                //return BadRequest("Error :(");
            return Ok();
        }
        
        // PUT:
        [HttpPut]
        public IActionResult Put(NewGarageDTO garageDTO)
        {
            if (!Validator.TryValidateObject(garageDTO,new ValidationContext(garageDTO),null, true))
                return BadRequest("Error");
            if (!_manager.ChangeGarage(new Garage(garageDTO.Id,garageDTO.Name,garageDTO.Adress,garageDTO.Telnr)))
                return NotFound();
            return Ok();
        }
    }
}