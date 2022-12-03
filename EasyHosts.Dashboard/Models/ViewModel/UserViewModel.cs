using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EasyHosts.Dashboard.Models
{
    public class Register
    {
        [Required]
        [MaxLength(255)]
        [DisplayName("Nome")]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        [DisplayName("E-mail")]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [RegularExpression("((?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{6,12})", ErrorMessage = "A senha deve conter ao menos uma letra maiúscula, minúscula e um número. Deve ser no mínimo 6 caractéres.")]
        [DisplayName("Senha")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password")]
        [DisplayName("Confirmação de senha")]
        public string ConfirmPassword { get; set; }
        [DisplayName("CPF")]
        public string Cpf { get; set; }
    }

    public class Access
    {
        [Required]
        [EmailAddress]
        [DisplayName("E-mail")]
        public string Email { get; set; }
        [Display(Name = "Senha")]
        [DataType(DataType.Password)]
        [RegularExpression("((?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{6,12})", ErrorMessage = "A senha deve conter aos menos uma letra maiúscula, minúscula e um número. Deve ser no mínimo 6 caractéres.")]
        public string Password { get; set; }
    }

    public class Message
    {
        [EmailAddress]
        [Required]
        [DisplayName("E-mail")]
        public string Email { get; set; }
        [Required]
        [DisplayName("Assunto")]
        public string Subject { get; set; }
        [DataType(DataType.MultilineText)]
        [DisplayName("Mensagem")]
        public string BodyMessage { get; set; }
    }

    public class ForgotPassword
    {
        [EmailAddress]
        [Required]
        [DisplayName("E-mail")]
        public string Email { get; set; }
    }

    public class ResetPassword
    {
        [DisplayName("E-mail")]
        public string Email { get; set; }
        public string Hash { get; set; }
        [DataType(DataType.Password)]
        [DisplayName("Senha")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password")]
        [DisplayName("Confirmação de senha")]
        public string ConfirmPassword { get; set; }
    }

}