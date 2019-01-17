using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SmartKitchenApi;
using SmartKitchenApi.Models;

namespace SmartKitchenApi.Controllers
{
    [Route("api/[controller]")]
    public class RestaurantOrdersController : ControllerBase
    {
        protected ApplicationDbContext MContext;
        // GET api/values
        public RestaurantOrdersController(ApplicationDbContext context)
        {
            MContext = context;
        }

        [HttpGet]
        public List<RestaurantOrdersModel> Get()
        {
    
            MContext.Database.EnsureCreated();
            MContext.SaveChanges();

            return MContext.RestaurantOrders.ToList();
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]RestaurantOrdersModel value)
        {
            if (value == null) return StatusCode(404);
           // MContext.Database.Migrate();
            MContext.Database.EnsureCreated();
            //if (!MContext.RestaurantOrders.Any()) return StatusCode(404);
           MContext.RestaurantOrders.Add(value);
            MContext.SaveChanges();
            return Accepted();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete]
        public IActionResult Delete([FromBody]string id)
        {
            RestaurantOrdersModel update = new RestaurantOrdersModel() { OrderId = id };
            MContext.RestaurantOrders.Attach(update);
            MContext.RestaurantOrders.Remove(update);
            MContext.SaveChanges();
            return Ok();

        }
    }
}
