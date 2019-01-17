using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartKitchenApi.Models
{
    public class Order
    {
        public List<string> _mainCourse = new List<string>();
        public List<string> _sides = new List<string>();
        public List<string> _drinks = new List<string>();
    }
}
