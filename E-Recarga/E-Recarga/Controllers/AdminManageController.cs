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
using System.Web.Services;
using System.Web.Script.Serialization;

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
            ViewBag.pesquisa = "";
            var postos = db.Postos.Include(p => p.Estacao).Include(p => p.Estacao.RedeProprietaria).Where(p => p.Estado.Equals(false));
            foreach (Posto posto in postos)
                postosPendentes.Add(posto);
            return View(postosPendentes.ToList());
        }

        public ActionResult AceitarPostoAdmin(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return RedirectToAction("ListarPostosPendentes");
            }
            Posto posto = db.Postos.Include(r => r.Estacao).Include(r => r.Estacao.RedeProprietaria).SingleOrDefault(r => r.PostoId == id);
            if (posto == null)
            {
                //return HttpNotFound();
                return RedirectToAction("ListarPostosPendentes");
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
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return RedirectToAction("ListarPostosPendentes");
            }
            Posto posto = db.Postos.Include(r => r.Estacao).Include(r => r.Estacao.RedeProprietaria).SingleOrDefault(r => r.PostoId == id);
            if (posto == null)
            {
                //return HttpNotFound();
                return RedirectToAction("ListarPostosPendentes");
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
            ViewBag.pesquisa = "";
            var redesdb = db.RedesProprietarias.Include(r => r.Estacoes);
            foreach (RedeProprietaria est in redesdb)
                redes.Add(est);
            return View(redes.ToList());
        }

        public ActionResult DetalhesRedesAdmin(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return RedirectToAction("ListarRedesAdmin");
            }
            RedeProprietaria rede = db.RedesProprietarias.Find(id);
            if (rede == null)
            {
                //return HttpNotFound();
                return RedirectToAction("ListarRedesAdmin");
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
            if (String.IsNullOrEmpty(id))
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return RedirectToAction("ListarRedesAdmin");
            }
            RedeProprietaria rede = db.RedesProprietarias.Find(id);
            if (rede == null)
            {
                //return HttpNotFound();
                return RedirectToAction("ListarRedesAdmin");
            }
            return View(rede);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<ActionResult> RemoverRedesAdmin(string id, string nome)
        {
            ApplicationUserManager _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            RedeProprietaria rede = db.RedesProprietarias.Find(id);
            List<Estacao> estacoes = new List<Estacao>();
            List<List<Posto>> listaPostos = new List<List<Posto>>();
            List<Reserva> reservas = new List<Reserva>();

            var estadoesdb = db.Estacoes.Where(r => r.RedeProprietariaId.Contains(id)); // Estações da rede
            foreach (Estacao est in estadoesdb)
            {
                estacoes.Add(est);
            }

            foreach (Estacao est in estacoes)
            {
                var postosdb = db.Postos.Where(p => p.EstacaoId == est.EstacaoId);  // Postos de uma estação
                List<Posto> lista = new List<Posto>();
                foreach (Posto posto in postosdb)
                {
                    lista.Add(posto);
                }
            }

            foreach (List<Posto> lista in listaPostos)
            {
                foreach (Posto posto in lista)
                {
                    var reservadb = db.Reservas.Where(rv => rv.PostoId == posto.PostoId); // Reservas de um posto
                    foreach (Reserva reserva in reservadb)
                        reservas.Add(reserva);
                }
            }

            foreach (Reserva reserva in reservas)
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
            foreach (Estacao estacao in estacoes)
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

        public ActionResult ListarUtilizadoresAdmin()
        {
            var usersdb = db.Users;
            ViewBag.pesquisa = "";
            List <User> utilizadores = new List<User>();
            foreach (User user in usersdb)
                utilizadores.Add(user);
            return View(utilizadores.ToList());
        }

        public ActionResult DetalhesUtilizadoresAdmin(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return RedirectToAction("ListarUtilizadoresAdmin");
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                //return HttpNotFound();
                return RedirectToAction("ListarUtilizadoresAdmin");
            }
            ViewBag.user = user;
            List<Reserva> reservas = new List<Reserva>();
            var reservasdb = db.Reservas.Include(r => r.Posto).Include(r => r.Posto.Estacao.RedeProprietaria).Where(r => r.UserId == id);
            foreach (Reserva reserva in reservasdb)
                reservas.Add(reserva);

            return View(reservas.ToList());
        }

        public ActionResult RemoverUtilizadoresAdmin(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return RedirectToAction("ListarUtilizadoresAdmin");
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                //return HttpNotFound();
                return RedirectToAction("ListarUtilizadoresAdmin");
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<ActionResult> RemoverUtilizadoresAdmin(string id, string nome)
        {
            ApplicationUserManager _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            User userE = db.Users.Find(id);
            List<Reserva> reservas = new List<Reserva>();
            var reservasdb = db.Reservas.Where(r => r.UserId == id);
            foreach (Reserva reserva in reservasdb)
                reservas.Add(reserva);
            foreach (Reserva reserva in reservas)
            {
                db.Reservas.Remove(reserva);
                db.SaveChanges();
            }
            db.Users.Remove(userE);
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

            return RedirectToAction("ListarUtilizadoresAdmin");
        }

        public ActionResult ListarMensagens()
        {
            ViewBag.pesquisa = "";
            var mensagens = db.Mensagens.OrderByDescending(c => c.MensagemId);
            return View(mensagens.ToList());
        }

        public ActionResult DetalhesMensagem(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return RedirectToAction("ListarMensagens");
            }
            Mensagem mensagem = db.Mensagens.Find(id);
            if (mensagem == null)
            {
                //return HttpNotFound();
                return RedirectToAction("ListarMensagens");
            }
            return View(mensagem);
        }

        public ActionResult RemoverMensagem(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return RedirectToAction("ListarMensagens");
            }
            Mensagem mensagem = db.Mensagens.Find(id);
            if (mensagem == null)
            {
                //return HttpNotFound();
                return RedirectToAction("ListarMensagens");
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

        public ActionResult EstatisticasAdmin()
        {
            ViewBag.pesquisa = "";
            SortedList<int, List<RedeProprietaria>> hashmap = new SortedList<int, List<RedeProprietaria>>();
            List<RedeProprietaria> redes = new List<RedeProprietaria>();
            List<List<Reserva>> listaReservas = new List<List<Reserva>>();

            // Redes Proprietárias
            var redesdb = db.RedesProprietarias;
            foreach (RedeProprietaria rede in redesdb)
            {
                redes.Add(rede);
                listaReservas.Add(new List<Reserva>());
            }
            // Reservas de cada rede proprietária
            for (int i = 0; i < redes.Count; i++)
            {
                string idRede = redes[i].RedeProprietariaId;
                var reservasdb = db.Reservas.Where(r => r.Posto.Estacao.RedeProprietariaId == idRede);
                foreach (Reserva r in reservasdb)
                {
                    listaReservas[i].Add(r);
                }
            }
            // Ordenar as redes propiretárias com os respetivos numeros de reserva
            for (int i = 0; i < redes.Count; i++)
            {
                List<RedeProprietaria> lista = new List<RedeProprietaria>();
                if (!hashmap.TryGetValue(listaReservas[i].Count, out lista)) // Se não existir nenhuma rede com aquele numero de 
                {
                    lista = new List<RedeProprietaria>();
                    hashmap.Add(listaReservas[i].Count, lista);
                }
                hashmap[listaReservas[i].Count].Add(redes[i]);
            }
            List<EstatisticasAdmin> estatisticas = new List<EstatisticasAdmin>();
            List<String> redesP = new List<String>();
            List<int> nR = new List<int>();
            for (int i = hashmap.Count - 1; i >= 0; i--)
            {
                List<RedeProprietaria> aux;
                aux = hashmap.Values[i];
                foreach (RedeProprietaria rede in aux)
                {
                    EstatisticasAdmin e = new EstatisticasAdmin(rede, hashmap.Keys[i]);
                    estatisticas.Add(e);
                    redesP.Add(rede.Nome);
                    nR.Add(hashmap.Keys[i]);
                }
            }

            ViewBag.REDES = redesP;
            ViewBag.NRESERVAS = nR;

            return View(estatisticas.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ListarRedesAdmin(string pesquisa)
        {
            List<RedeProprietaria> redes = new List<RedeProprietaria>();
            ViewBag.pesquisa = pesquisa;
            var redesdb = db.RedesProprietarias.Include(r => r.Estacoes).Where(r=>r.Nome.ToLower().Contains(pesquisa.ToLower()));
            if (String.IsNullOrEmpty(pesquisa))
                redesdb = db.RedesProprietarias.Include(r => r.Estacoes);
            foreach (RedeProprietaria est in redesdb)
                redes.Add(est);
            return View(redes.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ListarPostosPendentes(string pesquisa)
        {
            List<Posto> postosPendentes = new List<Posto>();
            ViewBag.pesquisa = pesquisa;
            var postos = db.Postos.Include(p => p.Estacao).Include(p => p.Estacao.RedeProprietaria).Where(p => p.Estado.Equals(false) && (p.Estacao.Cidade.ToLower().Contains(pesquisa.ToLower()) || p.Estacao.Localizacao.ToLower().Contains(pesquisa.ToLower()) || p.Estacao.RedeProprietaria.Nome.ToLower().Contains(pesquisa.ToLower())));
            if (String.IsNullOrEmpty(pesquisa))
                postos = db.Postos.Include(p => p.Estacao).Include(p => p.Estacao.RedeProprietaria).Where(p => p.Estado.Equals(false));
            foreach (Posto posto in postos)
                postosPendentes.Add(posto);
            return View(postosPendentes.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EstatisticasAdmin(string pesquisa)
        {
            ViewBag.pesquisa = pesquisa;
            SortedList<int, List<RedeProprietaria>> hashmap = new SortedList<int, List<RedeProprietaria>>();
            List<RedeProprietaria> redes = new List<RedeProprietaria>();
            List<List<Reserva>> listaReservas = new List<List<Reserva>>();

            // Redes Proprietárias
            var redesdb = db.RedesProprietarias.Where(v=>v.Nome.ToLower().Contains(pesquisa.ToLower()));
            if (String.IsNullOrEmpty(pesquisa))
                redesdb = db.RedesProprietarias;
            foreach (RedeProprietaria rede in redesdb)
            {
                redes.Add(rede);
                listaReservas.Add(new List<Reserva>());
            }
            // Reservas de cada rede proprietária
            for (int i = 0; i < redes.Count; i++)
            {
                string idRede = redes[i].RedeProprietariaId;
                var reservasdb = db.Reservas.Where(r => r.Posto.Estacao.RedeProprietariaId == idRede);
                foreach (Reserva r in reservasdb)
                {
                    listaReservas[i].Add(r);
                }
            }
            // Ordenar as redes propiretárias com os respetivos numeros de reserva
            for (int i = 0; i < redes.Count; i++)
            {
                List<RedeProprietaria> lista = new List<RedeProprietaria>();
                if (!hashmap.TryGetValue(listaReservas[i].Count, out lista)) // Se não existir nenhuma rede com aquele numero de 
                {
                    lista = new List<RedeProprietaria>();
                    hashmap.Add(listaReservas[i].Count, lista);
                }
                hashmap[listaReservas[i].Count].Add(redes[i]);
            }
            List<EstatisticasAdmin> estatisticas = new List<EstatisticasAdmin>();
            List<String> redesP = new List<String>();
            List<int> nR = new List<int>();
            for (int i = hashmap.Count - 1; i >= 0; i--)
            {
                List<RedeProprietaria> aux;
                aux = hashmap.Values[i];
                foreach (RedeProprietaria rede in aux)
                {
                    EstatisticasAdmin e = new EstatisticasAdmin(rede, hashmap.Keys[i]);
                    estatisticas.Add(e);
                    redesP.Add(rede.Nome);
                    nR.Add(hashmap.Keys[i]);
                }
            }

            ViewBag.REDES = redesP;
            ViewBag.NRESERVAS = nR;

            return View(estatisticas.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ListarUtilizadoresAdmin(string pesquisa)
        {
            var usersdb = db.Users.Where(v=>v.Nome.ToLower().Contains(pesquisa.ToLower()));
            if (String.IsNullOrEmpty(pesquisa))
                usersdb = db.Users;
            ViewBag.pesquisa = pesquisa;
            List<User> utilizadores = new List<User>();
            foreach (User user in usersdb)
                utilizadores.Add(user);
            return View(utilizadores.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ListarMensagens(string pesquisa)
        {
            ViewBag.pesquisa = pesquisa;
            if (String.IsNullOrEmpty(pesquisa))
                return View(db.Mensagens.OrderByDescending(c => c.MensagemId).ToList());
            else
                return View(db.Mensagens.Where(v=>v.Email.Contains(pesquisa)).OrderByDescending(c => c.MensagemId).ToList());
        }
    }
}