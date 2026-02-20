var dataTable;

$(document).ready(function () {
    CargarDataTable();
});

function CargarDataTable() {
    DataTable = $("#tblProductosColaborador").DataTable({
        "ajax": {
            "url": "/Colaborador/ProductosCol/GetProductos",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "id", "width": "8%" },
            {
                "data": "url", "width": "12%",
                "render": function (data) {
                    return `<img src="../../${data}" style="width:7rem; height: 5rem">`
                }
            },
            { "data": "nombre", "width": "15%" },
            { "data": "precio", "width": "10%" },
            { "data": "cantidad", "width": "10%" },
            { "data": "categoria.nombre", "width": "10%" }
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

function Detalles() {

}
