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
        [Display(Name = "ID Posto")]
        public int PostoId { get; set; }
        [Required(ErrorMessage = "Tem de especificar o estado do posto!")]
        [Display(Name = "Estado")]
        [DataType(DataType.Text)]
        public bool Estado { get; set; }

        [ForeignKey("Estacao")]
        [Display(Name = "ID Estação")]
        public int EstacaoId { get; set; }
        public Estacao Estacao { get; set; }

        public IList<Reserva> Reservas { get; set; }

        public Posto(bool estado, int estacaoId)
        {
            Estado = estado;
            EstacaoId = estacaoId;
        }

        public Posto()
        {
        }
    }
}