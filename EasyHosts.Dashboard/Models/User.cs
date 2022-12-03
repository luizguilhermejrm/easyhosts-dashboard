using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EasyHosts.Dashboard.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(200)]
        [DisplayName("Nome")]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        [DisplayName("E-mail")]
        public string Email { get; set; }
        [Required]
        [DisplayName("Senha")]
        public string Password { get; set; }
        [Required]
        [DisplayName("Confirmação de senha")]
        public string ConfirmPassword { get; set; }
        [Required]
        [DisplayName("Status do Usuario")]
        public int Status { get; set; }
        [Required]
        [DisplayName("CPF")]
        public string Cpf { get; set; }
        public string Hash { get; set; }
        [Required]
        [DisplayName("Nome do perfil")]
        public int PerfilId { get; set; }
        public virtual Perfil Perfil { get; set; }
    }
}