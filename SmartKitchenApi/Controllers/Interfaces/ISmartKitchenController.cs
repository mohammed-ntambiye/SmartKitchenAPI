using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SmartKitchenApi.Controllers
{
    interface ISmartKitchenController
    {
        IActionResult Get();

        IActionResult Post([FromBody] SmartKitchenModel value);

        IActionResult Put([FromBody]SmartKitchenModel value);

        IActionResult Delete([FromBody] string id);
    }
}
