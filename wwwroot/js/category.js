
var dataTable;

$(document).ready(function () {
    loadList();
});

function loadList() {

    dataTable = $("#DT_load").dataTable(
        {
            "ajax": {
                "url": "/api/category",
                "type": "GET",
                "datatype": "json"
            },

            "columns": [
                { "data": "name", "width": "40%" },
                { "data": "name", "width": "40%" },
                {
                    "data": "id",
                    "render": function(data) {
                        return ` <div class ="text-center">
                            <a href="/Admin/category/upsert?"id=${data}" 
                            class="btn btn-danger text-white style="cursor:pointer; width:100px;">
                                <i class="far fa-trash-alt"></i>
                                </a>
                        </div>`;

                    },
                    "width": "30%"
                }
            ],
            "language": {
                "emptyTable": "no data found."
            },
            "width": "100%"
        });

}
