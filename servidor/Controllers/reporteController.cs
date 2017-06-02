using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using servidor.Models;

namespace servidor.Controllers
{
    public class reporteController : Controller
    {
        private reporteManager reporte;
        public reporteController()
        {
            reporte = new reporteManager();
        }

        [HttpGet]
        public JsonResult obtenerReporteUsuario(string nombreU)
        {
            return Json(reporte.obtenerReporteUsuario(nombreU),
                        JsonRequestBehavior.AllowGet);
        }
        

        [HttpPost]
        public JsonResult crearReporte(Reporte item)
        {
            return Json(reporte.crearReporte(item),
                        JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult cancelarReporteUsuario(int idReporte)
        {
            return Json(reporte.cancelarReporteUsuario(idReporte),
                        JsonRequestBehavior.AllowGet);
        }

        //obtiene la lista de reportes pertenecientes a un estado en especifico ("enProceso","nuevo", y demas)
        [HttpGet]
        public JsonResult obtenerInfoReportesEstadoAction(string estadoReporte)
        {
            return Json(reporte.obtenerListaReportesSegunEstado(estadoReporte),
                        JsonRequestBehavior.AllowGet);
        }

        
        [HttpGet]
        public JsonResult obtenerReporteInformacion(int idReporte)
        {
            return Json(reporte.obtenerReporteInformacion(idReporte),
                        JsonRequestBehavior.AllowGet);
        }
    }
}