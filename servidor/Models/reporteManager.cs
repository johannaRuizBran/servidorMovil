using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace servidor.Models
{
    public class reporteManager
    {
        private static string conexionIP = Globals.IP;

        //cancelar reporte
        public bool crearReporte(Reporte item)
        {
            SqlConnection con = new SqlConnection(conexionIP);
            con.Open();
            string sql = "EXEC crearReporte @estadoReporteVar,@fechaFinalizacionVar," +
                "@descripcionVar,@establecimientoVar,@idUsuarioVar;";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.Add("@estadoReporteVar", System.Data.SqlDbType.VarChar).Value = item.estadoReporte;            
            cmd.Parameters.Add("@fechaFinalizacionVar", System.Data.SqlDbType.Date).Value = item.fechaFinalizacion;
            cmd.Parameters.Add("@descripcionVar", System.Data.SqlDbType.VarChar).Value = item.descripcion;
            cmd.Parameters.Add("@establecimientoVar", System.Data.SqlDbType.VarChar).Value = item.establecimiento;
            cmd.Parameters.Add("@idUsuarioVar", System.Data.SqlDbType.VarChar).Value = item.nombreUsuario;            

            int respuestaQuery = cmd.ExecuteNonQuery();

            con.Close();
            return (respuestaQuery == 1);
        }


        public List<Reporte> obtenerReporteUsuario(string nombreU)
        {
            List<Reporte> lista = new List<Reporte>();
            Reporte registroUSuario = null;

            SqlConnection con = new SqlConnection(conexionIP);
            con.Open();

            string sql = "exec selectListaDeReportes @nombreU";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.Add("@nombreU", System.Data.SqlDbType.VarChar).Value = nombreU;

            SqlDataReader reader =
                cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

            while (reader.Read())
            {                
                registroUSuario = new Reporte();
                registroUSuario.id = reader.GetInt32(0);
                registroUSuario.estadoReporte = reader.GetString(1);
                if (!reader.IsDBNull(2))
                {
                    registroUSuario.prioridadReporte = reader.GetString(2);
                }
                else
                {
                    registroUSuario.prioridadReporte = "";
                }
                registroUSuario.fechaReporte = reader.GetDateTime(3).ToString("yyyy/MM/dd");
                registroUSuario.fechaFinalizacion = reader.GetDateTime(4).ToString("yyyy/MM/dd");
                registroUSuario.descripcion = reader.GetString(5);
                registroUSuario.establecimiento = reader.GetString(6);
                registroUSuario.nombreUsuario = reader.GetString(7);
                registroUSuario.nombre = reader.GetString(8);
                lista.Add(registroUSuario);
            }
            reader.Close();
            return lista;
        }

        //cancelar reporte
        public bool cambiarEstadoReporte(int idReporte, string estado)
        {
            SqlConnection con = new SqlConnection(conexionIP);
            con.Open();             
            string sql = "update reporte set estadoReporte= @estado where id= @idReporte";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.Add("@idReporte", System.Data.SqlDbType.Int).Value = idReporte;
            cmd.Parameters.Add("@estado", System.Data.SqlDbType.VarChar).Value = estado;
            int respuestaQuery = cmd.ExecuteNonQuery();

            con.Close();
            return (respuestaQuery == 1);
        }


        //obtiene la lista de reportes pertenecientes a un estado en especifico ("enProceso","nuevo", y demas)
        public List<Reporte> obtenerListaReportesSegunEstado(string estadoReporte)
        {
            List<Reporte> lista = new List<Reporte>();
            SqlConnection con = new SqlConnection(conexionIP);
            con.Open();

            string sql = "exec selectReportesEspecificos @estadoReporte";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.Add("@estadoReporte", System.Data.SqlDbType.VarChar).Value = estadoReporte;

            SqlDataReader reader =
                cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

            while (reader.Read())
            {
                Reporte registroReporte = new Reporte();

                registroReporte = new Reporte();
                registroReporte.id = reader.GetInt32(0);
                registroReporte.estadoReporte = reader.GetString(1);
                if (!reader.IsDBNull(2))
                {
                    registroReporte.prioridadReporte = reader.GetString(2);
                }
                else
                {
                    registroReporte.prioridadReporte = "";
                }
                registroReporte.fechaReporte = reader.GetDateTime(3).ToString("yyyy/MM/dd");
                registroReporte.fechaFinalizacion = reader.GetDateTime(4).ToString("yyyy/MM/dd"); ;
                registroReporte.descripcion = reader.GetString(5);
                registroReporte.establecimiento = reader.GetString(6);
                registroReporte.nombreUsuario = reader.GetString(7);
                registroReporte.nombre = reader.GetString(8);


                lista.Add(registroReporte);
            }
            reader.Close();
            return lista;
        }



        public Reporte obtenerReporteInformacion(int idReporte)
        {
            Reporte registroReporte = null;
            SqlConnection con = new SqlConnection(conexionIP);
            con.Open();

            string sql = "exec selectReporte @idReporte";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.Add("@idReporte", System.Data.SqlDbType.Int).Value = idReporte;

            SqlDataReader reader =
                cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

            if (reader.Read())
            {

                registroReporte = new Reporte();                
                registroReporte.id = reader.GetInt32(0);
                registroReporte.estadoReporte = reader.GetString(1);
                if (!reader.IsDBNull(2))
                {
                    registroReporte.prioridadReporte = reader.GetString(2);
                }
                else
                {
                    registroReporte.prioridadReporte = "";
                }
                registroReporte.fechaReporte = reader.GetDateTime(3).ToString("yyyy/MM/dd");
                registroReporte.fechaFinalizacion = reader.GetDateTime(4).ToString("yyyy/MM/dd"); ;
                registroReporte.descripcion = reader.GetString(5);
                registroReporte.establecimiento = reader.GetString(6);
                registroReporte.nombreUsuario = reader.GetString(7);
                registroReporte.nombre = reader.GetString(8);

            }                            
            reader.Close();
            return registroReporte;
        }

        //obtiene la lista de reportes con proriedad, tomando en consideracion un filtro de importancia("fecha",'Admin')
        public List<Reporte> obtenerListaReportesPriorizadosFiltro(string tipo)
        {
            List<Reporte> lista = new List<Reporte>();
            SqlConnection con = new SqlConnection(conexionIP);
            con.Open();

            string sql = "exec selectReportesPrioritariosFiltro @tipoFiltro";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.Add("@tipoFiltro", System.Data.SqlDbType.VarChar).Value = tipo;

            SqlDataReader reader =
                cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

            while (reader.Read())
            {
                Reporte registroReporte = new Reporte();

                registroReporte = new Reporte();
                registroReporte.id = reader.GetInt32(0);
                registroReporte.estadoReporte = reader.GetString(1);
                if (!reader.IsDBNull(2))
                {
                    registroReporte.prioridadReporte = reader.GetString(2);
                }
                else
                {
                    registroReporte.prioridadReporte = "";
                }
                registroReporte.fechaReporte = reader.GetDateTime(3).ToString("yyyy/MM/dd");
                registroReporte.fechaFinalizacion = reader.GetDateTime(4).ToString("yyyy/MM/dd"); ;
                registroReporte.descripcion = reader.GetString(5);
                registroReporte.establecimiento = reader.GetString(6);
                registroReporte.nombreUsuario = reader.GetString(7);
                registroReporte.nombre = reader.GetString(8);


                lista.Add(registroReporte);
            }
            reader.Close();
            return lista;
        }


        // asigna un tecnico a un reporte
        public bool asignarTecnicoReporte(int idReporte, string idUsuario)
        {
            SqlConnection con = new SqlConnection(conexionIP);
            con.Open();
            string sql = "EXEC insertarUsuariosReporte  @idReporte,@idUsuario,@rol";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.Add("@idReporte", System.Data.SqlDbType.Int).Value = idReporte;
            cmd.Parameters.Add("@idUsuario", System.Data.SqlDbType.VarChar).Value = idUsuario;
            cmd.Parameters.Add("@rol", System.Data.SqlDbType.VarChar).Value = "Tecnico";

            int respuestaQuery = cmd.ExecuteNonQuery();

            con.Close();
            return (respuestaQuery == 1);
        }


        // asigna un tecnico a un reporte

        public bool actualizarPrioridad(int idReporte, string fechaFinalizacion, string nivelPrioridad)
        {
            SqlConnection con = new SqlConnection(conexionIP);
            con.Open();
            string sql = "EXEC modificarFechaYPrioridadReporte @idReporte,@nivelPrioridad,@fechaFinalizacion";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.Add("@idReporte", System.Data.SqlDbType.Int).Value = idReporte;
            cmd.Parameters.Add("@nivelPrioridad", System.Data.SqlDbType.VarChar).Value = nivelPrioridad;
            cmd.Parameters.Add("@fechaFinalizacion", System.Data.SqlDbType.Date).Value = fechaFinalizacion;
            int respuestaQuery = cmd.ExecuteNonQuery();

            con.Close();
            return (respuestaQuery == 1);
        }
    }
}