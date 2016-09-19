Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports ac_Funciones

Public Class cls_envios_seguimientos

#Region "VARIABLES"
    Dim var_Nombre_Tabla As String = "tbl_envios_seguimientos"
    Dim var_Campo_Id As String = "id"
    Dim var_Campos As String = "id_envio,fecha,observacion,id_usuario_reg,fecha_reg,estatus"
    Dim obj_Conex_int As New SqlConnection(ConfigurationManager.ConnectionStrings("connection").ConnectionString)

    Dim var_id As Integer
    Dim var_id_envio As Integer
    Dim var_fecha As Date
    Dim var_observacion As String
    Dim var_id_usuario_reg As cls_usuarios = New cls_usuarios
    Dim var_fecha_reg As Date = Now.Date
    Dim var_estatus As Integer = 0
#End Region

#Region "PROPIEDADES"
    Public Shared ReadOnly Property Nombre_Tabla() As String
        Get
            Return "tbl_envios_seguimientos"
        End Get
    End Property

    Public Shared ReadOnly Property Campo_Id() As String
        Get
            Return "id"
        End Get
    End Property
    Public Shared ReadOnly Property Listado() As String
        Get
            Return "lst_envios_seguimientos"
        End Get
    End Property
    Public ReadOnly Property Id() As Integer
        Get
            Return Me.var_id
        End Get
    End Property
    Public Property Idenvio() As Integer
        Get
            Return Me.var_id_envio
        End Get
        Set(ByVal value As Integer)
            Me.var_id_envio = value
        End Set
    End Property

    Public Property fecha() As Date
        Get
            Return Me.var_fecha
        End Get
        Set(ByVal value As Date)
            Me.var_fecha = value
        End Set
    End Property
    Public Property observacion() As String
        Get
            Return Me.var_observacion
        End Get
        Set(ByVal value As String)
            Me.var_observacion = value
        End Set
    End Property

    Public Property id_usuario_reg() As cls_usuarios
        Get
            Return Me.var_id_usuario_reg
        End Get
        Set(ByVal value As cls_usuarios)
            Me.var_id_usuario_reg = value
        End Set
    End Property
    Public Property fecha_reg() As Date
        Get
            Return Me.var_fecha_reg
        End Get
        Set(ByVal value As Date)
            Me.var_fecha_reg = value
        End Set
    End Property

    Public Property estatus() As Integer
        Get
            Return Me.var_estatus
        End Get
        Set(ByVal value As Integer)
            Me.var_estatus = value
        End Set
    End Property
#End Region

#Region "FUNCIONES"

    Sub New(Optional ByVal var_Id_int As Integer = 0)

        If var_Id_int > 0 Then
            Cargar(var_Id_int)
        End If

    End Sub

    Public Sub Cargar(ByVal var_id_int As Integer)
        Dim obj_dt_int As New DataTable
        obj_dt_int = Abrir_Tabla(Me.obj_Conex_int, "Select * from " & Me.var_Nombre_Tabla & " WHERE " & Me.var_Campo_Id & "=" & var_id_int)
        If obj_dt_int.Rows.Count > 0 Then
            Me.var_id = obj_dt_int.Rows(0).Item("id").ToString
            Me.var_id_envio = formato_Numero(obj_dt_int.Rows(0).Item("id_envio").ToString)
            Me.var_fecha = formato_Fecha(obj_dt_int.Rows(0).Item("fecha").ToString)
            Me.var_observacion = formato_Texto(obj_dt_int.Rows(0).Item("observacion").ToString)
            Me.var_id_usuario_reg = New cls_usuarios(CInt(obj_dt_int.Rows(0).Item("id_usuario_reg").ToString))
            Me.var_fecha_reg = formato_Fecha(obj_dt_int.Rows(0).Item("fecha_reg").ToString)
            Me.var_estatus = formato_Numero(obj_dt_int.Rows(0).Item("estatus").ToString)
        Else
            Me.var_id = -1
        End If
    End Sub

    Public Function Actualizar(ByRef var_Error As String) As Boolean
        If Me.var_id = 0 Then   'NUEVO
            If Not Ingresar(Me.obj_Conex_int, Me.var_Nombre_Tabla, Me.var_Campos, Sql_Texto(Me.var_id_envio) & "," & Sql_Texto(Me.var_fecha, True) & "," & Sql_Texto(Me.var_observacion) & "," & Sql_Texto(Me.var_id_usuario_reg.Id) & "," & Sql_Texto(Me.var_fecha_reg, True) & "," & Sql_Texto(Me.var_estatus), var_Error) Then
                Return False
                Exit Function
            End If
            Me.var_id = Valor_De(Me.obj_Conex_int, "select " & Me.var_Campo_Id & " from " & Me.var_Nombre_Tabla & " order by id desc")
            Return True
        ElseIf Me.var_id > 0 Then 'EDICION
            If Not ac_Funciones.Actualizar(Me.obj_Conex_int, Me.var_Nombre_Tabla, "id_envio=" & Sql_Texto(Me.var_id_envio) & ",fecha=" & Sql_Texto(Me.var_fecha, True) & ",observacion=" & Sql_Texto(Me.var_observacion) & ",estatus=" & Sql_Texto(Me.var_estatus), Me.var_Campo_Id & "=" & Me.var_id) Then
                Return False
                Exit Function
            End If
            Return True
        Else
            var_Error = "No se encontro el Registro"
            Return False
        End If
    End Function

    Public Shared Function Eliminar(ByVal var_id As Integer, ByVal obj_usuario As cls_usuarios, ByRef var_mensaje As String) As Boolean
        Dim obj_Conex_int As New SqlConnection(ConfigurationManager.ConnectionStrings("connection").ConnectionString)
        ac_Funciones.Eliminar(obj_Conex_int, cls_envios_seguimientos.Nombre_Tabla, cls_envios_seguimientos.Campo_Id & "=" & var_id)
        Return True
    End Function

    Public Shared Function Consulta(ByVal var_id As Integer, ByRef var_error As String) As DataTable
        Dim obj_Conex_int As New SqlConnection(ConfigurationManager.ConnectionStrings("connection").ConnectionString)
        Dim var_consulta As String = "Select * from " & cls_envios_seguimientos.Listado & "(" & var_id & ") order by fecha"
        Return Abrir_Tabla(obj_Conex_int, var_consulta, var_error)
    End Function

#End Region

End Class
