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
    public class SmartKitchenController : ControllerBase , ISmartKitchenController
    {
        protected ApplicationDbContext MContext;
        protected IRandomNumberGenerator RandomNumberHelper;

        public SmartKitchenController(ApplicationDbContext _context, IRandomNumberGenerator _randomNumberGenerator)
        {
            MContext = _context;
            RandomNumberHelper = _randomNumberGenerator;
        }

        [HttpGet]
        public List<SmartKitchenModel> Get()
        {
            return MContext.KitchenUpdates.ToList();
           
        }

  
        [HttpPost]
        public IActionResult Post([FromBody]SmartKitchenModel value)
        {
            if (value == null) return StatusCode(400);
            value.Id = RandomNumberHelper.RandomNumber(1, 10000000);
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


        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }


        [HttpDelete]
        public IActionResult Delete([FromBody]string id)
        {
            if (id ==null) return StatusCode(400);
            SmartKitchenModel update = new SmartKitchenModel() { OrderId = id };
            try
            {
                MContext.KitchenUpdates.Attach(update);
                MContext.KitchenUpdates.Remove(update);
                MContext.SaveChanges();
            }
            catch (SqlException exception )
            {
                Console.WriteLine(exception.ToString());
                return StatusCode(500);
            }
          
            return Ok();

        }
    }
}
