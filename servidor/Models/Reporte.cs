using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace servidor.Models
{
    public class Reporte
    {
        public int id { get; set; }
        public string estadoReporte { get; set; }
        public string prioridadReporte { get; set; }
        public string fechaReporte { get; set; }
        public string fechaFinalizacion { get; set; }
        public string descripcion { get; set; }
        public string establecimiento { get; set; }

        public string nombreUsuario { get; set; }
        public string nombre { get; set; }
    }
}