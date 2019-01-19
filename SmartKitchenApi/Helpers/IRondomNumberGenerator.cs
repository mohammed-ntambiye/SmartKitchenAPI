using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartKitchenApi.Helpers
{
   public interface IRandomNumberGenerator
    {
        int RandomNumber(int min, int max);
    }
}
