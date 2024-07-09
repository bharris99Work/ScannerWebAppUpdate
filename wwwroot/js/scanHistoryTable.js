import DataTable from 'datatables.net-dt';


document.addEventListener('DOMContentLoaded', function () {

    var dataTable = document.getElementById('datatable')

    let table = new DataTable(dataTable, {
        columnDefs: [{
            targets: 2, // Disable sorting on the last column (the buttons)
            orderable: false,
            render: function (data, type, row, meta) {
                if (type === 'display') {
                    return '<span class="truncate" title="' + data + '" onclick="toggleExpand(this)">' + data + '</span>';
                }
                return data;
            }
        }],
        responsive: true,
        paging: false,
        searching: false

    });

});
