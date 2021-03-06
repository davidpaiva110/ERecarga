﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_Recarga.Models
{
    public class Estacao
    {
        [Key]
        [Display(Name ="ID Estação")]
        public int EstacaoId { get; set; }
        [Required(ErrorMessage ="Tem de especificar a cidade da estação!")]
        [Display(Name ="Cidade")]
        [DataType(DataType.Text)]
        public string Cidade { get; set; }
        [Required(ErrorMessage = "Tem de especificar a localização da estação!")]
        [Display(Name = "Localização")]
        [DataType(DataType.Text)]
        public string Localizacao { get; set; }
        [Required(ErrorMessage = "Tem de especificar o preço por carregamento da estação!")]
        [Display(Name = "Preço por Carregamento")]
        [Remote("Positivo", "RedeProprietariaManage", ErrorMessage = "O preço deve ser positivo")]
        public double Preco { get; set; }

        [ForeignKey("RedeProprietaria")]
        public string RedeProprietariaId { get; set; }
        public RedeProprietaria RedeProprietaria { get; set; }

        public IList<Posto> Postos { get; set; }

        public Estacao()
        {
        }

        public Estacao(string cidade, string localizacao, double preco)
        {
            Cidade = cidade;
            Localizacao = localizacao;
            Preco = preco;
        }

        
    

    }
}