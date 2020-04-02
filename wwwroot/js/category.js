var dataTable;

$(document).ready(function () {
    loadList();
});

function loadList() {

    dataTable = $('#DT_load').DataTable(
        {
            "ajax": {
                "url": "/api/category",
                "type": "GET",
                "datatype": "json"
            },

            "columns": [
                { "data": "name", "width": "40%" },
                { "data": "displayOrder", "width": "30%" },
                {
                    "data": "id",
                    "render": function (data) {
                        return ` <div class ="text-center">
                            <a href="/Admin/category/upsert?id=${data}" 
                                class="btn btn-success text-white" style="cursor:pointer; width:100px;">
                                <i class="far fa-edit">&nbsp</i>Edit
                                </a>

                            <a class="btn btn-danger text-white" style="cursor:pointer; width:100px;" 
                                onclick=Delete('/api/category/'+${data}>
                                <i class="far fa-trash-alt">&nbsp</i>Delete
                                </a>
                        </div>`;

                    }, "width": "30%"
                }
            ],
            "language": {
                "emptyTable": "no data found."
            },
            "width": "100%"
        });

}

function Delete(url) {
    swal({
        title: "Are you sure you want to delete",
        text: "Tou cannot retrieve data!",
        icon: "warning",
        buttons: true,
        dangerMode: true

    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: 'DELETE',
                url: url,
                success: function(data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.read();
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}
