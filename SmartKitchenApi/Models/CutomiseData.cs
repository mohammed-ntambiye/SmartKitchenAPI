using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SmartKitchenApi.Models
{
    public class CustomiseData
    {
        [Key] public string CustomiseId { get; set; }

        public int Ketchup { get; set; }
        
        public int Cheese { get; set; }

        public int Mustard { get; set; }

        public int Pickles { get; set; }

        public int Onion { get; set; }
    }
}
