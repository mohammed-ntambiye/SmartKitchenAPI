using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartKitchenApi.Data
{
    public class MenuModel
    {
        [Required] [MaxLength(255)] public string Name { get; set; }

        [Required] [MaxLength(255)] public string CourseDescription { get; set; }

        [Required] [MaxLength(255)] public string FoodDescription { get; set; }

        [Required] [MaxLength(255)] public string ImageFileName { get; set; }

        [Required] [Key] public string ItemId { get; set; }

        [Required] public decimal Price { get; set; }

        [Required] public bool Customise { get; set; }

        [Required] public string Type { get; set; }

    }
}
