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
        public JsonResult obtenerPCsReporte(int idReporte)
        {
            return Json(computadora.listaPCs(idReporte),
                        JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult modificarDetalleReporte(int idReporte, string idPC, string color, string descripcion)
        {
            return Json(computadora.modificarDetalleReportes(idReporte, idPC, color, descripcion),
                        JsonRequestBehavior.AllowGet);
        }
    }
}
