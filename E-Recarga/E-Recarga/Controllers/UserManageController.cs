using E_Recarga.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Net;

namespace E_Recarga.Controllers
{
    [Authorize(Roles = "Utilizador")]
    public class UserManageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult PesquisarPostos()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PesquisarPostos(string Cidade, DateTime? Data, DateTime? HoraInicio, DateTime? HoraFim)
        {
            bool error = false;
            if (Data == null || HoraInicio == null || HoraFim == null)
            {
                ModelState.AddModelError("", "Insira a data e horários pretendidos!");
                error = true;
            }
            else if (Data.Value.Date < DateTime.Now.Date || (Data.Value.Date == DateTime.Now.Date && HoraInicio.Value <= DateTime.Now) || HoraInicio.Value >= HoraFim.Value)
            {
                ModelState.AddModelError("", "Data ou horas inválidas para efetuar reserva!");
                error = true;
            }
            if (Cidade.Length == 0)
            {
                ModelState.AddModelError("", "Insira a cidade pretendida!");
                error = true;
            }
            if (error) return View();
            return RedirectToAction("ListaPostosDisponiveis", new { cidade = Cidade, data = Data, horaInicio = HoraInicio, horaFim = HoraFim });
        }

        public ActionResult ListaPostosDisponiveis(string cidade, DateTime data, DateTime horaInicio, DateTime horaFim)
        {
            List<Posto> postosDisponiveis = new List<Posto>();
            List<Posto> postosToView = new List<Posto>();
            var postos = db.Postos.Include(r => r.Estacao).Include(r => r.Estacao.RedeProprietaria).Where(r => r.Estacao.Cidade.ToLower().Contains(cidade.ToLower()) && r.Estado==true);
            foreach (Posto posto in postos)
            {
                postosDisponiveis.Add(posto);
                postosToView.Add(posto);
            }

            foreach (Posto posto in postosDisponiveis)
            {
                var reservas = db.Reservas.Where(r => r.PostoId == posto.PostoId && DbFunctions.TruncateTime(r.Data) == DbFunctions.TruncateTime(data) && ((r.HoraInicio >= horaInicio && r.HoraInicio <= horaFim) || (r.HoraFim <= horaFim && r.HoraFim >= horaInicio)));
                if (reservas.Count() > 0) postosToView.Remove(posto);
            }
            ViewBag.data = data;
            ViewBag.horaInicio = horaInicio;
            ViewBag.horaFim = horaFim;
            return View(postosToView.ToList());
        }

        public ActionResult ConcluirReserva(int id, DateTime data, DateTime horaInicio, DateTime horaFim)
        {
            Posto posto = db.Postos.Include(r => r.Estacao).Include(r => r.Estacao.RedeProprietaria).SingleOrDefault(r => r.PostoId == id);
            Reserva reserva = new Reserva(id, data, horaInicio, horaFim);
            ViewBag.posto = posto;
            return View(reserva);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConcluirReserva([Bind(Include = "PostoId, Data, HoraInicio, HoraFim")] Reserva reserva)
        {
            if (ModelState.IsValid)
            {
                string userId = User.Identity.GetUserId();
                db.Reservas.Add(new Reserva(reserva.Data, reserva.HoraInicio, reserva.HoraFim, reserva.PostoId, userId));
                db.SaveChanges();
                return RedirectToAction("HistoricoReservas");
            }
            return RedirectToAction("PesquisarPostos");
        }

        public ActionResult HistoricoReservas()
        {
            string userId = User.Identity.GetUserId();
            return View(db.Reservas.Include(c => c.Posto).Include(c => c.Posto.Estacao).Where(c => c.UserId.Contains(userId)).OrderByDescending(c=>c.UserId).ToList());
        }

        public ActionResult DetalhesReserva(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reserva reserva = db.Reservas.Include(r => r.Posto).Include(r => r.Posto.Estacao).Include(r => r.Posto.Estacao.RedeProprietaria).SingleOrDefault(r => r.ReservaId == id);
            if (reserva == null)
            {
                return HttpNotFound();
            }
            return View(reserva);
        }

        public ActionResult CancelarReserva(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reserva reserva = db.Reservas.Include(r => r.Posto).Include(r => r.Posto.Estacao).Include(r => r.Posto.Estacao.RedeProprietaria).SingleOrDefault(r => r.ReservaId == id);
            if (reserva == null)
            {
                return HttpNotFound();
            }
            return View(reserva);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CancelarReserva(int id)
        {
            Reserva reserva = db.Reservas.Find(id);
            db.Reservas.Remove(reserva);
            db.SaveChanges();
            return RedirectToAction("HistoricoReservas");
        }
    }
}