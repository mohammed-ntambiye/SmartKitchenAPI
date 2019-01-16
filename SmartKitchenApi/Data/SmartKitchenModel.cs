using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartKitchenApi
{
    public class SmartKitchenModel
    {
        [Key]
        [Required] [MaxLength(256)] public string OrderId { get; set; }

        [Required] [MaxLength(2048)] public string StationId { get; set; }

    }
}