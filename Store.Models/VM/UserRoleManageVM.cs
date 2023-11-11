using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Models.VM
{
    public class UserRoleManageVM
    {
        public Applicationuser appuser { get; set; }
        public IEnumerable<SelectListItem> companlist { get; set; }
        public IEnumerable<SelectListItem> rolelist { get; set; }

       
    }
}
