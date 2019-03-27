using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartKitchenApi
{
    public class UpdateModel
    {     
        [Key] [Required] [MaxLength(255)] public string TreyId { get; set; }

        [Required] public int StationCount { get; set; }

        [Required] [MaxLength(20)] public string OrderNumber { get; set; }
    }
}