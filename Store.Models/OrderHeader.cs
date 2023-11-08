using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Store.Models
{
    
    public class OrderHeader
    {

        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public Applicationuser AppUser { get; set; }

        public DateTime Orderdate { get; set; }
        public DateTime Shippingdate { get; set; }

        public double Total { get; set; }

        public string? Orderstatus { get; set; }
        public string? Paymentstatus { get; set; }

        public string? Trackingnumber { get; set; }
        public string? Carrier { get; set; }


        public DateTime Paymentdate { get; set; }
        public DateOnly Paymentduedate { get; set; }


        public string? SessionId { get; set; }
        public string? Paymentintendid { get; set; }


        [Required]
        public string Name { get; set; }
        [Required]
        public string Phonenumber { get; set; }
        [Required]
        public string StreetAdress { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string Postalcode { get; set; }
    }
}
