using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_Recarga.Models
{
    public class Mensagem
    {
        [Key]
        [Display(Name = "ID Mensagem")]
        public int MensagemId { get; set; }
        [Required(ErrorMessage = "Tem de especificar o nome do remetente!")]
        [Display(Name = "Nome")]
        [DataType(DataType.Text)]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Tem de especificar o email do remetente!")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Tem de especificar a mensagem do remetente!")]
        [Display(Name = "Mensagem")]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }
    }
}