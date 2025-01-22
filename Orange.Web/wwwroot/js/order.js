var dataTable;

$(document).ready(function () {
    var url = window.location.search;
    if (url.includes("approved")) {
        loadDataTable("approved");
    }
    else {
        if (url.includes("readyforpickup")) {
            loadDataTable("readyforpickup");
        }
        else {
            if (url.includes("cancelled")) {
                loadDataTable("cancelled");
            }
            else {
                loadDataTable("all");
            }
        }
    }
});

function loadDataTable(status) {

    $.get("/order/getAllOrders?status=Pending", function(response) {
        console.log(response);
    });
    
    dataTable = $('#tblData').DataTable({
        //order: [[0, 'desc']],
        columnDefs: [
            {
                searchable: false,
                orderable: false,
                targets: 0
            }
        ],
        
        "ajax": { url: "/order/getAllOrders?status=" + status },
        "columns": [
            //{ "data": , width: "20%" },
            { data: 'email', "width": "25%" },
            { data: 'firstName', "width": "20%" },
            //{ data: 'lastName', "width": "10%" },
            { data: 'phoneNumber', "width": "10%" },
            { data: 'status', "width": "10%" },
            { data: 'orderTotal', "width": "10%" },
            {
                data: 'orderHeaderId',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                    <a href="/order/detail?orderId=${data}" class="btn btn-primary mx-2"><i class="bi bi-pencil-square"></i></a>
                    </div>`
                },
                "width": "10%"
            }
        ],
    })

    
}