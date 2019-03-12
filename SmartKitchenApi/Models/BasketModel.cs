using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartKitchenApi.Data
{
    public class BasketModel
    {
        public int Id { get; set; }

        public string Owner { get; set; }

        public string ItemId { get; set; }

        public int Quantity { get; set; }

        public string BasketId { get; set; }

        public string ItemName { get; set; }

        public decimal Price { get; set; }

        public string ImageFileName { get; set; }

        public  string TreyId { get; set; }
    }
}
