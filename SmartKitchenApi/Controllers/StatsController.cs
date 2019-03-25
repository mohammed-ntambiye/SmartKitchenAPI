using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using SmartKitchenApi.Data;

namespace SmartKitchenApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Stats")]
    public class StatsController : Controller
    {
        protected ApplicationDbContext DbContext;

        public StatsController(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }
        // GET: api/Stats
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(DbContext.Stats.ToList());
        }

        [HttpGet("filter/{most-popular}")]
        public IActionResult MostPopular()
        {
            var stats = DbContext.Stats.ToList();
            var id = string.Empty;

            foreach (var item in stats)
            {
                id = item.ItemId; 
            }

            return Ok();
        }

        // GET: api/Stats/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }
        
        // POST: api/Stats
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/Stats/5
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
