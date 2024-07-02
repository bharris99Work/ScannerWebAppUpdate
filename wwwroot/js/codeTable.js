import DataTable from 'datatables.net-dt';


document.addEventListener('DOMContentLoaded', function () {

    var dataTable = document.getElementById('datatable')

    let table = new DataTable(dataTable, {
        columnDefs: [{
            targets: -1, // Disable sorting on the last column (the buttons)
            orderable: false
        }],
        responsive: true,
        paging: false,
        searching: false

    });































});



