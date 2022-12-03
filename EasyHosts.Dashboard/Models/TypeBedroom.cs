using EasyHosts.Dashboard.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EasyHosts.Dashboard.Models
{
    public class TypeBedroom
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        [DisplayName("Nome")]
        public string NameTypeBedroom { get; set; }
        [Required]
        [DisplayName("Quantidade de pessoas")]
        public int AmountOfPeople { get; set; }
        [Required]
        [DisplayName("Quantidade de camas")]
        public int AmountOfBed { get; set; }

        [Required]
        [DisplayName("Comodidades")]
        public TypeBedroomClass ApartmentAmenities { get; set; }
    }
}