<%@ Page Title="" Language="VB" MasterPageFile="~/principal.master" AutoEventWireup="false" CodeFile="lst_usuarios.aspx.vb" Inherits="lst_usuarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css" title="currentStyle">
        @import "css/jquery.dataTables.css";
        /*@import "css/lst_envios.css";*/
        @media (min-width:400px) {
            .control-label {
                white-space: nowrap !important;
                text-align: left !important;
            }

            .form-control {
                margin-left: 25px !important;
                width: 250px !important;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container">
        <div class="izq"><span style="font-size: 14px;color:white">LISTADO DE USUARIOS</span></div>
        <div class="hr">
            <hr />
        </div>
        <div class="der">
            <div class="btn-group">
                <button class="btn btn-default" onclick="Nuevo();"><span class="glyphicon glyphicon-plus"></span>&nbsp;Nuevo</button>
                <button class="btn btn-default" onclick="Editar();"><span class="glyphicon glyphicon-edit"></span>&nbsp;Editar</button>
                <button class="btn btn-default" onclick="Confirmar();"><span class="glyphicon glyphicon-remove"></span>&nbsp;Eliminar</button>
            </div>
        </div>
        <%--        <div class="navbar navbar-default" role="navigation">
            <div class="container">
                <div class="navbar-header">
                    <button type="button" class="navbar-toggle" data-toggle="collapse" data-target="#navbar-collapse2">
                        <span class="sr-only">Acciones</span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                    </button>
                    <a class="navbar-brand" href="#"></a>
                </div>
                <div id="navbar-collapse2" class="navbar-collapse collapse">
                    <ul class="nav navbar-nav navbar-right">
                        <li><a href="#" onclick="Nuevo();">Nuevo</a></li>
                        <li><a href="#" onclick="Editar();">Editar</a></li>
                        <li><a href="#" onclick="Confirmar();">Eliminar</a></li>
                    </ul>
                </div>
                <!--/.nav-collapse -->
            </div>
            <!--/.container-fluid -->
        </div>--%>
        <style>
            @media (max-width:400px) {
                .btn-group {
                    width: 100% !important;
                }

                .btn {
                    width: 33.33% !important;
                }
            }
        </style>

        <div class="alert alert-danger" id="dv_error" name="dv_error">
        </div>
        <div class="alert alert-success" id="dv_mensaje" name="dv_mensaje">
        </div>
    </div>
    <div class="container" style="margin-top: 10px">
        <table id="tbDetails" cellpadding="0" cellspacing="0" border="0" class="table table-bordered table-striped" style ="background-color :white !important"">
            <thead>
                <tr>
                    <td  data-class="expand">Nombre</td>
                    <td  data-hide="phone,tablet">Rol</td>
                    <td  data-hide="phone,tablet">Usuario</td>
                    <td  data-hide="phone,tablet">Activo</td>
                </tr>
            </thead>
            <tbody>
            </tbody>
        </table>
    </div>
    <div class="remodal" data-remodal-id="modal" style="background-color:#013b63;font-size:14px !important">

        <div class="modal-header">
            <h4 class="modal-title" id="myModalLabel">Formulario de Usuarios</h4>
        </div>
        <div class="modal-body">
            <form id="form1" class="form-horizontal" role="form">
                <input type="hidden" id="id" name="id" />

                <div class="row-fluid">
                    <div class="span6">
                        <div class="control-group">
                            <label class="col-sm-2 control-label" for="Nombre">Nombre</label>
                            <div class="col-sm-10">
                                <input type="text" id="Nombre" name="Nombre" class="form-control" />
                            </div>
                        </div>
                    </div>
                    <!--/span-->

                    <div class="span6">
                        <div class="control-group">
                            <label class="col-sm-2 control-label" for="Apellido">Apellido</label>
                            <div class="col-sm-10">
                                <input type="text" id="Apellido" name="Apellido" class="form-control" />
                            </div>
                        </div>
                    </div>
                    <!--/span-->
                </div>
                <!--/row-->

                <div class="row-fluid">
                    <div class="span6">
                        <div class="control-group">
                            <label class="col-sm-2 control-label" for="Usuario">Usuario</label>
                            <div class="col-sm-10">
                                <input type="text" id="Usuario" name="Usuario" class="form-control" />
                            </div>
                        </div>
                    </div>
                    <!--/span-->

                    <div class="span6">
                        <div class="control-group">
                            <label class="col-sm-2 control-label" for="Clave">Clave</label>
                            <div class="col-sm-10">
                                <input type="password" id="Clave" name="Clave" class="form-control" />
                            </div>
                        </div>
                    </div>
                    <!--/span-->
                </div>
                <!--/row-->

                <div class="row-fluid">
                    <div class="span6">
                        <div class="control-group">
                            <label class="col-sm-2 control-label" for="rol">Rol</label>
                            <div class="col-sm-10">
                                <select id="rol" name="rol" class="form-control"></select>
                                <input type="hidden" id="hdf_RolId" name="hdf_RolId" value="0" />
                            </div>
                        </div>
                    </div>
                    <!--/span-->

                    <div class="span6">
                        <div class="control-group">
                            <label class="col-sm-2 control-label" for="Email">Email</label>
                            <div class="col-sm-10">
                                <input type="text" id="Email" name="Email" class="form-control" />
                            </div>
                        </div>
                    </div>
                    <!--/span-->
                </div>
                <!--/row-->

                <div class="row-fluid">

                    <div class="span6">
                        <div class="control-group">
                            <label class="col-sm-2 control-label" for="Unidad">Activo</label>
                            <div class="col-sm-10">
                                <input type="checkbox" id="Activo" name="Activo" class="form-control" />
                            </div>
                        </div>
                    </div>
                    <!--/span-->

                    <div class="span6">
                        <div class="control-group">
                            <label class="col-sm-2 control-label" for="Estatus">Cambiar Clave</label>
                            <div class="col-sm-10">
                                <input type="checkbox" id="cambiarclave" name="cambiarclave" class="form-control" />
                            </div>
                        </div>
                    </div>
                    <!--/span-->
                </div>
                <!--/row-->
            </form>
            <br />
            <hr />
        </div>
        <div class="modal-footer">
                <div class="alert alert-danger" id="dv_errorUsuario" name="dv_errorUsuario">
                </div>
                <div class="alert alert-success" id="dv_mensajeUsuario" name="dv_mensajeUsuario">
                </div>
                <div class="alert alert-warning" id="dv_advertenciaUsuario" name="dv_advertenciaUsuario">
                </div>
            <button type="button" class="btn btn-default" data-dismiss="modal" onclick="modal.close()">Cerrar</button>
            <button type="button" class="btn btn-primary" onclick="if(Validar()){Guardar()}">Aceptar</button>
        </div>
        <%--            </div>
        </div>
    </div>--%>
    </div>
    <script type="text/javascript">
        $(function () {
            $('#form1').validate({
                rules: {
                    Nombre: {
                        required: true
                    },
                    Apellido: {
                        required: true
                    },
                    Rol:{
                        required:true
                    },
                    Usuario: {
                        required: true
                    },
                    Clave: {
                        required: true
                    },
                    Email: {
                        required: true
                    },

                }
            });

            jQuery.validator.addMethod('select', function (value) {
                return (value != '0');
            }, "seleccione una opción");
        });
    </script>

    <div class="modal fade" id="deleteModal" tabindex="-1" role="dialog" aria-labelledby="deleteModal" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">x</button>
                    <h4>Eliminar</h4>
                </div>
                <div class="modal-body">
                    <p>Estas seguro que deseas eliminar el registro?</p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                    <button type="button" class="btn btn-danger" onclick="Eliminar()">Aceptar</button>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript" language="javascript" src="js/lst_usuarios.js?token=<% Response.Write(Replace(Format(Date.Now, "yyyyMMddHH:mm:ss"), ":", ""))%>"></script>
    <script>

        $(document).ready(function () {
            $("#dv_mensaje").hide();
            $("#dv_error").hide();

            $("#dv_errorUsuario").hide();
            $("#dv_mensajeUsuario").hide();
            $("#dv_advertenciaUsuario").hide();
            Cargar();
        });

    </script>
</asp:Content>
