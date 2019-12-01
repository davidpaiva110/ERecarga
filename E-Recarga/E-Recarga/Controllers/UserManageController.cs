using E_Recarga.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;

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
            if(Data==null || HoraInicio==null || HoraFim == null)
            {
                ModelState.AddModelError("", "Insira a data e horários pretendidos!");
                error = true;
            }
            else if (Data.Value.Date < DateTime.Now.Date || (Data.Value.Date == DateTime.Now.Date && HoraInicio.Value.Hour <= DateTime.Now.Hour) || HoraInicio.Value.Hour >= HoraFim.Value.Hour)
            {
                ModelState.AddModelError("", "Data ou horas inválidas para efetuar reserva!");
                error = true;
            }
            if (Cidade.Length == 0)
            {
                ModelState.AddModelError("", "Insira a cidade pretendida!");
                error = true;
            }
            if(error) return View();
            return RedirectToAction("ListaPostosDisponiveis", new { cidade = Cidade, data = Data, horaInicio = HoraInicio, horaFim = HoraFim });
        }

        public ActionResult ListaPostosDisponiveis(string cidade, DateTime data, DateTime horaInicio, DateTime horaFim)
        {
            // FALTA VERIFICAR SE O ESTADO DO POSTO É ATIVO
            List<Posto> postosDisponiveis = new List<Posto>();
            var postos = db.Postos.Include(r => r.Estacao).Include(r => r.Estacao.RedeProprietaria).Where(r => r.Estacao.Cidade.Contains(cidade));
            foreach (Posto posto in postos) postosDisponiveis.Add(posto);

            foreach (Posto posto in postosDisponiveis)
            {
                var reservas = db.Reservas.Where(r => r.PostoId == posto.PostoId && DbFunctions.TruncateTime(r.Data) == DbFunctions.TruncateTime(data) && ((r.HoraInicio.Hour >= horaInicio.Hour && r.HoraInicio.Hour <= horaFim.Hour) || (r.HoraFim.Hour <= horaFim.Hour && r.HoraFim.Hour >= horaInicio.Hour)));
                if (reservas.Count() > 0) postosDisponiveis.Remove(posto);
            }
            return View(postosDisponiveis.ToList());
        }
    }
}