using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartKitchenApi;
using SmartKitchenApi.Models;
using SmartKitchenApi.Data;
using SmartKitchenApi.Helpers;
namespace SmartKitchenApi.Controllers
{
    [Route("api/[controller]")]
    public class MenuController : ControllerBase
    {
        protected ApplicationDbContext DbContext;
        protected IRandomNumberGenerator RandomNumberHelper;

        public MenuController(ApplicationDbContext context, IRandomNumberGenerator _randomNumberGenerator)
        {
            DbContext = context;
            RandomNumberHelper = _randomNumberGenerator;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(DbContext.Menu.ToList());
        }

        // GET: api/Menu/5
        [HttpGet("filter/{id}")]
        public IActionResult FilterMenu(string id) { 
            return Ok(DbContext.Menu
                .Where(b => b.Type == id));
        }

        [HttpGet("get-ordered-items")]
        public IActionResult GetOrderedItems()
        {
            List<Orders> MenuItem = new List<Orders>(); 
            var listOfMenuItems = new List<Items>();
            var Items = DbContext.RestaurantOrders.ToList();

            foreach (var value in Items)
            {
                listOfMenuItems.Clear();

                var basket = DbContext.ConfirmedOrders
                      .Where(b => b.Owner == value.Owner && b.BasketId == value.BasketId && b.OrderId == value.OrderId).ToList();

                foreach (var basketItem in basket)
                {
                    if (value.BasketId == basketItem.BasketId)
                    {
                        var item = DbContext.Menu.FirstOrDefault(a => a.ItemId == basketItem.ItemId);
                        var customInfo = DbContext.Customise.FirstOrDefault(_ => _.CustomiseId == basketItem.CustomiseId);

                        listOfMenuItems.Add(new Items()
                        {
                            ItermId = item.ItemId,
                            ItemName = item.Name,
                            Count = basketItem.Quantity,
                            Customise = customInfo,
                            Iscustomisable = item.Customise                           
                        });
                    }

                }

                MenuItem.Add(new Orders()
                {
                    TimeStamp = value.TimeStamp,
                    OrderId = value.OrderId,
                    Extras = value.Extras,
                    Items = new List<Items>( listOfMenuItems),
                    TableNumber = value.TableNumber   
                });
            }

            List<Orders> SortedList = MenuItem.OrderBy(o => o.TimeStamp).ToList();
            return Ok(SortedList);
        }


        [HttpPost]
        public IActionResult Post([FromBody]MenuModel value)
        {
            if (value == null) return StatusCode(400);
            value.ItemId = RandomNumberHelper.RandomNumber(10, 200000).ToString();
            try
            {
                DbContext.Database.EnsureCreated();
                DbContext.Menu.Add(value);
                DbContext.SaveChanges();
            }
            catch (SqlException exception)
            {
                Console.WriteLine(exception.ToString());
                return StatusCode(500);
            }
            return Accepted();
        }


        [HttpGet("{get-single-item}")]
        public IActionResult GetSingleItem(string id)
        {          
            return Ok(DbContext.Menu.FirstOrDefault(b => b.ItemId == id));
        }


        [HttpPut]
        public IActionResult Put(string id, [FromBody]MenuModel value)
        {
            if (value == null) return StatusCode(400);
            try
            {
                DbContext.Database.EnsureCreated();
                var Update = DbContext.Menu
                    .FirstOrDefault(b => b.ItemId == id);

                if (Update != null)
                {
                    Update.Customise = value.Customise;
                    Update.Name = value.Name;
                    Update.CourseDescription = value.CourseDescription;
                    Update.Price = value.Price;
                    Update.Type = value.Type;
                    Update.ImageFileName = value.ImageFileName;
                    DbContext.SaveChanges();
                }
            }
            catch (SqlException exception)
            {
                Console.WriteLine(exception.ToString());
                return StatusCode(500);
            }

            return Accepted();
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            if (id == null) return StatusCode(400);
            MenuModel update = new MenuModel() { ItemId = id };
            try
            {
                DbContext.Menu.Attach(update);
                DbContext.Menu.Remove(update);
                DbContext.SaveChanges();
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
