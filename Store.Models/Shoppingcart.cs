using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Models
{
    public class Shoppingcart
    {
        [Key]
        public int Id { get; private set; }

        public int productId { get;set; }
        [ForeignKey("productId")]
        [ValidateNever]
        public Product product { get; set; }

        [Range(0, 100,ErrorMessage ="enter a range between 1 to 1000")]
        public int count {  get; set; }

        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public Applicationuser AppUser { get; set; }

        [NotMapped]
       public double price { get; set; }

    }
}
