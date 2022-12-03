using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyHosts.Dashboard.Service.Exceptions
{
    public class DbConcurrencyException : ApplicationException
    {
        public DbConcurrencyException(string message) : base(message)
        {

        }
    }

}