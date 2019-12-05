using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_Recarga.Models
{
    public class EstatisticasDetalhes
    {
        [Display(Name = "Estação")]
        public Estacao Estacao { get; set; }
        [Display(Name = "Posto")]
        public  Posto posto { get; set; }
        [Display(Name = "Tempo de Utilização")]
        public double temposUtilizacao { get; set; }

        public EstatisticasDetalhes(Estacao estacao, Posto postos, double temposUtilizacao)
        {
            Estacao = estacao;
            this.posto = postos;
            this.temposUtilizacao = temposUtilizacao;
        }

        public EstatisticasDetalhes()
        {
        }
    }
}