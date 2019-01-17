using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using SmartKitchenApi.Models;

namespace SmartKitchenApi
{
    public class RestaurantOrdersModel
    {
        [Key]
        public string OrderNumber { get; set; }
        public string TableNumber { get; set; }
        //public List<Order> Order = new List<Order>();
    }
}
