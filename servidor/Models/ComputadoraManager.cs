using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace servidor.Models
{
    public class ComputadoraManager
    {
        private static string conexionIP = Globals.IP;


        //FALTA CREAR PROCEDURE DB
        public List<Computadora> listaPCs(string nombreLab)
        {
            List<Computadora> lista = new List<Computadora>();
            Computadora computadora = null;
            SqlConnection con = new SqlConnection(conexionIP);
            con.Open();


            string sql = "exec obtenerPCsLaboratorio @nombreLab";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.Add("@nombreLab", System.Data.SqlDbType.VarChar).Value = nombreLab;

            SqlDataReader reader =
                cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

            while (reader.Read())
            {
                computadora = new Computadora();
                computadora.id = reader.GetInt32(0);
                computadora.idReporte = reader.GetInt32(1);
                computadora.x = reader.GetFloat(2);
                computadora.y = reader.GetFloat(3);
                computadora.color = reader.GetString(4);
                computadora.descripcion = reader.GetString(5);
                lista.Add(computadora);
            }
            reader.Close();
            return lista;
        }
    }
}