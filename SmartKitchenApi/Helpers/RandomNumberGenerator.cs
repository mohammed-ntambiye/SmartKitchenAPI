using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartKitchenApi.Helpers
{
    public class RandomNumberGenerator : IRandomNumberGenerator
    {
        public RandomNumberGenerator()
        {

        }

       public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
    }
}
