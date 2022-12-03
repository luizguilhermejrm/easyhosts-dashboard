using EasyHosts.Dashboard.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EasyHosts.Dashboard.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Código da reserva")]
        public string CodeBooking { get; set; }
        [DisplayName("Nome do hóspede")]
        public int UserId { get; set; }
        [DisplayName("Data de check-in")]
        public DateTime DateCheckin { get; set; }
        [DisplayName("Data de check-out")]
        public DateTime DateCheckout { get; set; }
        [DisplayName("Valor")]
        public decimal ValueBooking { get; set; }
        public virtual User User { get; set; }
        [DisplayName("Nome do quarto")]
        public int BedroomId { get; set; }
        public virtual Bedroom Bedroom { get; set; }

        public BookingStatus Status { get; set; }

    }
}