using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyHosts.Dashboard.Models.Enums
{
    public enum BedroomStatus : int
    {
        Disponível = 0,
        Ocupado = 1,
        Manutenção = 2,
        Reservado = 3
    }
}