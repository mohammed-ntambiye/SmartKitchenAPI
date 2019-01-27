﻿using System;
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


        [HttpGet("get-basket/{owner}")]
        public IActionResult GetBasket(string owner)
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
        public IActionResult Delete([FromBody]string id)
        {
            if (id == null) return StatusCode(400);
            BasketData update = new BasketData() { ItemId = id };
            try
            {
                MContext.Basket.Attach(update);
                MContext.Basket.Remove(update);
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