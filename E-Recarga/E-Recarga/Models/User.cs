using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_Recarga.Models
{
    public class User
    {
        [Key]
        [Display(Name = "ID User")]
        public int UserId { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Tem de especificar o nome do User")]
        [DataType(DataType.Text)]
        public string Nome { get; set; }

        [Display(Name = "NIF")]
        [Required(ErrorMessage = "Tem de especificar o NIF do User")]
        [DataType(DataType.Text), MaxLength(9)]
        public string Nif { get; set; }

        [Display(Name = "Número do Cartão de Crédito")]
        [Required(ErrorMessage = "Tem de especificar o número do cartão de crédito")]
        [DataType(DataType.CreditCard), MaxLength(16)]
        public string NumeroCC { get; set; }

        [Display(Name = "Titular Cartão de Crédito")]
        [Required(ErrorMessage = "Tem de especificar o titular do cartão de crédito")]
        [DataType(DataType.Text),]
        public string TitularCC { get; set; }

        [Display(Name = "CCV")]
        [Required(ErrorMessage = "Tem de especificar o CCV do cartão de crédito")]
        [DataType(DataType.Text), MaxLength(9)]
        public string CCV { get; set; }

        public IList<Reserva> Reservas { get; set; }

        public User(string nome, string nif, string numeroCC, string titularCC, string cCV)
        {
            Nome = nome;
            Nif = nif;
            NumeroCC = numeroCC;
            TitularCC = titularCC;
            CCV = cCV;
        }
    }
}