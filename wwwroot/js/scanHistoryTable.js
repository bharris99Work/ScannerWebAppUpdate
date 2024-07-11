import DataTable from 'datatables.net-dt';
import jsPDF from 'jspdf';
import 'jspdf-autotable';



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
        paging: true,
        searching: false

    });

    // Print button click event
    document.getElementById('printButton').addEventListener('click', function () {
        // Get all DataTable data
        let tableData = table.rows().data().toArray();

        // Initialize jsPDF
        let doc = new jsPDF();

        // Define columns
        let columns = ['Item Number', 'Job Number', 'Changed Information', 'Date Modified'];

        // Create rows array
        let rows = [];

        // Push data into rows array
        tableData.forEach(row => {
            let rowData = [
                row[0], // Item Number
                row[1], // Job Number
                row[2], // Changed Information
                row[3]  // Date Modified
            ];
            rows.push(rowData);
        });

        // Add content to PDF
        doc.text('Scan History: [PlaceHolder Date]', 10, 10);
        doc.autoTable({
            head: [columns],
            body: rows,
            startY: 20,
            styles: { overflow: 'linebreak', columnWidth: 'wrap' },
            columnStyles: { 2: { cellWidth: 'auto' } }
        });

        // Save PDF to a Blob object
        let pdfBlob = doc.output('blob');

        // Create object URL for the Blob
        let pdfUrl = URL.createObjectURL(pdfBlob);

        // Open the PDF in a new tab
        window.open(pdfUrl, '_blank');

        // Clean up
        URL.revokeObjectURL(pdfUrl);

    });

});
