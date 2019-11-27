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
        [Display(Name = "ID Estação")]
        public int ReservaId { get; set; }
        [Required(ErrorMessage = "Tem de especificar a data e hora de início da reserva!")]
        [Display(Name = "Horário Início")]
        [DataType(DataType.DateTime)]
        public DateTime HorarioInicio { get; set; }
        [Required(ErrorMessage = "Tem de especificar a data e hora de fim da reserva!")]
        [Display(Name = "Horário Fim")]
        [DataType(DataType.DateTime)]
        public DateTime HorarioFim { get; set; }

        [ForeignKey("Posto")]
        public int PostoId { get; set; }
        public Posto Posto { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}