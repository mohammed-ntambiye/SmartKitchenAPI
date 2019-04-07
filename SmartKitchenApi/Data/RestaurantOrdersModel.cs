using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SmartKitchenApi
{
    public class RestaurantOrdersModel
    {
        [Key]
        [Required] [MaxLength(255)] public string OrderId { get; set; }

        [Required] [MaxLength(255)] public string TableNumber { get; set; }

        [Required] [MaxLength(255)] public string BasketId { get; set; }

        [Required] [MaxLength(255)] public string Owner { get; set; }

        [Required] [MaxLength(255)] public string TreyId { get; set; }

        public bool Modifications { get; set; }

        public DateTime TimeStamp { get; set; }

        [Required] [MaxLength(255)] public string Extras { get; set; }
    }
}



