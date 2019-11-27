using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace E_Recarga.Models
{
    public class Reserva
    {
        [Key]
        public int ReservaId { get; set; }
        public DateTime HorarioInicio { get; set; }
        public DateTime HorarioFim { get; set; }

        [ForeignKey("Posto")]
        public int PostoId { get; set; }
        public Posto posto { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public User user { get; set; }
    }
}