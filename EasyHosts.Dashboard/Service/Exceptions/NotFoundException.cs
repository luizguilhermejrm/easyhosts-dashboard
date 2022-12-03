using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyHosts.Dashboard.Service.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string message) : base(message)
        {

        }
    }
}