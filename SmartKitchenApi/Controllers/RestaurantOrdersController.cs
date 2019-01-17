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
            return MContext.RestaurantOrders.ToList();
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]RestaurantOrdersModel value)
        {
            if (value == null) return StatusCode(400);
            try
            {
                MContext.Database.EnsureCreated();
                MContext.RestaurantOrders.Add(value);
                MContext.SaveChanges();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                return StatusCode(500);
            }
            
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
            if (id == null) return StatusCode(400);
           RestaurantOrdersModel update = new RestaurantOrdersModel() { OrderId = id };
            try
            {
                MContext.RestaurantOrders.Attach(update);
                MContext.RestaurantOrders.Remove(update);
                MContext.SaveChanges();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                return StatusCode(500);
            }
          
            return Ok();

        }
    }
}
