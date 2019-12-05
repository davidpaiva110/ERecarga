using E_Recarga.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity; //necessário por causa das querys sql -> para reconhecer as entidades
using System.Net;
using System.Web.Security;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

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

        public ActionResult RemoverRedesAdmin(string id)
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
            return View(rede);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<ActionResult> RemoverRedesAdmin(string id, string nome)
        {
            ApplicationUserManager _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); ;
            RedeProprietaria rede = db.RedesProprietarias.Find(id);
            List<Estacao> estacoes = new List<Estacao>();
            List<List<Posto>> listaPostos = new List<List<Posto>>();
            List<Reserva> reservas = new List<Reserva>();

            var estadoesdb = db.Estacoes.Where(r => r.RedeProprietariaId.Contains(id)); // Estações da rede
            foreach (Estacao est in estadoesdb)
            {
                estacoes.Add(est);
            }

            foreach(Estacao est in estacoes){
                var postosdb = db.Postos.Where(p => p.EstacaoId == est.EstacaoId);  // Postos de uma estação
                List<Posto> lista = new List<Posto>();
                foreach(Posto posto in postosdb)
                {
                    lista.Add(posto);
                }
            }

            foreach(List<Posto> lista in listaPostos)
            {
                foreach(Posto posto in lista)
                {
                    var reservadb = db.Reservas.Where(rv => rv.PostoId == posto.PostoId); // Reservas de um posto
                    foreach (Reserva reserva in reservadb)
                        reservas.Add(reserva);
                }
            }
            
            foreach(Reserva reserva in reservas)
            {
                db.Reservas.Remove(reserva);
                db.SaveChanges();
            }
            foreach (List<Posto> lista in listaPostos)
            {
                foreach (Posto posto in lista)
                {
                    db.Postos.Remove(posto);
                    db.SaveChanges();
                }
            }
            foreach(Estacao estacao in estacoes)
            {
                db.Estacoes.Remove(estacao);
                db.SaveChanges();
            }

            db.RedesProprietarias.Remove(rede);
            db.SaveChanges();

            var user = await _userManager.FindByIdAsync(id);
            var logins = user.Logins;
            var rolesForUser = await _userManager.GetRolesAsync(id);

            using (var transaction = db.Database.BeginTransaction())
            {
                foreach (var login in logins.ToList())
                {
                    await _userManager.RemoveLoginAsync(login.UserId, new UserLoginInfo(login.LoginProvider, login.ProviderKey));
                }

                if (rolesForUser.Count() > 0)
                {
                    foreach (var item in rolesForUser.ToList())
                    {
                        // item should be the name of the role
                        var result = await _userManager.RemoveFromRoleAsync(user.Id, item);
                    }
                }

                await _userManager.DeleteAsync(user);
                transaction.Commit();
            }


            return RedirectToAction("ListarRedesAdmin");
        }

        public ActionResult ListarMensagens()
        {
            var mensagens = db.Mensagens.OrderByDescending(c => c.MensagemId);
            return View(mensagens.ToList());
        }

        public ActionResult DetalhesMensagem(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mensagem mensagem = db.Mensagens.Find(id);
            if (mensagem == null)
            {
                return HttpNotFound();
            }
            return View(mensagem);
        }

        public ActionResult RemoverMensagem(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mensagem mensagem = db.Mensagens.Find(id);
            if (mensagem == null)
            {
                return HttpNotFound();
            }
            return View(mensagem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoverMensagem(int id)
        {
            Mensagem mensagem = db.Mensagens.Find(id);
            db.Mensagens.Remove(mensagem);
            db.SaveChanges();
            return RedirectToAction("ListarMensagens");
        }
    }
}