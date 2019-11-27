using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_Recarga.Models
{
    public class RedeProprietaria
    {
        [Key]
        public int RedeProprietariaId { get; set; }
        public string Nome { get; set; }
        public string Nif { get; set; }
        public string NumeroCC { get; set; }
        public string TitularCC { get; set; }
        public string CCV { get; set; }

        public IList<Estacao> Estacoes { get; set; }
    }
}