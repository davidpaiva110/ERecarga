using E_Recarga.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_Recarga.Controllers
{
    public partial class  RedeProprietariaManageController : Controller
    {

        // Validação do Preço de Carregamento
        public JsonResult Positivo(string Preco)
        {
            if (double.TryParse(Preco.ToString(), out double num))
                if (num > 0)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }

            return Json(false, JsonRequestBehavior.AllowGet);
        }
    }
}