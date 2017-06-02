using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace servidor.Models
{
    public class usuarioManager
    {
        private static string conexionIP = Globals.IP;


        //inserta a un usuario

        public bool insertarUsuario(string permisoAdmi,Usuario usuario)
        {
            SqlConnection con = new SqlConnection(conexionIP);
            con.Open();
            
            string sql = "exec insertarUsuario "
            + "@nombreUsuarioVar,"
            + "@contrasenaVar,"
            + "@nombreVar,"
            + "@apellido1Var,"
            + "@apellido2Var,"
            + "@correoVar,"
            + "@telefonoVar,"
            + "@rolVar,"
            + "@permisoVar";

            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.Add("@nombreUsuarioVar", System.Data.SqlDbType.VarChar).Value = usuario.nombreUsuario;
            cmd.Parameters.Add("@contrasenaVar", System.Data.SqlDbType.VarChar).Value = usuario.contrasena;
            cmd.Parameters.Add("@nombreVar", System.Data.SqlDbType.VarChar).Value = usuario.nombre;
            cmd.Parameters.Add("@apellido1Var", System.Data.SqlDbType.VarChar).Value = usuario.apellido1;
            cmd.Parameters.Add("@apellido2Var", System.Data.SqlDbType.VarChar).Value = usuario.apellido2;
            cmd.Parameters.Add("@correoVar", System.Data.SqlDbType.VarChar).Value = usuario.correo;
            cmd.Parameters.Add("@telefonoVar", System.Data.SqlDbType.VarChar).Value = usuario.telefono;
            cmd.Parameters.Add("@rolVar", System.Data.SqlDbType.VarChar).Value = usuario.rol;
            cmd.Parameters.Add("@permisoVar", System.Data.SqlDbType.VarChar).Value = permisoAdmi;            

            String nombre = usuario.nombreUsuario;
            int respuestaQuery = cmd.ExecuteNonQuery();
            con.Close();
            
            return (respuestaQuery == 1) ;
        }



        //obtiene la informacion de los usuarios (permite el filtro por tipo de usuario)
        public List<Usuario> obtenerListaUsuarios(string tipoPermiso)
        {
            List<Usuario> lista = new List<Usuario>();
            SqlConnection con = new SqlConnection(conexionIP);
            con.Open();

            string sql = "exec listaDeUsuario @tipoUsuario";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.Add("@tipoUsuario", System.Data.SqlDbType.VarChar).Value = tipoPermiso;

            SqlDataReader reader =
                cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

            while (reader.Read())
            {
                Usuario registroUSuario = new Usuario();

                registroUSuario = new Usuario();
                registroUSuario.nombreUsuario = reader.GetString(0);
                registroUSuario.contrasena = reader.GetString(1);
                registroUSuario.nombre = reader.GetString(2);
                registroUSuario.apellido1 = reader.GetString(3);
                registroUSuario.apellido2 = reader.GetString(4);
                registroUSuario.correo = reader.GetString(5);
                registroUSuario.telefono = reader.GetString(6);
                registroUSuario.rol = reader.GetString(7);
                registroUSuario.activo = reader.GetString(8);
                lista.Add(registroUSuario);
            }
            reader.Close();
            return lista;
        }


        //elimina un usuario
        public bool eliminarUsuario(string nombreUsuario)
        {
            SqlConnection con = new SqlConnection(conexionIP);
            con.Open();

            string sql = "exec eliminarUsuario @idUsuarioV";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.Add("@idUsuarioV", System.Data.SqlDbType.VarChar).Value = nombreUsuario;


            int respuestaQuery = cmd.ExecuteNonQuery();

            con.Close();
            return (respuestaQuery == 1);
        }


        //login usuario autentificacion

        public Usuario obtenerUsuarioLogin(string nombreU, string contr)
        {
            Usuario usuarioLog = null;

            SqlConnection con = new SqlConnection(conexionIP);
            con.Open();

            string sql = "select * from usuario where nombreUsuario = @nombreU and contrasena= @contr";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.Add("@nombreU", System.Data.SqlDbType.VarChar).Value = nombreU;
            cmd.Parameters.Add("@contr", System.Data.SqlDbType.VarChar).Value = contr;

            SqlDataReader reader =
                 cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

            if (reader.Read())
            {                
                usuarioLog = new Usuario();
                usuarioLog.nombreUsuario = reader.GetString(0);
                usuarioLog.contrasena = reader.GetString(1);
                usuarioLog.nombre = reader.GetString(2);
                usuarioLog.apellido1 = reader.GetString(3);
                usuarioLog.apellido2 = reader.GetString(4);
                usuarioLog.correo = reader.GetString(5);
                usuarioLog.telefono = reader.GetString(6);
                usuarioLog.rol = reader.GetString(7);
                usuarioLog.activo = reader.GetString(8);
            }
            reader.Close();
            return usuarioLog;
        }

        //actualiza un usuario
        public bool actualizarUsuario(string permisoAdmin,string nombreUsuarioOLD, Usuario usuario)
        {
            SqlConnection con = new SqlConnection(conexionIP);
            con.Open();

            string sql = "exec actualizarUsuario "
                + "@nombreUsuarioOldVar,"
                + "@nombreUsuarioNewVar,"
                + "@contrasenaVar,"
                + "@nombreVar,"
                + "@apellido1Var,"
                + "@apellido2Var,"
                + "@correoVar,"
                + "@telefonoVar,"
                + "@rolVar,"
                + "@permisoAdminVar";

            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.Add("@nombreUsuarioOldVar", System.Data.SqlDbType.VarChar).Value = nombreUsuarioOLD;
            cmd.Parameters.Add("@nombreUsuarioNewVar", System.Data.SqlDbType.VarChar).Value = usuario.nombreUsuario;
            cmd.Parameters.Add("@contrasenaVar", System.Data.SqlDbType.VarChar).Value = usuario.contrasena;
            cmd.Parameters.Add("@nombreVar", System.Data.SqlDbType.VarChar).Value = usuario.nombre;
            cmd.Parameters.Add("@apellido1Var", System.Data.SqlDbType.VarChar).Value = usuario.apellido1;
            cmd.Parameters.Add("@apellido2Var", System.Data.SqlDbType.VarChar).Value = usuario.apellido2;
            cmd.Parameters.Add("@correoVar", System.Data.SqlDbType.VarChar).Value = usuario.correo;
            cmd.Parameters.Add("@telefonoVar", System.Data.SqlDbType.VarChar).Value = usuario.telefono;
            cmd.Parameters.Add("@rolVar", System.Data.SqlDbType.VarChar).Value = usuario.rol;
            cmd.Parameters.Add("@permisoAdminVar", System.Data.SqlDbType.VarChar).Value = permisoAdmin;

            int respuestaQuery = cmd.ExecuteNonQuery();
            con.Close();
            return (respuestaQuery == 1);
        }


        //información de solamente un usuario

        public Usuario obtenerUsuarioInfo(string nombreU)
        {
            Usuario usuarioLog = null;

            SqlConnection con = new SqlConnection(conexionIP);
            con.Open();

            string sql = "select * from usuario where nombreUsuario = @nombreU";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.Add("@nombreU", System.Data.SqlDbType.VarChar).Value = nombreU;            
            SqlDataReader reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

            if (reader.Read())
            {
                usuarioLog = new Usuario();
                usuarioLog.nombreUsuario = reader.GetString(0);
                usuarioLog.contrasena = reader.GetString(1);
                usuarioLog.nombre = reader.GetString(2);
                usuarioLog.apellido1 = reader.GetString(3);
                usuarioLog.apellido2 = reader.GetString(4);
                usuarioLog.correo = reader.GetString(5);
                usuarioLog.telefono = reader.GetString(6);
                usuarioLog.rol = reader.GetString(7);
                usuarioLog.activo = reader.GetString(8);
            }
            reader.Close();
            return usuarioLog;
        }

    }
}