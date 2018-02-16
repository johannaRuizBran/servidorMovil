using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace servidor
{
    public class Globals
    {  
        private static string ip = @"Data Source= 172.24.20.41;Initial Catalog=mantenimiento;User Id=sa;Password=c0mun1d@d";


        public static string IP
        {
            get
            {
                return ip;
            }
            set
            {
                ip = value;
            }
        }
    }
}