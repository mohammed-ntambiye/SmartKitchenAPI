using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartKitchenApi;
using SmartKitchenApi.Helpers;

namespace SmartKitchenApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class SmartKitchenController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public SmartKitchenController(ApplicationDbContext context, IRandomNumberGenerator randomNumberGenerator)
        {
            _dbContext = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_dbContext.KitchenUpdates.ToList());
        }

        [HttpGet("single-update")]
        public IActionResult SingleUpdate(string id)
        {
            var result = _dbContext.KitchenUpdates.FirstOrDefault(_ => _.OrderNumber == id);
            if (result == null)
            {
                return new StatusCodeResult(500);
            }

            var singleUpdate = new UpdateModel()
            {
                TreyId = result.TreyId,
                StationCount = result.StationCount,
                OrderNumber = result.OrderNumber
            };

            return Ok(singleUpdate);
        }


        [HttpPost]
        public IActionResult Post([FromBody]UpdateModel value)
        {
            if (value == null) return StatusCode(400);
            try
            {
                _dbContext.Database.EnsureCreated();
                _dbContext.KitchenUpdates.Add(value);
                _dbContext.SaveChanges();
            }
            catch (SqlException exception)
            {
                Console.WriteLine(exception.ToString());
                return StatusCode(500);
            }

            return Accepted();
        }


        [HttpPut]
        public IActionResult Put([FromBody]UpdateModel value)
        {
            //Return bad request if value is null
            if (value == null) return StatusCode(400);
            try
            {
                //Remove unnecessary quotes 
                value.TreyId = value.TreyId.Replace("'","").Substring(6);
                _dbContext.Database.EnsureCreated();
                //Check if  tray id provided matches an order 
                var update = _dbContext.KitchenUpdates
                    .FirstOrDefault(b => b.TreyId == value.TreyId);

                //if station exists and station count doesn't exceed the set limit
                if (update != null && update.StationCount <4)
                {
                    //Update station
                    update.StationCount += value.StationCount;
                    _dbContext.SaveChanges();
                }
            }
            catch (SqlException exception)
            {
                //Log errors 
                Console.WriteLine(exception.ToString());
                return StatusCode(500);
            }
            return Accepted();
        }


        [HttpDelete]
        public IActionResult Delete([FromBody]string id)
        {
            if (id == null) return StatusCode(400);
            var update = new UpdateModel() { TreyId = id };
            try
            {
                _dbContext.KitchenUpdates.Attach(update);
                _dbContext.KitchenUpdates.Remove(update);
                _dbContext.SaveChanges();
            }
            catch (SqlException exception)
            {
                Console.WriteLine(exception.ToString());
                return StatusCode(500);
            }
            return Ok();
        }
    }
}
