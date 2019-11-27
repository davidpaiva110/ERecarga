using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace E_Recarga.Models
{
    public class Posto
    {
        [Key]
        public int PostoId { get; set; }
        public bool Estado { get; set; }

        [ForeignKey("Estacao")]
        public int EstacaoId { get; set; }
        public Estacao estacao { get; set; }

        public IList<Reserva> Reservas { get; set; }
    }
}