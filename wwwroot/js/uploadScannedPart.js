$(document).ready(function () {
    // Initialize all modals
    $('.modal').modal();

    var partName
    var formData;
    var form;

   

    // Handle the form submission using event delegation
    $(document).on('submit', '.update-part-form', function (event) {
        event.preventDefault();

        form = $(this);
        formData = form.serialize();

        // Get the part number and info from the form
        partName = form.find('input[name="PartName"]').val();

        // Get assigned parts and available parts values
        var assignedParts = parseInt(form.find('input[name="AssignedParts"]').val());
        var availableParts = parseInt(form.find('input[name="AvailableQuantity"]').val());

        var totalParts = assignedParts + availableParts;

        $('#inputQty').val(assignedParts);
        $('#maxQty').val(" / " + totalParts);

        $('#assignPart').click(function () {
            if (assignedParts <= totalParts && availableParts - 1 >= 0) {
                assignedParts += 1;
                availableParts -= 1;
                form.find('input[name="AssignedParts"]').val(assignedParts);
                form.find('input[name="AvailableQuantity"]').val(availableParts);
                $('#inputQty').val(assignedParts);
                formData = form.serialize();
            }
        });

        $('#removePart').click(function () {
            if (assignedParts >= 0 && availableParts + 1 <= totalParts) {
                assignedParts -= 1;
                availableParts += 1;
                form.find('input[name="AssignedParts"]').val(assignedParts);
                form.find('input[name="AvailableQuantity"]').val(availableParts);
                $('#inputQty').val(assignedParts);
                formData = form.serialize();
            }
        });

        // Set the job number to the header
        $("#JobPartHeader").text("Part: " + partName);

        // Get the instance of the quantity modal and open it
        var QuantityModalInst = M.Modal.getInstance($("#quantityModal"));
        if (QuantityModalInst) {
            QuantityModalInst.open();
        } else {
            console.error("Quantity modal instance is undefined.");
        }

        console.log(formData);

     
    });




    // Handle the form submission using event delegation
    $(document).on('submit', '.add-part-form', function (event) {
        event.preventDefault();

        form = $(this);
        formData = form.serialize();

        if (formData) {
            // Optionally send the form data via AJAX
            $.ajax({
                type: 'POST',
                url: form.attr('action'),
                data: formData,
                success: function (response) {
                    console.log('Form submitted successfully:', response);
                    // Optionally update the partial view with the new data
                    $('#divPartial').html(response);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.error('Error submitting form:', textStatus, errorThrown);
                }
            });
        }

    });

    // Handle the form submission using event delegation
    $(document).on('submit', '.remove-part-form', function (event) {
        event.preventDefault();

        form = $(this);
        formData = form.serialize();

        if (formData) {
            // Optionally send the form data via AJAX
            $.ajax({
                type: 'POST',
                url: form.attr('action'),
                data: formData,
                success: function (response) {
                    console.log('Form submitted successfully:', response);
                    // Optionally update the partial view with the new data
                    $('#divPartial').html(response);

                   
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.error('Error submitting form:', textStatus, errorThrown);
                }
            });
        }

    });


    $('#updatePart').click(function () {
        if (formData) {
            // Optionally send the form data via AJAX
            $.ajax({
                type: 'POST',
                url: form.attr('action'),
                data: formData,
                success: function (response) {
                    console.log('Form submitted successfully:', response);
                    // Optionally update the partial view with the new data
                    $('#divPartial').html(response);
                    // Reinitialize modals after updating the partial view
                    $('.modal').modal();
                    // Get the instance of the quantity modal and open it
                    var QuantityModalInst = M.Modal.getInstance($("#quantityModal"));
                    if (QuantityModalInst) {
                        QuantityModalInst.close();
                    } else {
                        console.error("Quantity modal instance is undefined.");
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.error('Error submitting form:', textStatus, errorThrown);
                }
            });
        }
    });


    // Event handler for the "Google Part" button
    $('#googlePartButton').click(function () {
        
        if (partName) {
            var googleSearchUrl = 'https://www.google.com/search?tbm=shop&q=' + encodeURIComponent(partName);
            window.open(googleSearchUrl, '_blank');
        } else {
            alert("Part number is not available.");
        }
    });


    $('#SpecificSearch').click(function () {
        var result = $('#resultText').val();

        //Grab list of parts
        $('#partsTable tbody tr').each(function () {

            var partName = $(this).find('td').eq(0).text().toLowerCase();

            //Search list for specific part
            if (partName.trim() == result.toLocaleLowerCase().trim()) {
                console.log("Part Found" + partName.trim());

                //Get List of details about the part
                var jobPartId = $(this).find('input[name="JobPartId"]').val();
                var partId = $(this).find('input[name="PartId"]').val();
                var partName = $(this).find('input[name="PartName"]').val();
                var jobId = $(this).find('input[name="JobId"]').val();
                var purchaseOrderId = $(this).find('input[name="PurchaseOrderId"]').val();
                var status = $(this).find('input[name="Status"]').val();
                var assignedParts = $(this).find('input[name="AssignedParts"]').val();
                var availableQuantity = $(this).find('input[name="AvailableQuantity"]').val();

                $('#JobPartId').val(jobPartId);
                $('#PartId').val(partId);
                $('#PartName').val(partName);
                $('#JobId').val(jobId);
                $('#PurchaseOrderId').val(purchaseOrderId);
                $('#Status').val(status);
                $('#AssignedParts').val(assignedParts);
                $('#AvailableQuantity').val(availableQuantity);

                if ($('#continousBN').text().trim() === 'false') {
                    $('#scanUpdatePart').click();
                }
                else {
                    $('#scanAddPart').click();
                }

                return true;
                console.log("Also Job Id: " + jobPartId);
            }

            console.log(partName);
        });

        //false
        //Click Search button
        //Show Search results for item
        //TODO: ADD SEARCH FUNCITON
        console.log("This scan was succesful: " + result);

    });
   

});
