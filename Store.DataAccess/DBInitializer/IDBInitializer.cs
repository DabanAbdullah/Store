using Microsoft.AspNetCore.Identity;
using Store.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DataAccess.DBInitializer
{
    public interface IDBInitializer
    {
      

        void Initialize();
    }
}
