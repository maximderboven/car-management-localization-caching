using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Insurance.BL;
using Insurance.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using UI.MVC.Models;

namespace UI.MVC.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class GaragesController : ControllerBase
    {
        private IDistributedCache _distributedCache;
        private readonly IManager _manager;

        public GaragesController(IManager manager,IDistributedCache distributedCache)
        {
            _manager = manager;
            _distributedCache = distributedCache;
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
        public async Task<IEnumerable<Garage>> GetGarages()
        {
            // Vind Cache Item
            byte[] objectFromCache = await _distributedCache.GetAsync("GaragesKEY");

            if (objectFromCache != null)
            {
                // Deserialize
                var jsonToDeserialize = System.Text.Encoding.UTF8.GetString(objectFromCache);
                var cachedResult = JsonSerializer.Deserialize<IEnumerable<Garage>>(jsonToDeserialize);
                if (cachedResult != null)
                {
                    return cachedResult;
                }
            }

            // Niet gevonden; opnieuw ophalen
            var result = _manager.GetAllGarages();
            
            byte[] objectToCache = JsonSerializer.SerializeToUtf8Bytes(result);
            var cacheEntryOptions = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromDays(1))
                .SetAbsoluteExpiration(TimeSpan.FromHours(10));

            // Cache it
            await _distributedCache.SetAsync("GaragesKEY", objectToCache, cacheEntryOptions);

            return result;
        }
        
        
        // [HttpGet]
        // //[ResponseCache(Duration = 15, Location = ResponseCacheLocation.Any)]
        // public IActionResult Get()
        // {
        //     var responses = GetGarages().Result;
        //     if (responses == null || !responses.Any())
        //         return NoContent();
        //     
        //     var garageDto = new List<NewGarageDTO>();
        //     foreach (var response in responses)
        //     {
        //         garageDto.Add(new NewGarageDTO()
        //         {
        //             Id = response.Id,
        //             Telnr = response.Telnr,
        //             Adress = response.Adress,
        //             Name = response.Name
        //         });
        //     }
        //     
        //     return Ok(garageDto);
        // }
        
        // POST:
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