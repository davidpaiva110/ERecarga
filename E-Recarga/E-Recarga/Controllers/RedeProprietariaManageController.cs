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
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return RedirectToAction("ListarPostos");
            }
            Posto posto = db.Postos.Find(id);
            if (posto == null)
            {
                //return HttpNotFound();
                return RedirectToAction("ListarPostos");
            }
            string userId = User.Identity.GetUserId();
            ViewBag.Estacoes = new SelectList(db.Estacoes.Where(u => u.RedeProprietariaId.Contains(userId)).ToList(), "EstacaoId", "EstacaoId");
            return View(posto);
        }

        public ActionResult EditarEstacao(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return RedirectToAction("ListarEstacoes");
            }
            Estacao estacao = db.Estacoes.Find(id);
            if (estacao == null)
            {
                //return HttpNotFound();
                return RedirectToAction("ListarEstacoes");
            }
            return View(estacao);
        }

        public ActionResult DetalhesPosto(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return RedirectToAction("ListarPostos");
            }
            Posto posto = db.Postos.Include(r => r.Estacao).SingleOrDefault(r => r.PostoId==id);
            if (posto == null)
            {
                //return HttpNotFound();
                return RedirectToAction("ListarPostos");
            }
            return View(posto);
        }

        public ActionResult DetalhesEstacao(int? id)
        {

            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return RedirectToAction("ListarEstacoes");

            }
            Estacao estacao = db.Estacoes.Find(id);
            if (estacao == null)
            {
                //return HttpNotFound();
                return RedirectToAction("ListarEstacoes");
            }
            return View(estacao);
        }

        public ActionResult RemoverPosto(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return RedirectToAction("ListarPostos");
            }
            Posto posto = db.Postos.Include(r => r.Estacao).SingleOrDefault(r => r.PostoId == id);
            if (posto == null)
            {
                //return HttpNotFound();
                return RedirectToAction("ListarPostos");
            }
            return View(posto);
        }

        public ActionResult RemoverEstacao(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return RedirectToAction("ListarEstacoes");
            }
            Estacao estacao = db.Estacoes.Find(id);
            if (estacao == null)
            {
                //return HttpNotFound();
                return RedirectToAction("ListarEstacoes");
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

        public ActionResult ListarReservas()
        {
            List<Reserva> reservas = new List<Reserva>();
            string userId = User.Identity.GetUserId();
            var reservasdb = db.Reservas.Include(r => r.Posto.Estacao.RedeProprietaria).Where(r => r.Posto.Estacao.RedeProprietariaId == userId);
            foreach(Reserva reserva in reservasdb)
                reservas.Add(reserva);
            return View(reservas.ToList());
        }

        public ActionResult DetalhesReserva(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return RedirectToAction("ListarReservas");
            }
            Reserva reserva = db.Reservas.Include(r => r.Posto.Estacao.RedeProprietaria).Include(r => r.User).SingleOrDefault(r => r.ReservaId == id);
            if (reserva == null)
            {
                //return HttpNotFound();
                return RedirectToAction("ListarReservas");
            }
            return View(reserva);
        }

        public ActionResult ListarEstacoesEstatisticas()
        {
            List<Estacao> estacoes = new List<Estacao>();
            SortedList<int, List<Estacao>> hashmap = new SortedList<int, List<Estacao>>();

            var estacoesdb = db.Estacoes;
            foreach (Estacao estacao in estacoesdb)
                estacoes.Add(estacao);
            //Ordenação com a SortedList
            foreach (Estacao estacao in estacoes)
            {
                List<Reserva> lista = new List<Reserva>();
                List<Estacao> listaEstacao = new List<Estacao>();
                var reservasdb = db.Reservas.Where(r => r.Posto.EstacaoId == estacao.EstacaoId);
                foreach (Reserva r in reservasdb)
                    lista.Add(r);
                //Ver se já existe alguma estação com o mesmo numero de reservas
                if (!hashmap.TryGetValue(lista.Count(), out listaEstacao))
                {
                    listaEstacao = new List<Estacao>();
                    hashmap.Add(lista.Count(), listaEstacao);
                }
                hashmap[lista.Count()].Add(estacao);
            }
            List<Estatisticas> estatisticas = new List<Estatisticas>();
            for(int i= hashmap.Count-1; i >=0 ; i--)
            {
                List<Estacao> aux;
                aux = hashmap.Values[i];
                foreach(Estacao estacao in aux)
                {
                    Estatisticas e = new Estatisticas(hashmap.Keys[i], estacao);
                    estatisticas.Add(e);
                }
            }
                return View(estatisticas.ToList());
        }

        public ActionResult DetalhesEstatisticas(int? id)
        {
            List<Posto> postos = new List<Posto>();
            List<double> tempoUtilizacao = new List<double>();
            List<List<Reserva>> listaReservas = new List<List<Reserva>>();
            SortedList<double, List<Posto>> hashmap = new SortedList<double, List<Posto>>();
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return RedirectToAction("ListarEstacoesEstatisticas");
            }
            Estacao estacao = db.Estacoes.Find(id);
            if (estacao == null)
            {
                //return HttpNotFound();
                return RedirectToAction("ListarEstacoesEstatisticas");
            }
            var postosdb = db.Postos.Where(r => r.EstacaoId == id); //Postos
            foreach(Posto p in postosdb)
            {
                listaReservas.Add(new List<Reserva>());
                postos.Add(p);
            }
            // Reservas de cada posto
            for(int i = 0; i < postos.Count; i++)
            {
                int pid = postos[i].PostoId;
                var reservasdb = db.Reservas.Where(r => r.PostoId == pid);
                foreach (Reserva r in reservasdb)
                {
                    listaReservas[i].Add(r);
                }
            }
            // Calcular o tempo de utilização de um posto
            double tempo = 0.0;
            foreach(List<Reserva> lista in listaReservas)
            {
                tempo = 0.0;
                foreach(Reserva r in lista)
                {
                    double inicio = r.HoraInicio.Hour * 3600 + r.HoraInicio.Minute * 60;
                    double fim = r.HoraFim.Hour * 3600 + r.HoraFim.Minute * 60;
                    double dif = fim - inicio;
                    tempo = tempo + dif;
                }
                tempoUtilizacao.Add(tempo/3600);
            }

            //Ordenar os postos com os respetivos tempos de utilização
            for (int i = 0; i < tempoUtilizacao.Count; i++)
            {
                List<Posto> lista = new List<Posto>();
                if (!hashmap.TryGetValue(tempoUtilizacao[i], out lista))
                {
                    lista = new List<Posto>(); 
                    hashmap.Add(tempoUtilizacao[i], lista); 
                }
                hashmap[tempoUtilizacao[i]].Add(postos[i]);
            }
            List<EstatisticasDetalhes> estatisticas = new List<EstatisticasDetalhes>();
            for (int i = hashmap.Count - 1; i >= 0; i--)
            {
                List<Posto> aux;
                aux = hashmap.Values[i];
                foreach (Posto posto in aux)
                {
                    EstatisticasDetalhes e = new EstatisticasDetalhes(estacao, posto, hashmap.Keys[i]);
                    estatisticas.Add(e);
                }
            }

            return View(estatisticas.ToList());
        }
    }
}