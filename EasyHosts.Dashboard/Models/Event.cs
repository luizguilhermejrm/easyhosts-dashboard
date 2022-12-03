using EasyHosts.Dashboard.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EasyHosts.Dashboard.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Nome")]
        public string NameEvent { get; set; }
        [DisplayName("Organizador")]
        public string Organizer { get; set; }
        [DisplayName("Data")]
        public DateTime DateEvent { get; set; }
        [DisplayName("Localização")]
        public string EventsPlace { get; set; }

        [DisplayName("Foto")]
        public byte[] Picture { get; set; }
        [DisplayName("Descrição")]
        public string DescriptionEvent { get; set; }
        [DisplayName("Atrações")]
        public string Attractions { get; set; }
        [DisplayName("Tipo")]
        public EventType TypeEvent { get; set; }
    }
}