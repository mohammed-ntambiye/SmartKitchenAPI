using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartKitchenApi.Helpers;
using SmartKitchenApi.Models;

namespace SmartKitchenApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Customise")]
    public class CustomiseController : Controller
    {
        protected ApplicationDbContext MContext;
        protected IRandomNumberGenerator RandomNumberHelper;
        public CustomiseController(ApplicationDbContext context, IRandomNumberGenerator _randomNumberGenerator)
        {
            MContext = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(MContext.Customise.ToList());
        }

        // GET: api/Customise/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(string id)
        {
           var data = MContext.Customise
                .Where(b => b.CustomiseId == id);

           return Ok(data);
        }

        // POST: api/Customise
        [HttpPost]
        public IActionResult Post([FromBody]CustomiseData value)
        {
            if (value == null) return StatusCode(400);
            try
            {
                MContext.Database.EnsureCreated();
                MContext.Customise.Add(value);
                MContext.SaveChanges();
            }
            catch (SqlException exception)
            {
                Console.WriteLine(exception.ToString());
                return StatusCode(500);
            }
            return Accepted();
        }

        // PUT: api/Customise/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
