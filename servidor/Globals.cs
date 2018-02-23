using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace servidor
{
    public class Globals
    {
        //c0mun1d@d
        private static string ip = @"Data Source= 172.24.42.5;Initial Catalog=mantenimiento;User Id=sa;Password=1234";


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