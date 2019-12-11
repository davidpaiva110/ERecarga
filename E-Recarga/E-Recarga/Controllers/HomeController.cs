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
                var postosAtivos = db.Postos.Where(c => c.EstacaoId == est.EstacaoId && c.Estado == true);
                if (postosAtivos.Count() > 0)
                    estacoesComPostosAtivos.Add(est);
            }
            ViewBag.pesquisa = "";
            return View(estacoesComPostosAtivos);
        }

        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact([Bind(Include = "Nome, Email, Message")] Mensagem mensagem)
        {
            if (ModelState.IsValid)
            {
                db.Mensagens.Add(mensagem);
                db.SaveChanges();
                return RedirectToAction("Contact");
            }
            return View(mensagem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult About(string pesquisa)
        {
            List<Estacao> estacoesComPostosAtivos = new List<Estacao>();
            List<Estacao> allEstacoes = new List<Estacao>();
            var estacoes = db.Estacoes.Include(v => v.RedeProprietaria).Where(v => v.RedeProprietaria.Nome.ToLower().Contains(pesquisa.ToLower()) || v.Cidade.ToLower().Contains(pesquisa.ToLower()) || v.Localizacao.ToLower().Contains(pesquisa.ToLower()));
            if (String.IsNullOrEmpty(pesquisa))
            {
                estacoes = db.Estacoes.Include(v => v.RedeProprietaria);
            }
            foreach (Estacao est in estacoes) allEstacoes.Add(est);
            foreach (Estacao est in allEstacoes)
            {
                var postosAtivos = db.Postos.Where(c => c.EstacaoId == est.EstacaoId && c.Estado == true);
                if (postosAtivos.Count() > 0)
                    estacoesComPostosAtivos.Add(est);
            }
            ViewBag.pesquisa = pesquisa;
            return View(estacoesComPostosAtivos);
        }
    }
}