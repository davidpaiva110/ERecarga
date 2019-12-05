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
        [Display(Name = "ID Reserva")]
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
        [Display(Name = "ID Posto")]
        public int PostoId { get; set; }
        public Posto Posto { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }

        public Reserva()
        {
        }

        public Reserva(int postoId, DateTime data, DateTime horaInicio, DateTime horaFim)
        {
            Data = data;
            HoraInicio = horaInicio;
            HoraFim = horaFim;
            PostoId = postoId;
        }

        public Reserva(DateTime data, DateTime horaInicio, DateTime horaFim, int postoId, string userId)
        {
            Data = data;
            HoraInicio = horaInicio;
            HoraFim = horaFim;
            PostoId = postoId;
            UserId = userId;
        }
    }
}