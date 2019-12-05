using E_Recarga.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;


namespace E_Recarga.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            List<Estacao> estacoesComPostosAtivos = new List<Estacao>();
            List<Estacao> allEstacoes = new List<Estacao>();
            var estacoes = db.Estacoes.Include(v => v.RedeProprietaria);
            foreach (Estacao est in estacoes) allEstacoes.Add(est);
            foreach (Estacao est in allEstacoes)
            {
                var postosAtivos = db.Postos.Where(c => c.EstacaoId == est.EstacaoId && c.Estado == false);
                if (postosAtivos.Count() > 0)
                    estacoesComPostosAtivos.Add(est);
            }
            return View(estacoesComPostosAtivos);
        }

        public ActionResult Contact()
        {

            return View();
        }
    }
}