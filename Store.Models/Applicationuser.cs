using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Models
{
    public class Applicationuser:IdentityUser
    {
        [Required]
        public string fullname { get; set; }

        public string? StreetAdress { get; set; }
        public string? city { get; set; }
        public string? state { get; set; }
        public string? postalcode { get; set; }

       
    }
}
