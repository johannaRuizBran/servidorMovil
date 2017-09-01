using servidor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace servidor.Controllers
{
    public class ComputadoraController : Controller
    {
        private ComputadoraManager computadora;
        public ComputadoraController()
        {
            computadora = new ComputadoraManager();
        }


        [HttpGet]
        public JsonResult obtenerPCs(string nombreLab)
        {
            return Json(computadora.listaPCs(nombreLab),
                        JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult actualizarColor(int idReporte, int idPC, string color)
        {
            return Json(computadora.actualizarColor(idReporte, idPC, color));
        }
    }
}
