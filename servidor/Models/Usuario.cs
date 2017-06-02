using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace servidor.Models
{
    public class Usuario
    {
        public string nombreUsuario { get; set; }
        public string contrasena { get; set; }
        public string nombre { get; set; }
        public string apellido1 { get; set; }
        public string apellido2 { get; set; }
        public string correo { get; set; }
        public string telefono { get; set; }
        public string rol { get; set; }
        public string activo { get; set; }
    }
}