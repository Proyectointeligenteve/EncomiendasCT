function Cargar() {
    CargarRoles();
    CargarTabla();
    EventosListado()
}

function CargarTabla() {
    var giRedraw = false;
    var responsiveHelper;
    var breakpointDefinition = {
        tablet: 1024,
        phone: 480
    };
   
    tableElement = $('#tbDetails')

    tableElement.dataTable({
        "bProcessing": true,
        "bServerSide": false,
        "sAjaxSource": "lst_usuarios.aspx?fn=cargar",
        "sPaginationType": "full_numbers",
        "bAutoWidth": false,
        "bLengthChange": false,
        "oLanguage": {
            "sInfo": "_TOTAL_ Registro(s)",
            "sInfoFiltered": " - de _MAX_ registros",
            "sInfoThousands": ",",
            "sLengthMenu": "Mostrar _MENU_ Registros",
            "sLoadingRecords": "<img src='img/loading2.gif' />",
            "sProcessing": "",
            "sSearch": "",
            "sZeroRecords": "No se encontraron registros",
            "oPaginate": {
                "sNext": " <span class='glyphicon glyphicon-chevron-right' /> ",
                "sPrevious": " <span class='glyphicon glyphicon-chevron-left' / ",
                "sFirst": " << ",
                "sLast": " >> "
            }
        },
        "sDom": 'frt<"izq"i><"der"p>',
        //"oTableTools": {
        //    "sSwfPath": "swf/copy_csv_xls_pdf.swf"
        //},
        fnPreDrawCallback: function () {
            // Initialize the responsive datatables helper once.
            if (!responsiveHelper) {
                responsiveHelper = new ResponsiveDatatablesHelper(tableElement, breakpointDefinition);
            }
        },
        fnRowCallback  : function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            responsiveHelper.createExpandIcon(nRow);
        },
        fnDrawCallback : function (oSettings) {
            responsiveHelper.respond();
        },
        "aoColumns": [
            { "mDataProp": "Nombre" },
            { "mDataProp": "Rol" },
            { "mDataProp": "Usuario" },
            { "mDataProp": "Activo" }
        ]
    });

    $(".first.paginate_button, .last.paginate_button").hide();
    var search_input = tableElement.closest('.dataTables_wrapper').find('div[id$=_filter] input');
    search_input.attr('placeholder', "Buscar");
}

function EventosListado()
{
    $("#tbDetails tbody").click(function (event) {

        $(tableElement.fnSettings().aoData).each(function () {
            $(this.nTr).removeClass('row_selected');
        });
        $(event.target.parentNode).addClass('row_selected');

    });

}
function CargarRoles() {
    $.ajax({
        type: "POST",
        url: "lst_usuarios.aspx?fn=cargar_roles",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $.each(response.aaData, function (i, item) {
                if (item.id == 0) {
                    $("#rol").append("<option value=" + item.id + " selected='selected'>" + item.des + "</option>");
                }
                else {
                    $("#rol").append("<option value=" + item.id + ">" + item.des + "</option>");
                }
            });
            $("#rol").val($("#hdf_RolId").val());
        },
        error: function () {
            $("#dv_mensaje").hide();
            $("#dv_error").html('Error de comunicación con el servidor al intentar cargar las formas de pago.');
            $("#dv_error").show();
            setTimeout(function () { $('#dv_error').hide(); }, 10000);
        }
    });
}

function Validar() {
    var valido = $("#form1").valid();
    return valido;
}

function Nuevo() {
    $('#id').val(0);
    $('#Nombre').val('');
    $('#Apellido').val('');
    $('#Usuario').val('');
    $('#Clave').val('');
    $('#Email').val('');
    $('#Activo').prop('checked', true);
    $('#cambiarclave').prop('checked', true);
    $('#rol').val(0);
    $.ajax({
        success: function (response) {
            $('#Nombre').focus();
            modal = $.remodal.lookup[$('[data-remodal-id=modal]').data('remodal')];
            modal.open();
        },
        error: function () {
            $('#basicModal').hide();
            $("#dv_mensaje").hide();
            $("#dv_error").html('Error de comunicación con el servidor. El registro no ha sido actualizado.');
            $("#dv_error").show();
            $('#basicModal').modal('hide');
        }
    });
}

function Editar() {
    var id = "";
    $('#tbDetails tr').each(function () {
        if ($(this).hasClass('row_selected')) {
            id = this.id;
            $("#id").val(id);
        }
    });

    $.ajax({
        type: "POST",
        url: "lst_usuarios.aspx?fn=editar",
        data: '{"id":"' + id  + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response.rslt == 'exito') {
                $('#Nombre').val(response.nombre);
                $('#Apellido').val(response.apellido);
                $('#Usuario').val(response.usuario);
                $('#Clave').val(response.clave);
                $('#rol').val(response.rol);
                $('#hdf_RolId').val(response.rol);
                $('#Email').val(response.email);
                $('#Activo').prop('checked', response.activo.toString().toLowerCase() == "false" ? false : true);
                $('#cambiarclave').prop('checked', response.cambiarClave.toString().toLowerCase() == "false" ? false : true);
                modal = $.remodal.lookup[$('[data-remodal-id=modal]').data('remodal')];
                modal.open();
            }
            else {
                $("#dv_mensaje").hide()
                $("#dv_error").html(response.msj);
                $("#dv_error").show();
                setTimeout(function () { $('#dv_error').hide(); }, 10000);
            }
        },
        error: function () {
            $('#basicModal').hide();
            $("#dv_mensaje").hide();
            $("#dv_error").html('Error de comunicación con el servidor. El registro no ha sido actualizado.');
            $("#dv_error").show();
            $('#basicModal').modal('hide');
        }
    });
        
}

function Guardar() {
        var registro = {};
        registro.id = $('#id').val();
        registro.nombre = $('#Nombre').val();
        registro.apellido = $('#Apellido').val();
        registro.usuario = $('#Usuario').val();
        registro.clave = $('#Clave').val();
        registro.email = $('#Email').val();
        registro.rol = $('#rol').val();
        registro.activo = $('#Activo').val();
        registro.cambiarclave = $('#cambiarclave').val();
        $.ajax({
            type: "POST",
            url: "lst_usuarios.aspx?fn=guardar",
            data: JSON.stringify(registro),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                modal.close();
                if (response.rslt == 'exito') {
                    $("#dv_error").hide()
                    $("#dv_mensaje").html('El registro ha sido procesado con exito.');
                    $("#dv_mensaje").show();
                    setTimeout(function () { $('#dv_mensaje').hide(); }, 10000);
                }
                else {
                    $("#dv_mensaje").hide()
                    $("#dv_error").html(response.msj);
                    $("#dv_error").show();
                    setTimeout(function () { $('#dv_error').hide(); }, 10000);
                }
                $('#tbDetails').dataTable().fnDestroy();
                CargarTabla();
            },
            error: function () {
                $("#dv_mensajeUsuario").hide();
                $("#dv_errorUsuario").html('Error de comunicación con el servidor. El registro no ha sido actualizado.');
                $("#dv_errorUsuario").show();
                modal.close();
            }
        });
}

function Confirmar() {
    var options = {
        "backdrop": "static",
        "keyboard": "true"
    }
    $('#deleteModal').modal(options);
}

function Eliminar() {
    var id
    $('#tbDetails tr').each(function () {
        if ($(this).hasClass('row_selected')) {
            id = this.id;
            $("#id").val(id);
        }
    });

    $.ajax({
        type: "POST",
        url: "lst_usuarios.aspx?fn=eliminar",
        data: '{"id":"' + id + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $('#deleteModal').modal('hide');
            if (response.rslt == 'exito') {
                $("#dv_error").hide()
                $("#dv_mensaje").html('El registro ha sido eliminado.');
                $("#dv_mensaje").show();
                setTimeout(function () { $('#dv_mensaje').hide(); }, 10000);
                $('#tbDetails').dataTable().fnDestroy();
                CargarTabla();
            }
            else {
                $("#dv_mensaje").hide()
                $("#dv_error").html(response.msj);
                $("#dv_error").show();
                setTimeout(function () { $('#dv_error').hide(); }, 10000);
            }
        },
        error: function () {
            $('#basicModal').hide();
            $("#dv_mensaje").hide();
            $("#dv_error").html('Error de comunicación con el servidor. El registro no ha sido actualizado.');
            $("#dv_error").show();
            $('#basicModal').modal('hide');
        }
    });

}