using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using servidor.Models;

namespace servidor.Controllers
{
    public class informacionFaltaController : Controller
    {
        // GET: informacionFalta
        private informacionFaltanteManager informacion;

        public informacionFaltaController()
        {
            informacion = new informacionFaltanteManager();
        }

        [HttpGet]
        public JsonResult informacionFaltante(int idReporte)
        {
            return Json(informacion.informacionFaltante(idReporte),
                        JsonRequestBehavior.AllowGet);
        }
    }
}