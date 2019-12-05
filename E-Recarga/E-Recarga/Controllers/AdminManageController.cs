using E_Recarga.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity; //necessário por causa das querys sql -> para reconhecer as entidades
using System.Net;

namespace E_Recarga.Controllers
{

    [Authorize(Roles = "Admin")]
    public class AdminManageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AdminManage
        public ActionResult ListarPostosPendentes()
        {
            List<Posto> postosPendentes = new List<Posto>();
            var postos = db.Postos.Include(p => p.Estacao).Include(p => p.Estacao.RedeProprietaria).Where(p => p.Estado.Equals(false));
            foreach (Posto posto in postos)
                postosPendentes.Add(posto);
            return View(postosPendentes.ToList());
        }

        public ActionResult AceitarPostoAdmin(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Posto posto = db.Postos.Include(r => r.Estacao).Include(r => r.Estacao.RedeProprietaria).SingleOrDefault(r => r.PostoId == id);
            if (posto == null)
            {
                return HttpNotFound();
            }
            return View(posto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AceitarPostoAdmin([Bind(Include = "PostoId,EstacaoId")] Posto posto)
        {
            if (ModelState.IsValid)
            {
                posto.Estado = true;
                db.Entry(posto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ListarPostosPendentes");
            }
            return View(posto);
        }

        public ActionResult RemoverPostoAdmin(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Posto posto = db.Postos.Include(r => r.Estacao).Include(r => r.Estacao.RedeProprietaria).SingleOrDefault(r => r.PostoId == id);
            if (posto == null)
            {
                return HttpNotFound();
            }
            return View(posto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoverPostoAdmin(int id)
        {
            Posto posto = db.Postos.Find(id);
            db.Postos.Remove(posto);
            db.SaveChanges();
            return RedirectToAction("ListarPostosPendentes");
        }


        public ActionResult ListarRedesAdmin()
        {
            List<RedeProprietaria> redes = new List<RedeProprietaria>();
            var redesdb = db.RedesProprietarias.Include(r => r.Estacoes);
            foreach (RedeProprietaria est in redesdb)
                redes.Add(est);
            return View(redes.ToList());
        }

        public ActionResult DetalhesRedesAdmin(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RedeProprietaria rede = db.RedesProprietarias.Find(id);
            if (rede == null)
            {
                return HttpNotFound();
            }
            ViewBag.rede = rede;
            List<Estacao> estacoes = new List<Estacao>();
            var estadoesdb = db.Estacoes.Include(r => r.Postos).Where(r => r.RedeProprietariaId == id);
            foreach (Estacao est in estadoesdb)
                estacoes.Add(est);

            return View(estacoes.ToList());
        }

        

    }
}