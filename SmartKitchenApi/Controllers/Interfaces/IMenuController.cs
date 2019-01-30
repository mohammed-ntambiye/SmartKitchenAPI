using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartKitchenApi.Data;

namespace SmartKitchenApi.Controllers
{
    interface IMenuController
    {
        IActionResult Post([FromBody] MenuModel value);

        IActionResult Get();

        IActionResult Put(int id, [FromBody] string value);

        IActionResult Delete([FromBody] string id);


    }
}
