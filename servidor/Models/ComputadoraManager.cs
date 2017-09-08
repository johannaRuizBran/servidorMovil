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


      
        public List<Computadora> listaPCs(int idReporte)
        {
            List<Computadora> lista = new List<Computadora>();
            Computadora computadora = null;
            SqlConnection con = new SqlConnection(conexionIP);
            con.Open();


            string sql = "exec obtenerPCsLaboratorio @idReporte;";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.Add("@idReporte", System.Data.SqlDbType.VarChar).Value = idReporte;

            SqlDataReader reader =
                cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

            while (reader.Read())
            {
                
                computadora = new Computadora();
                computadora.id = reader.GetString(1);
                computadora.idReporte = reader.GetInt32(0);
                computadora.x = reader.GetDouble(5);
                computadora.y = reader.GetDouble(6);
                computadora.color = reader.GetString(2);
                try
                {
                    computadora.descripcion = reader.GetString(3);
                }
                catch (Exception e)
                { }

                
                lista.Add(computadora);
            }
            reader.Close();
            return lista;
        }

        public bool modificarDetalleReportes(int idReporte, string idPC, string color, string descripcion)
        {
            SqlConnection con = new SqlConnection(conexionIP);
            con.Open();
            string sql = "EXEC actualizarDetalleReporte @idReporte,@idPC,@color, @descripcion;";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.Add("@idReporte", System.Data.SqlDbType.Int).Value = idReporte;
            cmd.Parameters.Add("@idPC", System.Data.SqlDbType.VarChar).Value = idPC;
            cmd.Parameters.Add("@color", System.Data.SqlDbType.VarChar).Value = color;
            cmd.Parameters.Add("@descripcion", System.Data.SqlDbType.VarChar).Value = descripcion;
            int respuestaQuery = cmd.ExecuteNonQuery();

            con.Close();
            return (respuestaQuery == 3);
        }
    }
}