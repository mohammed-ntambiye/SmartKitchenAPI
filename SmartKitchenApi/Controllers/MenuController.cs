﻿using System;
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
        public IActionResult Get()
        {
            return Ok(MContext.Menu.ToList());
        }

        [HttpGet("get-ordered-items")]
        public IActionResult GetOrderedItems()
        {
            List<Orders> MenuItem = new List<Orders>();
            var listOfMenuItems = new List<Items>();
            var Items = MContext.RestaurantOrders.ToList();

            foreach (var value in Items)
            {
                listOfMenuItems.Clear();

                var basket = MContext.Basket
                      .Where(b => b.Owner == value.Owner && b.BasketId == value.BasketId).ToList();

                foreach (var basketItem in basket)
                {
                    if (value.BasketId == basketItem.BasketId)
                    {
                        var item = MContext.Menu.Where(a => a.ItemId == basketItem.ItemId).FirstOrDefault();

                        listOfMenuItems.Add(new Items()
                        {
                            ItermId = item.ItemId,
                            ItemName = item.Name,
                            Count = basketItem.Quantity                        
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


        [HttpGet("{get-single-item}")]
        public IActionResult GetSingleItem(string id)
        {          
            return Ok(MContext.Menu.Where(b => b.ItemId == id).FirstOrDefault());
        }


        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]string value)
        {
            throw new NotImplementedException();
        }


        [HttpDelete]
        public IActionResult Delete([FromBody]string id)
        {
            if (id == null) return StatusCode(400);
            MenuModel update = new MenuModel() { ItemId = id };
            try
            {
                MContext.Menu.Attach(update);
                MContext.Menu.Remove(update);
                MContext.SaveChanges();
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
