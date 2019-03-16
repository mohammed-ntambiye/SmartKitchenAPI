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
        private readonly ApplicationDbContext _dbContext;
        public CustomiseController(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_dbContext.Customise.ToList());
        }


        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
           var data = _dbContext.Customise
                .Where(b => b.CustomiseId == id);

           return Ok(data);
        }


        [HttpPost]
        public IActionResult Post([FromBody]CustomiseData value)
        {
            if (value == null) return StatusCode(400);
            try
            {
                _dbContext.Database.EnsureCreated();
                _dbContext.Customise.Add(value);
                _dbContext.SaveChanges();
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
    }
}
