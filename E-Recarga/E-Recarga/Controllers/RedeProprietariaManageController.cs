using E_Recarga.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_Recarga.Controllers
{
    [Authorize(Roles = "Rede Proprietaria")]
    public class RedeProprietariaManageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult ListarPostos()
        {
            return View(db.Postos.OrderBy(c => c.PostoId).ToList());
        }

        public ActionResult ListarEstacoes()
        {
            return View(db.Estacoes.OrderBy(c => c.EstacaoId).ToList());
        }

        public ActionResult NovaEstacao()
        {
            return View();
        }

        public ActionResult NovoPosto()
        {
            string userId = User.Identity.GetUserId();
            ViewBag.Estacoes = new SelectList(db.Estacoes.Where(u => u.RedeProprietariaId.Contains(userId)).ToList(), "EstacaoId", "EstacaoId");
            return View();
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NovoPosto([Bind(Include = "EstacaoId")] Posto posto)
        {
            if (ModelState.IsValid)
            {
                db.Postos.Add(new Posto(false, posto.EstacaoId));
                db.SaveChanges();
                return RedirectToAction("ListarPostos");
            }
            return View(posto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NovaEstacao([Bind(Include = "Cidade,Localizacao,Preco")] Estacao estacao)
        {
            if (ModelState.IsValid)
            {
                string userId = User.Identity.GetUserId();
                Estacao est = new Estacao(estacao.Cidade, estacao.Localizacao, estacao.Preco);
                RedeProprietaria rp = db.RedesProprietarias.FirstOrDefault(s => s.RedeProprietariaId.Contains(userId));
                est.RedeProprietaria = rp;
                db.Estacoes.Add(est);
                db.SaveChanges();
                return RedirectToAction("ListarEstacoes");
            }
            return View(estacao);
        }
    }
}