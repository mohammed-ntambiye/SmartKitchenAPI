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

        [Required] [MaxLength(255)] public List<Items> Items { get; set; }

        [Required] [MaxLength(255)] public string BaketId { get; set; }

        [Required] [MaxLength(255)] public string Owner { get; set; }

        [Required] [MaxLength(255)] public string TreyId { get; set; }

        public bool Modifications { get; set; }

        public DateTime TimeStamp { get; set; }

        [MaxLength(255)] public string Extras { get; set; }


    }
}
