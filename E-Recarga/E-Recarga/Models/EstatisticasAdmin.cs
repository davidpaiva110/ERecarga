using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_Recarga.Models
{
    public class EstatisticasAdmin
    {
        [Display(Name = "Rede Proprietária")]
        public RedeProprietaria RedeProprietaria { get; set; }

        [Display(Name = "Número de Reservas")]
        public int NumeroReservas{ get; set; }

        public EstatisticasAdmin(RedeProprietaria redeProprietaria, int numeroReservas)
        {
            RedeProprietaria = redeProprietaria;
            NumeroReservas = numeroReservas;
        }

        public EstatisticasAdmin()
        {
        }
    }
}