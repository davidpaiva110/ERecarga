using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_Recarga.Controllers
{
    [Authorize(Roles="Utilizador")]
    public class ReservationsController : Controller
    {
        // GET: Reservations
        public ActionResult PesquisarPosto()
        {
            return View();
        }

        public ActionResult Historico()
        {
            return View();
        }

        public ActionResult Detalhes()
        {
            return View();
        }

        public ActionResult Reservar()
        {
            return View();
        }
    }
}