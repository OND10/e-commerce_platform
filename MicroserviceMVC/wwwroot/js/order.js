function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            url: "/Order/GetAll",
            type: "GET",
            dataSrc: "data",
            error: function (xhr, status, error) {
                console.error("Failed to load data:", {
                    status: status,
                    error: error,
                    response: xhr.responseText
                });
            }
        },
        "columns": [
            { data: 'id', "width": "5%" },
            { data: 'email', "width": "25%" },
            { data: 'name', "width": "20%" },
            { data: 'phoneNumber', "width": "15%" },
            { data: 'status', "width": "10%" },
            { data: 'orderTotal', "width": "15%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class = "w-75 btn-group" role="group">
                    <a href ="/order/details?id=${data}" class="btn btn-primary mx-2"><i class="bi bi-pencil-square"></i></a>
                    </div>`
                },
                width: "100%"
            }
        ]
    });
}


$(document).ready(function () {
    loadDataTable();
});
