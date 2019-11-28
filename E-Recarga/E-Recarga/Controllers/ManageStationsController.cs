using E_Recarga.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace E_Recarga.Controllers
{
    [Authorize(Roles = "Rede Proprietaria")]
    public class ManageStationsController : Controller
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
            ViewBag.Estacoes = new SelectList(db.Estacoes.Where(u => u.RedeProprietaria.AspNetUserId.Contains(userId)).ToList(), "Name", "Name");
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

        public ActionResult NovaEstacao([Bind(Include ="Cidade,Localizacao,Preco")] Estacao estacao)
        {
            if (ModelState.IsValid)
            {
                int id = Int32.Parse(User.Identity.GetUserId());
                db.Estacoes.Add(new Estacao(estacao.Cidade, estacao.Localizacao, estacao.Preco, id));
                db.SaveChanges();
                return RedirectToAction("ListarEstacoes");
            }
            return View(estacao);
        }
    }
}