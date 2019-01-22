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
            var Items = MContext.RestaurantOrders.ToList();
            foreach (var value in Items)
            {

                var Item = MContext.Menu
                      .Where(b => b.ItemId == value.MenuItemId)
                      .FirstOrDefault();

                MenuItem.Add(new Orders()
                {
                    ItemName = Item.Name,
                    TableNumber = value.TableNumber,
                    OrderId = value.OrderId,
                    Extras = value.Extras,
                    MenuItemId = value.MenuItemId
                });

            }
            return Ok(MenuItem);
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
