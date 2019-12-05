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

    }
}