document.addEventListener('DOMContentLoaded', function () {

    var addedParts = [];

    var allPartsTable = document.getElementById('allPartsTable');

    var addedPartsTable = document.getElementById('addedPartsTable');

    var formData;

    var form;

    var truckselect = document.getElementById('truckNameSelect');
    var jobselect = document.getElementById('jobNumbersSelect');

    function renderTable() {
        //Grab table
        const tableBody = document.querySelector('#addedPartsTable tbody');
        tableBody.innerHTML = ''; // Clear existing rows

        //Take In Part
        //Create Table
        addedParts.forEach((part, index) => {
            const row = document.createElement('tr');

            // Part Name Cell
            const partNameCell = document.createElement('td');
            partNameCell.textContent = part.PartNumber;
            row.appendChild(partNameCell);

            // Ordered Cell (Editable)
            const orderedCell = document.createElement('td');
            const orderedInput = document.createElement('input');
            orderedInput.type = 'number';
            orderedInput.value = part.Ordered;
            orderedInput.addEventListener('change', function () {
                updateOrdered(index, orderedInput.value);
            });
            orderedCell.appendChild(orderedInput);
            row.appendChild(orderedCell);

            // Actions Cell
            const actionsCell = document.createElement('td');
            const removeButton = document.createElement('button');
            removeButton.textContent = 'Remove';
            removeButton.addEventListener('click', function () {
                removePart(index);
            });
            actionsCell.appendChild(removeButton);
            row.appendChild(actionsCell);

            // Append row to the table
            tableBody.appendChild(row);
        });
    }


    // Function to update the 'Ordered' quantity in the addedParts array
    function updateOrdered(index, value) {
        addedParts[index].Ordered = parseInt(value);
    }










    $(document).on('submit', '.addpart-po-form', function (event) {
        event.preventDefault();

        form = $(this);

        partName = form.find('input[name="PartNumber"]').val();

        partId = form.find('input[name="PartId"]').val();

        formData = form.serialize();

        const part = {
            PartId: parseInt(partId),
            PartNumber: partName,
            Ordered: 0
        };

        // Check if the part is already in the list
        const partExists = addedParts.some(function (existingPart) {
            return existingPart.PartId === partId;
        });

        if (partExists) {
            //Do Not push or render
            console.log();
        }
        else {
            addedParts.push(part);

            renderTable();
        }

    });


    function removePart() {


    }


    $('#submitbn').on('click', function () {

        var truckinstance = M.FormSelect.getInstance(truckselect).input.value;

        var jobinstance = M.FormSelect.getInstance(jobselect).input.value;



        const purchaseOrderViewModel = {
            POName: $('#poNumber').val(),
            SelectedJob: jobinstance,
            SelectedTruck: truckinstance,
            parts: addedParts
        }

        $.ajax({
            type: "POST",
            url: "/PurchaseOrders/UploadPurchaseOrder",
            contentType: 'application/json',
            data: JSON.stringify(purchaseOrderViewModel),
            success: function (response) {
                console.log('Parts submitted successfully.');
            },
            error: function (xhr, status, error) {
                console.error('Error submitting parts:', error);
            }

        });

        //// Optionally send the form data via AJAX
        //$.ajax({
        //    type: 'POST',
        //    url: "/PurchaseOrders/Index",
        //    data: formData,
        //    success: function (response) {
        //        console.log('Form submitted successfully:', response);


        //        if (funcType.trim() == "3") {
        //            // Redirect to the Returns controller's Index action
        //            window.location.href = '/Returns/Index';
        //        }
        //        else {
        //            $('#divPartial').html(response);
        //            // Optionally update the partial view with the new data
        //            UploadResult();
        //        }
        //        // Reinitialize modals after updating the partial view
        //        // Initialize all modal elements within the updated partial view

        //    },
        //    error: function (jqXHR, textStatus, errorThrown) {
        //        UploadResult();

        //        console.error('Error submitting form:', textStatus, errorThrown);
        //    }
        //});

    }); 

});