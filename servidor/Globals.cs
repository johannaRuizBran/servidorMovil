using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace servidor
{
    public class Globals
    {  
        private static string ip = @"Data Source= 192.168.2.116;Initial Catalog=mantenimiento;User Id=sa;Password=123";


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