using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SmartKitchenApi.Controllers
{
    interface IRestaurantController
    {
        List<RestaurantOrdersModel> Get();

        IActionResult Post([FromBody] RestaurantOrdersModel value);

        void Put(int id, [FromBody] string value);

        IActionResult Delete([FromBody] string id);
    }
}
