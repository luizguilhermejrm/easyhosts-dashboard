using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EasyHosts.Dashboard.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Nome")]
        public string Name { get; set; }
        [Required]
        [DisplayName("Valor")]
        public decimal Value { get; set; }
        [Required]
        [DisplayName("Quantidade do Produto")]
        public int QuantityProduct { get; set; }
    }
}