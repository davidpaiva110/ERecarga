using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_Recarga.Models
{
    public class Estatisticas
    {
        [Display(Name = "Número de Reservas")]
        public int numeroReservas { get; set; }
        public Estacao Estacao { get; set; }

        public Estatisticas(int numeroReservas, Estacao estacao)
        {
            this.numeroReservas = numeroReservas;
            Estacao = estacao;
        }

        public Estatisticas()
        {
        }
    }
}