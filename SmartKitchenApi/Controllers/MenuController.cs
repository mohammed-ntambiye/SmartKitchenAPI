using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartKitchenApi;
using SmartKitchenApi.Data;
using SmartKitchenApi.Helpers;

namespace SmartKitchenApi.Controllers
{
    [Route("api/[controller]")]
    public class MenuController : ControllerBase, IMenuController
    {
        protected ApplicationDbContext MContext;
        protected IRandomNumberGenerator RandomNumberHelper;

        public MenuController(ApplicationDbContext context, IRandomNumberGenerator _randomNumberGenerator)
        {
            MContext = context;
            RandomNumberHelper = _randomNumberGenerator;
        }

        [HttpGet]
        public List<MenuModel> Get()
        {
            var results = MContext.Menu.ToList();
            return MContext.Menu.ToList();
        }

  
        [HttpPost]
        public IActionResult Post([FromBody]MenuModel value)
        {
            if (value == null) return StatusCode(400);
            value.ItemId = RandomNumberHelper.RandomNumber(10, 200000).ToString();
            try
            {
                MContext.Database.EnsureCreated();
                MContext.Menu.Add(value);
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
            MenuModel update = new MenuModel() { ItemId = id };
            try
            {
                MContext.Menu.Attach(update);
                MContext.Menu.Remove(update);
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
