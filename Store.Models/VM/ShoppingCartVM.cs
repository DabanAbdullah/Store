using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Models.VM
{
    public class ShoppingCartVM
    {
        public  IEnumerable<Shoppingcart> shoppingcartlist { get; set; }
        public OrderHeader orderheader { get; set; }
       
    }
}
