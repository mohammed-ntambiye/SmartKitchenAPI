using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartKitchenApi.Data;
using SmartKitchenApi.Helpers;
using SmartKitchenApi.Models;

namespace SmartKitchenApi.Controllers
{
    //[Produces("application/json")]
    [Route("api/Basket")]
    public class BasketController : Controller
    {
        protected ApplicationDbContext MContext;
        protected IRandomNumberGenerator RandomNumberHelper;

        public BasketController(ApplicationDbContext context, IRandomNumberGenerator _randomNumberGenerator)
        {
            MContext = context;
            RandomNumberHelper = _randomNumberGenerator;
        }


        [HttpGet]
        public IActionResult Get()
        {
            return Ok(MContext.Basket.ToList());
        }


        [HttpPost("get-basket")]
        public IActionResult GetBasket([FromBody]string owner)
        {
            if (owner == null) return StatusCode(400);

            var Basket = new List<BasketModel>();
            var Items = MContext.Basket.Where(_ => _.Owner == owner).ToList();
            try
            {
                foreach (var value in Items)
                {
                    var Item = MContext.Menu
                          .Where(b => b.ItemId == value.ItemId)
                          .FirstOrDefault();

                    Basket.Add(new BasketModel()
                    {
                        ImageFileName = Item.ImageFileName,
                        Price = Item.Price,
                        ItemName = Item.Name,
                        Quantity = value.Quantity,
                        Owner = value.Owner,
                        ItemId = Item.ItemId,
                        BasketId = value.BasketId,
                    });
                }
            }
            catch (SqlException exception)
            {
                Console.WriteLine(exception.ToString());
                return StatusCode(500);
            }
            return Ok(Basket);
        }

        [HttpPost]
        public IActionResult Post([FromBody]BasketData value)
        {
            if (value == null) return StatusCode(400);
            try
            {
                value.Id = 0;
                MContext.Database.EnsureCreated();
                MContext.Basket.Add(value);
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
        public IActionResult Delete(string id,string basketId,string user)
        {
            if (id == null) return StatusCode(400);
            var update = new BasketData() { ItemId = id , BasketId = basketId ,Owner = user};
            try
            {
                var Item = MContext.Basket
                    .Where(b => b.Owner == user && b.BasketId == basketId && b.ItemId == id )
                    .FirstOrDefault();

                if (Item != null)
                {
                    MContext.Basket.Remove(Item);
                    MContext.SaveChanges();
                }
            }      
            catch (SqlException exception)
            {
                Console.WriteLine(exception.ToString());
                return StatusCode(500);
            }

            return Ok();

        }
    }
}
