using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using servidor.Models;
using System.Net.Http;

using System.Web.Script.Services;
using System.Web.Services;

namespace servidor.Controllers
{
    public class usuarioController : Controller
    {
        private usuarioManager usuarioManager;
        public usuarioController()
        {
            usuarioManager = new usuarioManager();
        }

        // insertar a un usuario 
        [HttpPost]
        public JsonResult insertarUsuarioAction(string permisoAdmin, Usuario user)
        {
            return Json(usuarioManager.insertarUsuario(permisoAdmin,user));
        }        

        //obtiene lista de usuarios segun tipo de permisos
        [HttpGet]
        public JsonResult obtenerListaUsuariosAction(string tipoPermiso)
        {
            return Json(usuarioManager.obtenerListaUsuarios(tipoPermiso),
                        JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult obetenerUsuariosNoPermiso()
        {
            return Json(usuarioManager.obetenerUsuariosNoPermiso(),
                        JsonRequestBehavior.AllowGet);
        }

        //elimina un usuario de la base
        [HttpPost]
        public JsonResult eliminarUsuarioAction(string nombreUsuario)
        {
            return Json(usuarioManager.eliminarUsuario(nombreUsuario));
        }

        //actualiza la info de un usuario (se cuenta con permisos de administrador)
        [HttpPost]
        public JsonResult actualizarUsuarioAction(string permisoAdmin,string nombreUsuarioOLD, Usuario usuario)
        {
            return Json(usuarioManager.actualizarUsuario(permisoAdmin,nombreUsuarioOLD,usuario));
        }
      
        [HttpGet]
        public JsonResult obtenerUsuarioLoginAction(string nombreU, string contr, Usuario item)
        {
            switch (Request.HttpMethod)
            {
                case "GET":
                    return Json(usuarioManager.obtenerUsuarioLogin(nombreU, contr), JsonRequestBehavior.AllowGet);
            }
            return Json(new { Error = true, Message = "Operación HTTP desconocida" });
        }

        //cambiar estado de usuario a activo
        [HttpPost]
        public JsonResult cambiarUsuarioPermiso(string nombreUsuario)
        {
            return Json(usuarioManager.cambiarUsuarioPermiso(nombreUsuario));
        }


        //cambia el estado activo o inactivo de un usuario
        public bool cambiarUsuarioPermiso(string nombreUsuario)
        {
            SqlConnection con = new SqlConnection(conexionIP);
            con.Open();

            string sql = "exec cambiar @idUsuarioV";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.Add("@idUsuarioV", System.Data.SqlDbType.VarChar).Value = nombreUsuario;
            int respuestaQuery = cmd.ExecuteNonQuery();

            con.Close();
            return (respuestaQuery == 1);
        }


        [HttpGet]
        public JsonResult obtenerUsuarioInf(string nombreU, Usuario item)
        {
            switch (Request.HttpMethod)
            {
                case "GET":
                    return Json(usuarioManager.obtenerUsuarioInfo(nombreU), JsonRequestBehavior.AllowGet);
            }
            return Json(new { Error = true, Message = "Operación HTTP desconocida" });
        }


        //nuevo funciones push


        //enviar mensaje
        [HttpPost]
        public JsonResult enviarMensajePushAction(ConexionPush item)
        {
            return Json(usuarioManager.enviarMensajePush(item));
        }

        //actualizar token
        [HttpPost]
        public JsonResult actualizarTokenPushNotf(Token item)
        {
            return Json(usuarioManager.actualizarTokenPushNotf(item));
        }

        //obtener el token del usuario (id del telefono especifico)
        [HttpGet]
        public JsonResult obtenerTokenUsuarioAction(int idReporte)
        {
            return Json(usuarioManager.obtenerTokenUsuario(idReporte),
                        JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult obtenerListaTokenAdministradores()
        {
            return Json(usuarioManager.obtenerListaTokenAdministradores(),
                        JsonRequestBehavior.AllowGet);
        }


        //obtener lista de tecnicos que estab asignados a un reporte
        [HttpGet]
        public JsonResult obtenerListaTecnicosReporteAction(int idReporte)
        {
            return Json(usuarioManager.obtenerListaTecnicosReporte(idReporte),
                        JsonRequestBehavior.AllowGet);
        }
        //eliminar todos los tecnicos que estan asociados a un reporte
        [HttpPost]
        public JsonResult eliminarTecnicosReporteAction(int idReporte)
        {
            return Json(usuarioManager.eliminarTecnicosReporte(idReporte));
        }
        

    }
}
