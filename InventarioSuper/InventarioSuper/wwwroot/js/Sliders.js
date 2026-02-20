var dataTable;

$(document).ready(function() {
    CargarDataTable();
});

function CargarDataTable() {
    DataTable = $("#tblSliders").DataTable({
        "ajax": {
            "url": "/Admin/Sliders/GetAll",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "id", "width": "5%" },
            {
                "data": "imagen", "width": "25%",
                "render": function (data) {
                    return `<img src="../${data}" style="width:7rem; height: 5rem">`
                }
            },
            { "data": "nombre", "width": "25%" },
            {
                "data": "estado",
                "render": function (data) {
                    if (data) {
                        return `<span class="badge bg-success">Activo</span>`;
                    } else {
                        return `<span class="badge bg-danger">Inactivo</span>`;
                    }
                },
                "width": "15%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return ` <div class="text-center">
                                <a href="/Admin/Sliders/Editar/${data}" class="btn btn-success text-white" style="cursor: pointer; width:110px;">
                                    <i class="fas fa-edit"></i> Editar
                                </a>
                                &nbsp;
                                <a onclick=Delete("/Admin/Sliders/Delete/${data}") class="btn btn-danger text-white" style="cursor: pointer; width:110px;">
                                    <i class="fas fa-trash-alt"></i> Eliminar
                                </a>
                             </div>
                    `;
                },
                "width": "30%"
            }
        ],

        "language": {
            "decimal": "",
            "emptyTable": "No hay datos disponibles en la tabla",
            "info": "Mostrando _START_ a _END_ de _TOTAL_ entradas",
            "infoEmpty": "Mostrando 0 a 0 de 0 entradas",
            "infoFiltered": "(filtrado de _MAX_ entradas totales)",
            "infoPostFix": "",
            "thousands": ",",
            "lengthMenu": "Mostrar _MENU_ entradas",
            "loadingRecords": "Cargando...",
            "processing": "Procesando...",
            "search": "Buscar:",
            "zeroRecords": "No se encontraron resultados",
            "paginate": {
                "first": "Primero",
                "last": "Último",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        }, "width": "100%", 
    })
}

function Delete(url) {
    swal({
        title: "Estas seguro de borrar la categoria",
        text: "Esta acción no se puede deshacer.",
        icon: "warning",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        confirmButtonText: "Si, borrar!",
        closeOnConfirm: true

    }, function () {
        $.ajax({
            type: 'DELETE',
            url: url,
            success: function (data) {
                if (data.success) {
                    toastr.success(data.message);
                    DataTable.ajax.reload();
                } else {
                    toastr.error(data.message);
                }
            }
        });
    });
}