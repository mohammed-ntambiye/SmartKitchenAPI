using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SmartKitchenApi;
using SmartKitchenApi.Data;
using SmartKitchenApi.Helpers;
using SmartKitchenApi.Models;

namespace SmartKitchenApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class RestaurantOrdersController : ControllerBase
    {
        protected ApplicationDbContext DBContext;
        protected IRandomNumberGenerator RandomNumberHelper;
        // GET api/values
        public RestaurantOrdersController(ApplicationDbContext _context, IRandomNumberGenerator _randomNumberGenerator)
        {
            DBContext = _context;
            RandomNumberHelper = _randomNumberGenerator;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(DBContext.RestaurantOrders.ToList());
        }

        [HttpPost]
        public IActionResult Post([FromBody]RestaurantOrdersModel value)
        {
            if (value == null) return StatusCode(400);
            value.TimeStamp = DateTime.Now;
            try
            {
                DBContext.Database.EnsureCreated();
                var confirmedItems = DBContext.Basket.Where(_ => _.BasketId == value.BasketId && _.Owner == value.Owner).ToList();

                foreach (var item in confirmedItems)
                {
                    var itemToAdd = new ConfirmedOrders()
                    {
                        CustomiseId = item.CustomiseId,
                        Quantity = item.Quantity,
                        Owner = item.Owner,
                        BasketId = item.BasketId,
                        ItemId = item.ItemId,
                        OrderId = value.OrderId

                    };

                    var stat = new Stats()
                    {
                        ItemId = itemToAdd.ItemId,
                        Quantity = itemToAdd.Quantity,
                        Timestamp = DateTime.Now
                    };

                    DBContext.ConfirmedOrders.Add(itemToAdd);
                    DBContext.Stats.Add(stat);
                    DBContext.SaveChanges();

                    var Item = DBContext.Basket
                        .FirstOrDefault(b => b.Owner == item.Owner && b.BasketId == item.BasketId && b.ItemId == item.ItemId);
                    if (Item != null)
                    {
                        DBContext.Basket.Remove(Item);
                        DBContext.SaveChanges();
                    }
                }


                DBContext.RestaurantOrders.Add(value);
                DBContext.SaveChanges();
                var orderReceived = new UpdateModel
                {
                    TreyId = value.TreyId,
                    StationCount = 1,
                    OrderNumber = value.OrderId

                };

                DBContext.KitchenUpdates.Add(orderReceived);
                DBContext.SaveChanges();

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                return StatusCode(500);
            }
            return Accepted();
        }

        [HttpGet("single-order/{orderNumber}")]
        public IActionResult GetSingleOrder(string orderNumber)
        {
            var order = new Orders();
            var listOfMenuItems = new List<Items>();
            var Items = DBContext.RestaurantOrders.FirstOrDefault(b => b.OrderId == orderNumber);

            var basket = DBContext.ConfirmedOrders
                .Where(b => b.Owner == Items.Owner && b.BasketId == Items.BasketId && b.OrderId == orderNumber).ToList();

            foreach (var basketItem in basket)
            {
                if (Items.BasketId != basketItem.BasketId) continue;
                var item = DBContext.Menu.FirstOrDefault(a => a.ItemId == basketItem.ItemId);
                var customInfo = DBContext.Customise.FirstOrDefault(_ => _.CustomiseId == basketItem.CustomiseId);

                listOfMenuItems.Add(new Items()
                {
                    ItermId = item.ItemId,
                    ItemName = item.Name,
                    Count = basketItem.Quantity,
                    Customise = customInfo,
                    Iscustomisable = item.Customise
                });
            }

            order.TimeStamp = Items.TimeStamp;
            order.OrderId = Items.OrderId;
            order.Extras = Items.Extras;
            order.Items = new List<Items>(listOfMenuItems);
            order.TableNumber = Items.TableNumber;
            order.TreyId = Items.TreyId.Substring(Items.TreyId.Length - 12);     
            return Ok(order);
        }


        // DELETE api/values/5
        [HttpDelete]
        public IActionResult Delete([FromBody]string id)
        {
            if (id == null) return StatusCode(400);
            RestaurantOrdersModel update = new RestaurantOrdersModel() { OrderId = id };
            try
            {
                DBContext.RestaurantOrders.Attach(update);
                DBContext.RestaurantOrders.Remove(update);
                DBContext.SaveChanges();
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