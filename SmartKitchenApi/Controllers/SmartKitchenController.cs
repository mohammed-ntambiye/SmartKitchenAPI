using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartKitchenApi;
using SmartKitchenApi.Helpers;

namespace SmartKitchenApi.Controllers
{
    [Route("api/[controller]")]
    public class SmartKitchenController : ControllerBase
    {
        protected ApplicationDbContext MContext;
        protected IRandomNumberGenerator RandomNumberHelper;

        public SmartKitchenController(ApplicationDbContext _context, IRandomNumberGenerator _randomNumberGenerator)
        {
            MContext = _context;
            RandomNumberHelper = _randomNumberGenerator;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(MContext.KitchenUpdates.ToList());
        }

        [HttpGet("single-update")]
        public IActionResult SingleUpdate(string id)
        {
            var result = MContext.KitchenUpdates.FirstOrDefault(_ => _.OrderNumber == id);
            if (result == null)
            {
                return new StatusCodeResult(500);
            }
            var SingleUpdate = new SmartKitchenModel()
            {
                TreyId = result.TreyId,
                StationId = result.StationId,
                OrderNumber = result.OrderNumber
                
            };

            return Ok(SingleUpdate);
        }


        [HttpPost]
        public IActionResult Post([FromBody]SmartKitchenModel value)
        {
            if (value == null) return StatusCode(400);
            try
            {
                MContext.Database.EnsureCreated();
                MContext.KitchenUpdates.Add(value);
                MContext.SaveChanges();
            }
            catch (SqlException exception)
            {
                Console.WriteLine(exception.ToString());
                return StatusCode(500);
            }

            return Accepted();
        }
        [HttpPut]
        public IActionResult Put([FromBody]SmartKitchenModel value)
        {
            if (value == null) return StatusCode(400);
            try
            {
                value.TreyId = value.TreyId.Replace("'","").Substring(6);
                MContext.Database.EnsureCreated();
                var Update = MContext.KitchenUpdates
                    .FirstOrDefault(b => b.TreyId == value.TreyId);

                if (Update == null)
                {
                    MContext.KitchenUpdates.Add(value);
                    MContext.SaveChanges();
                }
                else
                {
                    Update.StationId = value.StationId;
                    MContext.SaveChanges();
                }
            }
            catch (SqlException exception)
            {
                Console.WriteLine(exception.ToString());
                return StatusCode(500);
            }

            return Accepted();
        }

        [HttpDelete]
        public IActionResult Delete([FromBody]string id)
        {
            if (id == null) return StatusCode(400);
            SmartKitchenModel update = new SmartKitchenModel() { TreyId = id };
            try
            {
                MContext.KitchenUpdates.Attach(update);
                MContext.KitchenUpdates.Remove(update);
                MContext.SaveChanges();
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
