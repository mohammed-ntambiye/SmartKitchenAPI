using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartKitchenApi.Models
{
    public class Items
    {
        public string ItemName { get; set; }
        public string ItermId { get; set; }
        public int Count { get; set; }
        public CustomiseData Customise { get; set; }
        public bool Iscustomisable { get; set; }
    }
}
