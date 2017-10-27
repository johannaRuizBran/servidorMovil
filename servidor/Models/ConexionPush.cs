using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace servidor.Models
{
    public class ConexionPush
    {
        public string appID { get; set; }
        public string senderID { get; set; }
        public string deviceID { get; set; }
        public string mensaje { get; set; }
        public int idReporte { get; set; }
    }
}