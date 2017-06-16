using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace servidor.Models
{    
    public class informacionFaltanteManager
    {
        private static string conexionIP = Globals.IP;

        public informacionFaltante informacionFaltante(int idReporte)
        {
            informacionFaltante informacionFaltante = null;
            SqlConnection con = new SqlConnection(conexionIP);
            con.Open();

            string sql = "exec obtenerInformacionFaltante @idReporte";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.Add("@idReporte", System.Data.SqlDbType.Int).Value = idReporte;

            SqlDataReader reader =
                cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

            if (reader.Read())
            {

                informacionFaltante = new informacionFaltante();
                informacionFaltante.idReporte = reader.GetInt32(0);
                informacionFaltante.observacion = reader.GetString(1);

            }
            reader.Close();
            return informacionFaltante;
        }


        // solicitas mas informacion al usuario sobre un reporte en especifico
        public bool solicitarMasInformacionReporte(int idReporte,string informacion)
        {
            SqlConnection con = new SqlConnection(conexionIP);
            con.Open();
            string sql = "EXEC solicitudMasInformacionReporte @idReporte,@informacion";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.Add("@idReporte", System.Data.SqlDbType.Int).Value = idReporte;
            cmd.Parameters.Add("@informacion", System.Data.SqlDbType.VarChar).Value = informacion;

            int respuestaQuery = cmd.ExecuteNonQuery();

            con.Close();
            return (respuestaQuery == 1);
        }
            
        
        public bool agregarMasInformacion(int idReporte, string informacion)
        {            
            SqlConnection con = new SqlConnection(conexionIP);
            con.Open();
            string sql = "EXEC actualizarInformacionReporte @idReporte,@informacion";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.Add("@idReporte", System.Data.SqlDbType.Int).Value = idReporte;
            cmd.Parameters.Add("@informacion", System.Data.SqlDbType.VarChar).Value = informacion;

            int respuestaQuery = cmd.ExecuteNonQuery();

            con.Close();
            return (respuestaQuery == 3);
        }
    }
}