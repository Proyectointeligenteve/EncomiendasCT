Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Services
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Partial Class lst_usuarios
    Inherits System.Web.UI.Page

    Dim obj_Session As cls_Sesion

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If IsNothing(Session.Contents("obj_Session")) Then
            Response.Redirect("info.aspx", True)
        End If
        obj_Session = Session.Contents("obj_Session")

        If Not IsNothing(Request.QueryString("fn")) Then
            Dim var_fn As String = Request.QueryString("fn").ToString
            Select Case var_fn
                Case "cargar"
                    Cargar()
                Case "cargar_roles"
                    cargar_roles()
                Case "guardar"
                    Guardar()
                Case "editar"
                    Editar()
                Case "eliminar"
                    Eliminar()
            End Select
        End If
    End Sub

    Sub Cargar()
        Dim var_error As String = ""
        Dim obj_dt_int As System.Data.DataTable = cls_usuarios.Consulta
        Dim var_json As String = JsonConvert.SerializeObject(obj_dt_int)
        Response.Clear()
        Response.ClearHeaders()
        Response.ClearContent()
        Response.Write("{ " & Chr(34) & "aaData" & Chr(34) & ":" & var_json & "}")
        Response.End()
    End Sub

    Sub cargar_roles()
        Dim var_error As String = ""
        Dim obj_dt_int As System.Data.DataTable = cls_Roles.Lista
        Dim var_json As String = JsonConvert.SerializeObject(obj_dt_int)
        Response.Clear()
        Response.ClearHeaders()
        Response.ClearContent()
        Response.Write("{ " & Chr(34) & "aaData" & Chr(34) & ":" & var_json & "}")
        Response.End()
    End Sub
    Sub Guardar()
        Dim var_sr = New System.IO.StreamReader(Request.InputStream)
        Dim var_data As JObject = JObject.Parse(var_sr.ReadToEnd)

        Dim obj_usuario As New cls_usuarios(CInt(var_data("id")))
        obj_usuario.activo = True
        obj_usuario.apellido = var_data("apellido")
        obj_usuario.cambiar_clave = True
        obj_usuario.Clave = var_data("clave")
        obj_usuario.email = var_data("email")
        obj_usuario.fecha_reg = Now
        obj_usuario.fecha_ult = Now
        obj_usuario.id_rol = var_data("rol")
        obj_usuario.id_usuario_reg = obj_Session.Usuario.Id
        obj_usuario.nombre = var_data("nombre")
        obj_usuario.usuario = var_data("usuario")
        Response.ContentType = "application/json"
        Response.Clear()
        Response.ClearHeaders()
        Response.ClearContent()
        Dim var_error As String = ""
        If Not obj_usuario.Actualizar(var_error) Then
            Response.Write("{" & Chr(34) & "rslt" & Chr(34) & ":" & Chr(34) & "error" & Chr(34) & "," & Chr(34) & "msj" & Chr(34) & ":" & Chr(34) & var_error & Chr(34) & "}")
        Else
            Response.Write("{" & Chr(34) & "rslt" & Chr(34) & ":" & Chr(34) & "exito" & Chr(34) & "," & Chr(34) & "msj" & Chr(34) & ":" & Chr(34) & "" & Chr(34) & "}")
        End If
        Response.End()
    End Sub

    Sub Editar()
        Dim var_sr = New System.IO.StreamReader(Request.InputStream)
        Dim var_data As JObject = JObject.Parse(var_sr.ReadToEnd)

        Dim obj_usuario As New cls_usuarios(CInt(var_data("id").ToString))
        Dim obj_sb As New StringBuilder
        obj_sb.Append("," & Chr(34) & "apellido" & Chr(34) & ":" & Chr(34) & obj_usuario.apellido & Chr(34) & "")
        obj_sb.Append("," & Chr(34) & "nombre" & Chr(34) & ":" & Chr(34) & obj_usuario.nombre & Chr(34) & "")
        obj_sb.Append("," & Chr(34) & "usuario" & Chr(34) & ":" & Chr(34) & obj_usuario.usuario & Chr(34) & "")
        obj_sb.Append("," & Chr(34) & "email" & Chr(34) & ":" & Chr(34) & obj_usuario.email & Chr(34) & "")
        obj_sb.Append("," & Chr(34) & "clave" & Chr(34) & ":" & Chr(34) & obj_usuario.Clave & Chr(34) & "")
        obj_sb.Append("," & Chr(34) & "rol" & Chr(34) & ":" & Chr(34) & obj_usuario.id_rol & Chr(34) & "")
        obj_sb.Append("," & Chr(34) & "activo" & Chr(34) & ":" & Chr(34) & obj_usuario.activo & Chr(34) & "")
        obj_sb.Append("," & Chr(34) & "cambiarClave" & Chr(34) & ":" & Chr(34) & obj_usuario.cambiar_clave & Chr(34) & "")
        Response.ContentType = "application/json"
        Response.Clear()
        Response.ClearHeaders()
        Response.ClearContent()
        Dim var_error As String = ""
        Response.Write("{" & Chr(34) & "rslt" & Chr(34) & ":" & Chr(34) & "exito" & Chr(34) & "," & Chr(34) & "msj" & Chr(34) & ":" & Chr(34) & var_error & Chr(34) & obj_sb.ToString & "}")
        Response.End()
    End Sub

    Sub Eliminar()
        Dim var_sr = New System.IO.StreamReader(Request.InputStream)
        Dim var_data As JObject = JObject.Parse(var_sr.ReadToEnd)
        Dim var_error As String = ""

        Response.ContentType = "application/json"
        Response.Clear()
        Response.ClearHeaders()
        Response.ClearContent()
        If Not cls_usuarios.Eliminar(CInt(var_data("id").ToString), obj_Session.Usuario, var_error) Then
            Response.Write("{" & Chr(34) & "rslt" & Chr(34) & ":" & Chr(34) & "error" & Chr(34) & "," & Chr(34) & "msj" & Chr(34) & ":" & Chr(34) & var_error & Chr(34) & "}")
        Else
            Response.Write("{" & Chr(34) & "rslt" & Chr(34) & ":" & Chr(34) & "exito" & Chr(34) & "," & Chr(34) & "msj" & Chr(34) & ":" & Chr(34) & Chr(34) & "}")
        End If
        Response.End()
    End Sub
End Class
