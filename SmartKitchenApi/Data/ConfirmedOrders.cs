using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartKitchenApi.Data
{
    public class ConfirmedOrders
    {
        public int Id { get; set; }

        public string Owner { get; set; }

        public string ItemId { get; set; }

        public int Quantity { get; set; }

        public string BasketId { get; set; }

        public string CustomiseId { get; set; }

        public string OrderId { get; set; }
    }
}
