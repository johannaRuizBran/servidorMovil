using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace servidor.Models
{
    public class Computadora
    {
        public string id { get; set; }
        public int idReporte { get; set; }
        public double x { get; set; }
        public double y { get; set; }
        public string color { get; set; }
        public string descripcion { get; set; }
    }
}