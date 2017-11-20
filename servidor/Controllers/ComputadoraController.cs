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

 	[HttpGet]
        public JsonResult obtenerPCsLab(String nombreLab)
        {
            return Json(computadora.listaPCsReporte(nombreLab),
                        JsonRequestBehavior.AllowGet);
        }
        
        [HttpGet]
        public JsonResult obtenerlistaPCsLab(String nombreLab)
        {
            return Json(computadora.listaPCsLab(nombreLab),
                        JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult modificarDetalleReporte(int idReporte, string idPC, string color, string descripcion)
        {
            return Json(computadora.modificarDetalleReportes(idReporte, idPC, color, descripcion),
                        JsonRequestBehavior.AllowGet);
        }

 	    [HttpPost]
        public JsonResult crearPc(string idPC, string x, string y, string nombreLab)
        {
            return Json(computadora.crearPc(idPC, x, y,nombreLab),
                        JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult BorrarPC(string idPC)
        {
            return Json(computadora.BorrarPC(idPC),
                        JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult listaLabs()
        {
            return Json(computadora.listaLabs(),
                        JsonRequestBehavior.AllowGet);
        }
    }
}
