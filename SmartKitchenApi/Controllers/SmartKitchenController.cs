using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartKitchenApi;

namespace SmartKitchenApi.Controllers
{
    [Route("api/[controller]")]
    public class SmartKitchenController : ControllerBase
    {
        protected ApplicationDbContext MContext;
        // GET api/values
        public SmartKitchenController(ApplicationDbContext context)
        {
            MContext = context;
        }

        [HttpGet]
        public List<SmartKitchenModel> Get()
        {
            return MContext.KitchenUpdates.ToList();
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]SmartKitchenModel value)
        {
            if (value == null) return StatusCode(404);
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

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete]
        public IActionResult Delete([FromBody]string id)
        {
            if (id ==null) return StatusCode(404);
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
