﻿using System;
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

namespace SmartKitchenApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class RestaurantOrdersController : ControllerBase
    {
        protected ApplicationDbContext MContext;
        protected IRandomNumberGenerator RandomNumberHelper;
        // GET api/values
        public RestaurantOrdersController(ApplicationDbContext _context, IRandomNumberGenerator _randomNumberGenerator)
        {
            MContext = _context;
            RandomNumberHelper = _randomNumberGenerator;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(MContext.RestaurantOrders.ToList());
        }
   
        [HttpPost]
        public IActionResult Post([FromBody]RestaurantOrdersModel value)
        {
            if (value == null) return StatusCode(400);
            value.TimeStamp = DateTime.Now;
            try
            {
                MContext.Database.EnsureCreated();
                var confirmedItems = MContext.Basket.Where(_ => _.BasketId == value.BasketId && _.Owner == value.Owner).ToList();

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

                    MContext.ConfirmedOrders.Add(itemToAdd);
                    MContext.Stats.Add(stat);

                 
           
                    MContext.SaveChanges();

                    var Item = MContext.Basket
                        .FirstOrDefault(b => b.Owner == item.Owner  && b.BasketId == item.BasketId && b.ItemId == item.ItemId);
                    if (Item != null)
                    {
                        MContext.Basket.Remove(Item);
                        MContext.SaveChanges();
                    }
                }


                MContext.RestaurantOrders.Add(value);
                MContext.SaveChanges();
                var orderReceived = new UpdateModel
                {
                    TreyId = value.TreyId,
                    StationCount = 1,
                    OrderNumber = value.OrderId
                    
                };

                MContext.KitchenUpdates.Add(orderReceived);
                MContext.SaveChanges();

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                return StatusCode(500);
            }
            return Accepted();
        }

        // DELETE api/values/5
        [HttpDelete]
        public IActionResult Delete([FromBody]string id)
        {
            if (id == null) return StatusCode(400);
            RestaurantOrdersModel update = new RestaurantOrdersModel() { OrderId = id };
            try
            {
                MContext.RestaurantOrders.Attach(update);
                MContext.RestaurantOrders.Remove(update);
                MContext.SaveChanges();
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