using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartKitchenApi.Data
{
    public class Stats
    {
        [Key]
        public int Id { get; set; } 

        public string ItemId { get; set; }

        public int Quantity { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
