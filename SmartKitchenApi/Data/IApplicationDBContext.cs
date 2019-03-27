using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartKitchenApi.Data
{
    interface IApplicationDBContext
    {
         DbSet<RestaurantOrdersModel> RestaurantOrders { get; set; }
         DbSet<UpdateModel> KitchenUpdates { get; set; }
         DbSet<MenuModel> Menu { get; set; }

    }
}
