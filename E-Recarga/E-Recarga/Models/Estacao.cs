using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace E_Recarga.Models
{
    public class Estacao
    {
        [Key]
        public int EstacaoId { get; set; }
        public string Cidade { get; set; }
        public string Localizacao { get; set; }
        public double Preco { get; set; }

        [ForeignKey("RedeProprietaria")]
        public string RedeProprietariaId { get; set; }
        public RedeProprietaria redeProprietaria { get; set; }

        public IList<Posto> Postos { get; set; }
    }
}