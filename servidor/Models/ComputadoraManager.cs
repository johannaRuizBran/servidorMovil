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
        public List<Computadora> listaPCsReporte(String nombreLab)
        {
            List<Computadora> lista = new List<Computadora>();
            Computadora computadora = null;
            SqlConnection con = new SqlConnection(conexionIP);
            con.Open();


            string sql = "exec obtenerPCsLaboratorioReporte @nombreLab;";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.Add("@nombreLab", System.Data.SqlDbType.VarChar).Value = nombreLab;

            SqlDataReader reader =
                cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            while (reader.Read())
            {
                computadora = new Computadora();
                computadora.id = reader.GetString(0);
                computadora.idReporte = 0;
                computadora.x = reader.GetDouble(1);
                computadora.y = reader.GetDouble(2);
                computadora.color = "Gris";
                computadora.descripcion = "none";
                lista.Add(computadora);
            }
           
            reader.Close();
            return lista;
        }
        public List<Computadora_detalle> listaPCsLab(String nombreLab)
        {
            List<Computadora_detalle> lista = new List<Computadora_detalle>();
            Computadora_detalle computadora = null;
            SqlConnection con = new SqlConnection(conexionIP);
            con.Open();


            string sql = "exec obtenerPCsLaboratorioReporte @nombreLab;";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.Add("@nombreLab", System.Data.SqlDbType.VarChar).Value = nombreLab;

            SqlDataReader reader =
                cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            while (reader.Read())
            {
                computadora = new Computadora_detalle();
                computadora.id = reader.GetString(0);
                computadora.x = reader.GetDouble(1);
                computadora.y = reader.GetDouble(2);
                computadora.lab = reader.GetString(3);
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

        public bool crearPc(string idPC, string x, string y, string nombreLab)
        {
            SqlConnection con = new SqlConnection(conexionIP);
            con.Open();
            string sql = "EXEC crearPc @idPC,@x,@y, @nombreLab;";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.Add("@idPC", System.Data.SqlDbType.VarChar).Value = idPC;
            cmd.Parameters.Add("@x", System.Data.SqlDbType.VarChar).Value = x;
            cmd.Parameters.Add("@y", System.Data.SqlDbType.VarChar).Value = y;
            cmd.Parameters.Add("@nombreLab", System.Data.SqlDbType.VarChar).Value = nombreLab;
            int respuestaQuery = cmd.ExecuteNonQuery();

            con.Close();
            return (respuestaQuery == 3);
        }
        public bool BorrarPC(string idPC)
        {
            SqlConnection con = new SqlConnection(conexionIP);
            con.Open();
            string sql = "EXEC borrarPc @idPC;";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.Add("@idPC", System.Data.SqlDbType.VarChar).Value = idPC;
            int respuestaQuery = cmd.ExecuteNonQuery();

            con.Close();
            return (respuestaQuery == 3);
        }

        public List<string> listaLabs()
        {
            List<string> lista = new List<string>();
            
            SqlConnection con = new SqlConnection(conexionIP);
            con.Open();


            string sql = "exec obtenerLabs;";
            SqlCommand cmd = new SqlCommand(sql, con);

            SqlDataReader reader =
                cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            while (reader.Read())
            {
                lista.Add(reader.GetString(0));
            }

            reader.Close();
            return lista;
        }
    }
}