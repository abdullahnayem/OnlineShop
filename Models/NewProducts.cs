using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Models
{
    public class NewProducts
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Product Type")]
        public String NewProduct { get; set; }
    }
}
