using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        private readonly ApplicationDbContext DBContext;
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


        [HttpGet("{orderNumber}")]
        public IActionResult Get(string orderNumber)
        {
            var confirmedOrders = DBContext.ConfirmedOrders.Where(_ => _.OrderId == orderNumber).ToList();
            var basket = new List<BasketModel>();
            try
            {
                foreach (var value in confirmedOrders)
                {
                    var item = DBContext.Menu
                        .FirstOrDefault(b => b.ItemId == value.ItemId);

                    basket.Add(new BasketModel()
                    {
                        ImageFileName = item.ImageFileName,
                        Price = item.Price,
                        ItemName = item.Name,
                        Quantity = value.Quantity,
                        Owner = value.Owner,
                        ItemId = item.ItemId,
                        BasketId = value.BasketId,
                        OrderNumber = value.OrderId
                    });
                }
            }
            catch (SqlException exception)
            {
                Console.WriteLine(exception.ToString());
                return StatusCode(500);
            }

            return Ok(basket);
        }

        [HttpPost("{toggle-modifications}")]
        public IActionResult Get([FromBody]RestaurantOrdersModel model)
        {
            var order = DBContext.RestaurantOrders.FirstOrDefault(_ => _.OrderId == model.OrderId);
            order.Modifications = model.Modifications;
            DBContext.SaveChanges();

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
                    if (Item == null) continue;
                    DBContext.Basket.Remove(Item);
                    DBContext.SaveChanges();
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
            // Variables of Orders,  a list of menu Items
            var order = new Orders();
            var listOfMenuItems = new List<Items>();
            
            //Retrieves all ordered items from the RestaurantOrders Table
            var orderConfirmation = DBContext.RestaurantOrders.FirstOrDefault(b => b.OrderId == orderNumber);
            //Check for null exceptions 
            if (orderConfirmation == null)
                return BadRequest();
            //Follows the the basketId and OrderId references to the ConfirmedOrders Table
            var basket = DBContext.ConfirmedOrders
                .Where(b => b.Owner == orderConfirmation.Owner 
                            && b.BasketId == orderConfirmation.BasketId
                            && b.OrderId == orderNumber).ToList();
            //Loops through the matched confirmed order, retrieves the items from Menu Table and Customization table
            foreach (var basketItem in basket)
            {
                if (orderConfirmation.BasketId != basketItem.BasketId) continue;
                var item = DBContext.Menu.FirstOrDefault(a => a.ItemId == basketItem.ItemId);
                if (item == null)
                    return BadRequest();

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
            MatchOrderInformation(orderConfirmation,order);  
            order.Items = new List<Items>(listOfMenuItems);  
            //Return all orders
            return Ok(order);
        }


        public Orders MatchOrderInformation(RestaurantOrdersModel orderConfirmation, Orders order)
        {
            order.TimeStamp = orderConfirmation.TimeStamp;
            order.OrderId = orderConfirmation.OrderId;
            order.Extras = orderConfirmation.Extras;
            order.TableNumber = orderConfirmation.TableNumber;
            order.Modifications = orderConfirmation.Modifications;
            order.TreyId = orderConfirmation.TreyId.Substring(orderConfirmation.TreyId.Length - 12);
            order.Owner = orderConfirmation.Owner;
            return order;
        }

        [HttpDelete("{orderId}")]
        public IActionResult Delete(string orderId)
        {
            if (orderId == null) return StatusCode(400);
            var update = new RestaurantOrdersModel() { OrderId = orderId };
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

        [HttpDelete("{orderId}/{itemId}")]
        public IActionResult Delete(string orderId, string itemId)
        {
            if (orderId == null) return StatusCode(400);
            var order = DBContext.ConfirmedOrders.FirstOrDefault(_ => _.OrderId == orderId && _.ItemId == itemId);
            if (order == null)
                return BadRequest();
            try
            {
                DBContext.ConfirmedOrders.Remove(order);
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