using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Taste.Models
{
    public class FoodType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Food Type")]
        public string Name { get; set; }
    }
}
