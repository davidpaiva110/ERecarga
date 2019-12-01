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
        [Required(ErrorMessage = "Tem de especificar a data da reserva!")]
        [Display(Name = "Data")]
        [DataType(DataType.Date)]
        public DateTime Data { get; set; }
        [Required(ErrorMessage = "Tem de especificar a hora de início da reserva!")]
        [Display(Name = "Hora de Início")]
        [DataType(DataType.Time)]
        public DateTime HoraInicio { get; set; }
        [Required(ErrorMessage = "Tem de especificar a hora de fim da reserva!")]
        [Display(Name = "Hora de Fim")]
        [DataType(DataType.Time)]
        public DateTime HoraFim { get; set; }

        [ForeignKey("Posto")]
        public int PostoId { get; set; }
        public Posto Posto { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }
    }
}