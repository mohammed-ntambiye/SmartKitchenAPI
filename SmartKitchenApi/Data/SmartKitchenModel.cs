using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartKitchenApi
{
    public class SmartKitchenModel
    {
        
        [Key] [Required] public int Id { get; set; }

        [Required] [MaxLength(255)] public string OrderId { get; set; }

        [Required] [MaxLength(20)] public string StationNumber { get; set; }

    }
}