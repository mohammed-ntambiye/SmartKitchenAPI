using System;
using System.Collections.Generic;
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
        protected ApplicationDbContext mContext;
        // GET api/values
        public SmartKitchenController(ApplicationDbContext context)
        {
            mContext = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Accepted();
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]SmartKitchenModel value)
        {
            if (value == null) return StatusCode(404);
            mContext.Database.EnsureCreated();
            if (!mContext.KitchenUpdates.Any()) return StatusCode(404);
            mContext.KitchenUpdates.Add(value);
            mContext.SaveChanges();
            return Accepted();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
