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
        [Required] [MaxLength(255)] public string OrderId { get; set; }

        [Required] [MaxLength(255)] public string TableNumber { get; set; }

        [Required] [MaxLength(255)] public string MenuItemId { get; set; }

        [Required] [MaxLength(255)] public string Extras { get; set; }
    }
}
