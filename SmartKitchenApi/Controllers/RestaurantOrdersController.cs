using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SmartKitchenApi;
using SmartKitchenApi.Helpers;

namespace SmartKitchenApi.Controllers
{
    [Route("api/[controller]")]
    public class RestaurantOrdersController : ControllerBase, IRestaurantController
    {
        protected ApplicationDbContext MContext;
        protected IRandomNumberGenerator RandomNumberHelper;
        // GET api/values
        public RestaurantOrdersController(ApplicationDbContext _context, IRandomNumberGenerator _randomNumberGenerator)
        {
            MContext = _context;
            RandomNumberHelper = _randomNumberGenerator;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(MContext.RestaurantOrders.ToList());
        }

        [HttpPost]
        public IActionResult Post([FromBody]RestaurantOrdersModel value)
        {
            if (value == null) return StatusCode(400);
            value.TimeStamp = DateTime.Now;
            value.OrderId = RandomNumberHelper.RandomNumber(1, 1000000).ToString();
            try
            {
                MContext.Database.EnsureCreated();
                MContext.RestaurantOrders.Add(value);
                MContext.SaveChanges();
                var orderRecieved = new SmartKitchenModel
                {
                    OrderId = value.OrderId,
                    StationNumber = "Order recieved"
                };
                MContext.KitchenUpdates.Add(orderRecieved);
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