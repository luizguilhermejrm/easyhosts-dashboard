using EasyHosts.Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyHosts.Dashboard.ViewModel
{
    public class DashboardVm
    {
        private Context db = new Context();
        public IEnumerable<User> User { get; set; }
        public IEnumerable<Product> Product { get; set; }
        public IEnumerable<Bedroom> Bedroom { get; set; }
        public IEnumerable<Event> Event{ get; set; }

        public DashboardVm()
        {
            User = db.User.ToList();
            Product = db.Product.ToList();
            Bedroom = db.Bedroom.ToList();
            Event = db.Event.ToList();
        }
    }
}