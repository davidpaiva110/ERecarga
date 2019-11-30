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

        public ActionResult EditarPosto(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Posto posto = db.Postos.Find(id);
            if (posto == null)
            {
                return HttpNotFound();
            }
            string userId = User.Identity.GetUserId();
            ViewBag.Estacoes = new SelectList(db.Estacoes.Where(u => u.RedeProprietariaId.Contains(userId)).ToList(), "EstacaoId", "EstacaoId");
            return View(posto);
        }

        public ActionResult EditarEstacao(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estacao estacao = db.Estacoes.Find(id);
            if (estacao == null)
            {
                return HttpNotFound();
            }
            return View(estacao);
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

        public ActionResult DetalhesEstacao(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estacao estacao = db.Estacoes.Find(id);
            if (estacao == null)
            {
                return HttpNotFound();
            }
            return View(estacao);
        }

        public ActionResult RemoverPosto(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Posto posto = db.Postos.Include(r => r.Estacao).SingleOrDefault(r => r.PostoId == id);
            if (posto == null)
            {
                return HttpNotFound();
            }
            return View(posto);
        }

        public ActionResult RemoverEstacao(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estacao estacao = db.Estacoes.Find(id);
            if (estacao == null)
            {
                return HttpNotFound();
            }
            return View(estacao);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarPosto([Bind(Include = "PostoId,Estado,EstacaoId")] Posto posto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(posto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ListarPostos");
            }
            return View(posto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarEstacao([Bind(Include = "EstacaoId,Cidade,Localizacao,Preco, RedeProprietariaId")] Estacao estacao)
        {
            if (ModelState.IsValid)
            {
                db.Entry(estacao).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ListarEstacoes");
            }
            return View(estacao);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoverPosto(int id)
        {
            Posto posto = db.Postos.Include(r => r.Estacao).SingleOrDefault(r => r.PostoId == id);

            var reservasAssociadas = db.Reservas.Where(c => c.PostoId == id);
            if (reservasAssociadas.Count() > 0)
            {
                ModelState.AddModelError("", "Não é possível apagar postos com reservas associadas!");
                return View(posto); //Não permite apagar postos com alguma reserva associada!
            }

            db.Postos.Remove(posto);
            db.SaveChanges();
            return RedirectToAction("ListarPostos");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoverEstacao(int id)
        {
            Estacao estacao = db.Estacoes.Find(id);

            var postosAssociados = db.Postos.Where(c => c.EstacaoId == id);
            if (postosAssociados.Count() > 0)
            {
                ModelState.AddModelError("", "Não é possível apagar estações com postos associados!");
                return View(estacao); //Não permite apagar estações com algum posto associado!
            }

            db.Estacoes.Remove(estacao);
            db.SaveChanges();
            return RedirectToAction("ListarEstacoes");
        }
    }
}