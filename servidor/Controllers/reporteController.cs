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
        public JsonResult cambiarEstadoReporte(int idReporte, string estado)
        {
            return Json(reporte.cambiarEstadoReporte(idReporte, estado),
                        JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult obtenerReporteInformacion(int idReporte)
        {
            return Json(reporte.obtenerReporteInformacion(idReporte),
                        JsonRequestBehavior.AllowGet);
        }



        //obtiene la lista de reportes pertenecientes a un estado en especifico ("enProceso","nuevo", y demas)
        [HttpGet]
        public JsonResult obtenerInfoReportesEstadoAction(string estadoReporte)
        {
            return Json(reporte.obtenerListaReportesSegunEstado(estadoReporte),
                        JsonRequestBehavior.AllowGet);
        }


        //obtiene la lista de reportes con proriedad, tomando en consideracion un filtro de importancia("fecha",'Admin')
        [HttpGet]
        public JsonResult obtenerListaReportesPriorizadosFiltroAction(string tipo)
        {
            return Json(reporte.obtenerListaReportesPriorizadosFiltro(tipo),
                        JsonRequestBehavior.AllowGet);
        }



        //asigna un tecnico a un reporte
        [HttpPost]
        public JsonResult asignarTecnicoReporteAction(int idReporte, int idUsuario)
        {
            return Json(reporte.asignarTecnicoReporte(idReporte, idUsuario),
                        JsonRequestBehavior.AllowGet);
        }


        //actualizar prioridad reporte
        [HttpPost]
        public JsonResult actualizarPrioridadAction(int idReporte, string fechaFinalizacion, string nivelPrioridad)
        {
            return Json(reporte.actualizarPrioridad(idReporte, fechaFinalizacion, nivelPrioridad),
                        JsonRequestBehavior.AllowGet);
        }

    }
}