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
        [Required] [MaxLength(256)] public string OrderId { get; set; }
        [Required] [MaxLength(2048)] public string TableNumber { get; set; }
    }
}
