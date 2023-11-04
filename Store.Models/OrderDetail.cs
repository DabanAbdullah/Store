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
    public class OrderDetail
    {
        
        public int Id { get; set; }
        public int Orderheaderid { get; set; }
        [ForeignKey("Orderheaderid")]
        [ValidateNever]
        public OrderHeader Orderheader { get; set; }


        public int Productid { get; set; }
        [ForeignKey("Productid")]
        [ValidateNever]
        public Product Product { get; set; }

        public int count { get; set; }
        public double price { get; set; }



    }
}
