using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace servidor.Models
{
    public class Computadora
    {
        public int id { get; set; }
        public int idReporte { get; set; }
        public float x { get; set; }
        public float y { get; set; }
        public string color { get; set; }
        public string descripcion { get; set; }
    }
}