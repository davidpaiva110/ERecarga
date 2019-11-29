using E_Recarga.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using System.Net;

namespace E_Recarga.Controllers
{
    [Authorize(Roles = "Rede Proprietaria")]
    public class RedeProprietariaManageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult ListarPostos()
        {
            string userId = User.Identity.GetUserId();
            var postos = db.Postos.Include(r => r.Estacao).Where(r=>r.Estacao.RedeProprietariaId.Contains(userId));
            return View(postos.ToList());
        }

        public ActionResult ListarEstacoes()
        {
            string userId = User.Identity.GetUserId();
            return View(db.Estacoes.Where(c => c.RedeProprietariaId.Contains(userId)).ToList());
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

        public ActionResult EditarPosto()
        {
            return View();
        }

        public ActionResult EditarEstacao()
        {
            return View();
        }

        public ActionResult DetalhesPosto(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Posto posto = db.Postos.Include(r => r.Estacao).SingleOrDefault(r => r.PostoId==id);
            if (posto == null)
            {
                return HttpNotFound();
            }
            return View(posto);
        }

        public ActionResult DetalhesEstacao()
        {
            return View();
        }

        public ActionResult RemoverPosto()
        {
            return View();
        }

        public ActionResult RemoverEstacao()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarPosto([Bind(Include = "Cidade,Localizacao,Preco")] Estacao estacao)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarEstacao([Bind(Include = "Cidade,Localizacao,Preco")] Estacao estacao)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoverPosto([Bind(Include = "Cidade,Localizacao,Preco")] Estacao estacao)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoverEstacao([Bind(Include = "Cidade,Localizacao,Preco")] Estacao estacao)
        {
            return View();
        }
    }
}