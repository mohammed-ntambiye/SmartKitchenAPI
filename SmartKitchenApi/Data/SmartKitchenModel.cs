using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartKitchenApi
{
    public class SmartKitchenModel
    {     
        [Key] [Required] [MaxLength(255)] public string TreyId { get; set; }

        [Required] [MaxLength(20)] public string StationId { get; set; }

        [Required] [MaxLength(20)] public string OrderNumber { get; set; }
    }
}