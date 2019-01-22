using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartKitchenApi.Models
{
    public class Orders
    {

        [MaxLength(255)] public string OrderId { get; set; }

         [MaxLength(255)] public string TableNumber { get; set; }

         [MaxLength(255)] public string MenuItemId { get; set; }

         public DateTime TimeStamp { get; set; }

        [MaxLength(255)] public string Extras { get; set; }

        public string ItemName { get; set; }

    }
}
