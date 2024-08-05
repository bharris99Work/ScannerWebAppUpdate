
$(document).ready(function () {
    // Initialize all modals
    $('.modal').modal();

    var partName
    var formData;
    var form;




 
    //Function if scanned prt
    $(document).on('submit', '.update-scanpart-form', function (event) {
        event.preventDefault();

        form = $(this);
        formData = form.serialize();

        SubmitForm();
    });

    //Function if Use part is selected
    $(document).on('submit', '.use-part-form', function (event) {
        event.preventDefault();

        form = $(this);
        formData = form.serialize();

        $('#functionType').val("4");

        SubmitForm();
    });

    //Function if Sign Off part is selected
    $(document).on('submit', '.signoff-part-form', function (event) {
        event.preventDefault();

        form = $(this);
        formData = form.serialize();

        $('#functionType').val("1");

        SubmitForm();
    });


    //Function is add part is selected
    $(document).on('submit', '.add-part-form', function (event) {
        event.preventDefault();

        form = $(this);
        formData = form.serialize();
        var selectedjob = $('#jobId').val();
        form.find('input[name="JobId"]').val(selectedjob);

        $('#functionType').val("2");

        formData = form.serialize();

        SubmitForm();
    });



    //$(document).on('submit', '.update-scanpart-form', function (event) {
    //    var result;

    //    event.preventDefault();

    //    form = $(this);
    //    formData = form.serialize();

    //    // Get the part number and info from the form
    //    partName = form.find('input[name="PartName"]').val();

    //    // Get assigned parts and available parts values
    //    var assignedParts = parseInt(form.find('input[name="AssignedParts"]').val());
    //    var availableParts = parseInt(form.find('input[name="AvailableQuantity"]').val());
    //    var totalParts = assignedParts + availableParts;

    //    //Get scan type
    //    scanType = $('#ScanType').val();
    //    console.log("Scanned Type in upload: " + scanType);
    //    //Continous = Check if can add, submit or return false
    //    if (scanType === 'Continous') {

    //        if (assignedParts <= totalParts && availableParts - 1 >= 0) {
    //            //Add Part
    //            assignedParts += 1;
    //            availableParts -= 1;
    //            form.find('input[name="AssignedParts"]').val(assignedParts);
    //            form.find('input[name="AvailableQuantity"]').val(availableParts);
    //            formData = form.serialize();

    //            //Send the form data via AJAX
    //            $.ajax({
    //                type: 'POST',
    //                url: form.attr('action'),
    //                data: formData,
    //                success: function (response) {
    //                    console.log('Form submitted successfully:', response);
    //                    // Optionally update the partial view with the new data
    //                    $('#divPartial').html(response);
    //                    result = "Succesfully Updated Part";
    //                    $('#resultStatus').text(result); 
                         
    //                },
    //                error: function (jqXHR, textStatus, errorThrown) {
    //                    console.error('Error submitting form:', textStatus, errorThrown);
    //                    result = "Failed To Update Part";
    //                    $('#resultStatus').text(result); 
    //                }
    //            });
    //        }
    //        else {
    //            //Cannot add part
    //            console.log("Not Enough Parts To Add")
    //            result = 'Not Enough Parts Available';
                
    //        }

    //        //Submit data to addpart
    //        //Display dialog with results
    //        console.log("Scan Is Continous..")
    //        $('#resultStatus').text(result); 
           
    //    }
    //    //Not Continous = Open Dialog
    //    else {
    //        //Open Dialog
            

    //        $('#inputQty').val(assignedParts);
    //        $('#maxQty').val(" / " + totalParts);

    //        $('#assignPart').click(function () {
    //            if (assignedParts <= totalParts && availableParts - 1 >= 0) {
    //                assignedParts += 1;
    //                availableParts -= 1;
    //                form.find('input[name="AssignedParts"]').val(assignedParts);
    //                form.find('input[name="AvailableQuantity"]').val(availableParts);
    //                $('#inputQty').val(assignedParts);
    //                formData = form.serialize();
    //            }
    //        });

    //        $('#removePart').click(function () {
    //            if (assignedParts >= 0 && availableParts + 1 <= totalParts) {
    //                assignedParts -= 1;
    //                availableParts += 1;
    //                form.find('input[name="AssignedParts"]').val(assignedParts);
    //                form.find('input[name="AvailableQuantity"]').val(availableParts);
    //                $('#inputQty').val(assignedParts);
    //                formData = form.serialize();
    //            }
    //        });

    //        // Set the job number to the header
    //        $("#JobPartHeader").text("Part: " + partName);

    //        // Get the instance of the quantity modal and open it
    //        var QuantityModalInst = M.Modal.getInstance($("#quantityModal"));
    //        if (QuantityModalInst) {
    //            QuantityModalInst.open();
    //        } else {
    //            console.error("Quantity modal instance is undefined.");
    //        }

    //        console.log(formData);

    //        //On Submit, Submit data
    //        console.log("Scan Not Continous....")
    //        result = "Waiting On Dialog...";
    //        $('#resultStatus').text(result); 
            
    //    }
    //});
   

    // Handle the form submission using event delegation
    //$(document).on('submit', '.update-part-form', function (event) {
    //    event.preventDefault();

    //    form = $(this);
    //    formData = form.serialize();

    //    // Get the part number and info from the form
    //    partName = form.find('input[name="PartName"]').val();

    //    // Get assigned parts and available parts values
    //    var assignedParts = parseInt(form.find('input[name="AssignedParts"]').val());
    //    var availableParts = parseInt(form.find('input[name="AvailableQuantity"]').val());

    //    var totalParts = assignedParts + availableParts;

    //    $('#inputQty').val(assignedParts);
    //    $('#maxQty').val(" / " + totalParts);

    //    $('#assignPart').click(function () {
    //        if (assignedParts <= totalParts && availableParts - 1 >= 0) {
    //            assignedParts += 1;
    //            availableParts -= 1;
    //            form.find('input[name="AssignedParts"]').val(assignedParts);
    //            form.find('input[name="AvailableQuantity"]').val(availableParts);
    //            $('#inputQty').val(assignedParts);
    //            formData = form.serialize();
    //        }
    //    });

    //    $('#removePart').click(function () {
    //        if (assignedParts >= 0 && availableParts + 1 <= totalParts) {
    //            assignedParts -= 1;
    //            availableParts += 1;
    //            form.find('input[name="AssignedParts"]').val(assignedParts);
    //            form.find('input[name="AvailableQuantity"]').val(availableParts);
    //            $('#inputQty').val(assignedParts);
    //            formData = form.serialize();
    //        }
    //    });

    //    // Set the job number to the header
    //    $("#JobPartHeader").text("Part: " + partName);

    //    // Get the instance of the quantity modal and open it
    //    var QuantityModalInst = M.Modal.getInstance($("#quantityModal"));
    //    if (QuantityModalInst) {
    //        QuantityModalInst.open();
    //    } else {
    //        console.error("Quantity modal instance is undefined.");
    //    }

    //    console.log(formData);

     
    //});




    //// Handle the form submission using event delegation
    //$(document).on('submit', '.add-part-form', function (event) {
    //    event.preventDefault();

    //    form = $(this);
    //    formData = form.serialize();

    //    if (formData) {
    //        // Optionally send the form data via AJAX
    //        $.ajax({
    //            type: 'POST',
    //            url: form.attr('action'),
    //            data: formData,
    //            success: function (response) {
    //                console.log('Form submitted successfully:', response);
    //                // Optionally update the partial view with the new data
    //                $('#divPartial').html(response);
    //            },
    //            error: function (jqXHR, textStatus, errorThrown) {
    //                console.error('Error submitting form:', textStatus, errorThrown);
    //            }
    //        });
    //    }

    //});

    // Handle the form submission using event delegation
    //$(document).on('submit', '.remove-part-form', function (event) {
    //    event.preventDefault();

    //    form = $(this);
    //    formData = form.serialize();

    //    if (formData) {
    //        // Optionally send the form data via AJAX
    //        $.ajax({
    //            type: 'POST',
    //            url: form.attr('action'),
    //            data: formData,
    //            success: function (response) {
    //                console.log('Form submitted successfully:', response);
    //                // Optionally update the partial view with the new data
    //                $('#divPartial').html(response);

                   
    //            },
    //            error: function (jqXHR, textStatus, errorThrown) {
    //                console.error('Error submitting form:', textStatus, errorThrown);
    //            }
    //        });
    //    }

    //});


    $('#updatePart').click(function () {
        if (formData) {

            $('.modal').each(function () {
                M.Modal.init(this);
            });
            console.error("Quantity modal instance is undefined.");
            //}

            // Get the instance of the quantity modal and open it
            var QuantityModalInst = M.Modal.getInstance($("#quantityModal"));
            var SearchDialogInst = M.Modal.getInstance($('#SearchDialog'));
            if (QuantityModalInst) {
                QuantityModalInst.close();
            } else {
                console.error("Quantity modal instance is undefined.");
            }
            if (SearchDialogInst) {
                SearchDialogInst.close();
            }

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
                    // Initialize all modal elements within the updated partial view
                  
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
        //var result = $('#resultText').val();

        ////Grab list of parts
        //$('#partsTable tbody tr').each(function () {

        //    var partName = $(this).find('td').eq(0).text().toLowerCase();

        //    //Search list for specific part
        //    if (partName.trim() == result.toLocaleLowerCase().trim()) {
        //        console.log("Part Found" + partName.trim());

        //        //Get List of details about the part
        //        var jobPartId = $(this).find('input[name="JobPartId"]').val();
        //        var partId = $(this).find('input[name="PartId"]').val();
        //        var partName = $(this).find('input[name="PartName"]').val();
        //        var jobId = $(this).find('input[name="JobId"]').val();
        //        var purchaseOrderId = $(this).find('input[name="PurchaseOrderId"]').val();
        //        var status = $(this).find('input[name="Status"]').val();
        //        var assignedParts = $(this).find('input[name="AssignedParts"]').val();
        //        var availableQuantity = $(this).find('input[name="AvailableQuantity"]').val();

        //        $('#JobPartId').val(jobPartId);
        //        $('#PartId').val(partId);
        //        $('#PartName').val(partName);
        //        $('#JobId').val(jobId);
        //        $('#PurchaseOrderId').val(purchaseOrderId);
        //        $('#Status').val(status);
        //        $('#AssignedParts').val(assignedParts);
        //        $('#AvailableQuantity').val(availableQuantity);

        //        $('#scanUpdatePart').click();

        //        //if ($('#continousBN').text().trim() === 'false') {
        //        //    $('#scanUpdatePart').click();
        //        //}
        //        //else {
        //        //    $('#scanAddPart').click();
        //        //}

        //        return true;
        //        console.log("Also Job Id: " + jobPartId);
        //    }

        //    console.log(partName);
        //});

        ////false
        ////Click Search button
        ////Show Search results for item
        ////TODO: ADD SEARCH FUNCITON
        //console.log("This scan was succesful: " + result);

    });


    function SubmitForm() {
        // Get the part number and info from the form
        partName = form.find('input[name="PartName"]').val();
        var funcType = $('#functionType').val();
        //Set FunctionType form variable
        form.find('input[name="FunctionType"]').val(funcType);

        var currentcount;
        var maxcount;
        var totalParts;

        switch (funcType) {
            //Sign Part
            case "1":
                currentcount = parseInt(form.find('input[name="SignedOff"]').val());
                maxcount = parseInt(form.find('input[name="AssignedParts"]').val());
                totalParts = maxcount;
                break;
            //Add Part
            case "2":
                currentcount = 0;
                maxcount = parseInt(form.find('input[name="AvailableQuantity"]').val());
                totalParts = maxcount;
                break;
            //Return Part Not Used
            case "3":
                currentcount = 0;
                maxcount = parseInt(form.find('input[name="AvailableQuantity"]').val());
                totalParts = maxcount;
                break;
            //Update Part
            case "4":
                currentcount = parseInt(form.find('input[name="AssignedParts"]').val());
                maxcount = parseInt(form.find('input[name="AvailableQuantity"]').val());
                totalParts = currentcount + maxcount;
                break;
        }



        //Dialog functions
        // Get assigned parts and available parts values
        //var assignedParts = parseInt(form.find('input[name="AssignedParts"]').val());
        //var availableParts = parseInt(form.find('input[name="AvailableQuantity"]').val());
        //var totalParts = assignedParts + availableParts;



        $('#inputQty').val(currentcount);
        $('#maxQty').val(" / " + totalParts);

        $('#assignPart').click(function () {
            //if (assignedParts +1 <= availableParts) {
            //    assignedParts += 1;
            //    //availableParts -= 1;
            //    form.find('input[name="AssignedParts"]').val(assignedParts);
            //    //form.find('input[name="AvailableQuantity"]').val(availableParts);
            //    $('#inputQty').val(assignedParts);
            //    formData = form.serialize();
            //}

            if (currentcount + 1 <= totalParts) {
                currentcount += 1;
                switch (funcType) {
                    //Sign Part
                    case "1":
                        form.find('input[name="SignedOff"]').val(currentcount);
                        break;
                    //Add Part
                    case "2":
                        form.find('input[name="AssignedParts"]').val(currentcount);
                        break;
                    //Return Part Not Used
                    case "3":
                        //currentcount = 0;
                        //maxcount = parseInt(form.find('input[name="AvailableQuantity"]').val());
                        break;
                    //Update Part
                    case "4":
                        form.find('input[name="AssignedParts"]').val(currentcount);
                        form.find('input[name="AvailableQuantity"]').val(maxcount-=1);
                        break;
                }

                $('#inputQty').val(currentcount);
                formData = form.serialize();
            }

        });

        $('#removePart').click(function () {
            if (currentcount > 0) {
                currentcount -= 1;
                switch (funcType) {
                    //Sign Part
                    case "1":
                        form.find('input[name="SignedOff"]').val(currentcount);
                        break;
                    //Add Part
                    case "2":
                        form.find('input[name="AssignedParts"]').val(currentcount);
                        break;
                    //Return Part Not Used
                    case "3":
                        //currentcount = 0;
                        //maxcount = parseInt(form.find('input[name="AvailableQuantity"]').val());
                        break;
                    //Update Part
                    case "4":
                        form.find('input[name="AssignedParts"]').val(currentcount);
                        form.find('input[name="AvailableQuantity"]').val(maxcount+=1);

                        break;
                }

                $('#inputQty').val(currentcount);
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
    }

    
   

});


