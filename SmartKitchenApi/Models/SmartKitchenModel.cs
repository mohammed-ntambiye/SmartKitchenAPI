using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartKitchenApi.Models
{
    public class SmartKitchenModel
    {
        public string OrderId { get; set; }

        public string StationId { get; set; }

        public DateTime TimeScanned { get; set; }
    }
}
