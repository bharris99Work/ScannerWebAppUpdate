import DataTable from 'datatables.net-dt';
$(document).ready(function () {


    var quantitydialog = document.getElementById('quantityModal');
    var qtydialogopt = { onCloseEnd: resetQuantityDialog };
    var qtydialoginit = M.Modal.init(quantitydialog, qtydialogopt);

    $('select').formSelect();

    var partName
    var formData;
    var form;

    var currentcount;
    var maxcount;
    var totalParts;
    var funcType;


    var returnselect = document.getElementById('returnOptions');
    var clearreturn = document.getElementById('clearReturn');
    var otherreturnbn = document.getElementById('otherReturn');
    var returnothertext = document.getElementById('returnOptionText');

    var returnSelectWrapper = document.getElementById('returnSelectWrapper');// Select the parent wrapper

    var returnotherwrapper = document.getElementById('returnOtherWrapper');

    //returnselect.value = '0';


 
    //Function if scanned prt
    $(document).on('submit', '.update-scanpart-form', function (event) {
        event.preventDefault();

        form = $(this);

        partName = form.find('input[name="PartName"]').val();
        formData = form.serialize();



        SubmitForm();
    });

    //Function if Use part is selected
    $(document).on('submit', '.use-part-form', function (event) {
        event.preventDefault();

        form = $(this);
        formData = form.serialize();

        $('#functionTypeHeader').text("Update Used");


        $('#functionType').val("4");

        SubmitForm();
    });

    //Function if Sign Off part is selected
    $(document).on('submit', '.signoff-part-form', function (event) {
        event.preventDefault();

        form = $(this);
        formData = form.serialize();

        $('#functionTypeHeader').text("Sign Parts Off:");


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

        $('#functionTypeHeader').text("Add Part To Job:");


        $('#functionType').val("2");

        formData = form.serialize();

        SubmitForm();
    });

    //Function if not used part is selected
    $(document).on('submit', '.notused-part-form', function (event) {
        event.preventDefault();

        form = $(this);
        formData = form.serialize();
        //var selectedjob = $('#jobId').val();
        //form.find('input[name="JobId"]').val(selectedjob);

        $('#functionType').val("3");

        $('#functionTypeHeader').text("Return Part:");


        formData = form.serialize();

        SubmitForm();
    });

    //Function if Checking In (Return Queue)
    $(document).on('submit', '.checkin-part-form', function (event) {
        event.preventDefault();

        form = $(this);
        formData = form.serialize();

        $('#functionTypeHeader').text("Check-In Part:");

        ReturnSubmit();
    });


    //Function to send part to database
    $('#updatePart').click(function () {
        if (formData) {
            form.find('input[name="FunctionType"]').val(funcType);

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
                    form.find('input[name="AssignedParts"]').val(currentcount);
                    break;
                //Update Part
                case "4":
                    form.find('input[name="AssignedParts"]').val(currentcount);
                    form.find('input[name="AvailableQuantity"]').val(maxcount);
                    break;
            }


            //Hide Container
            if (funcType.trim() == "3") {
                var instance = M.FormSelect.getInstance(returnselect);

                //Get and Set Return Reason
                if (instance.selectedIndex == 0 || returnselect.selectedIndex == 0) {
                    if (returnothertext.value == '' || returnothertext.value == null) {
                        console.log("Must Have Return Reason");
                    }
                    else {
                        //Other text found
                        form.find('input[name="ReturnReason"]').val(returnothertext.value);
                    }
                }
                else {
                    //select option found
                    form.find('input[name="ReturnReason"]').val(returnselect.options[returnselect.selectedIndex].text);

                }

            }
            formData = form.serialize();


         


            $('#returnElements').css("display", "none");

            // Get the instance of the quantity modal and open it
            var QuantityModalInst = M.Modal.getInstance($("#quantityModal"));
            var SearchDialogInst = M.Modal.getInstance($('#SearchDialog'));
            var TruckPartsInst = M.Modal.getInstance($('#truckPartsModal'));
            if (QuantityModalInst) {
                QuantityModalInst.close();
            } else {
                console.error("Quantity modal instance is undefined.");
            }
            if (SearchDialogInst) {
                SearchDialogInst.close();
            }
            if (TruckPartsInst) {
                TruckPartsInst.close();
            }

            // Optionally send the form data via AJAX
            $.ajax({
                type: 'POST',
                url: form.attr('action'),
                data: formData,
                success: function (response) {
                    console.log('Form submitted successfully:', response);
                    // Optionally update the partial view with the new data
                    
                    if (funcType.trim() == "3") {
                        // Redirect to the Returns controller's Index action
                        window.location.href = '/Returns/Index';
                    }
                    else {
                        $('#divPartial').html(response);

                    }
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


    //Function for updating parts (job editor)
    function SubmitForm() {
        // Get the part number and info from the form
        partName = form.find('input[name="PartName"]').val();
        funcType = $('#functionType').val();
        //Set FunctionType form variable
        //form.find('input[name="FunctionType"]').val(funcType);

        $('#JobPartHeader').text(partName);


       

        switch (funcType) {
            //Sign Part
            case "1":
                currentcount = parseInt(form.find('input[name="SignedOff"]').val());
                maxcount = parseInt(form.find('input[name="AssignedParts"]').val());
                //Hide Container
                $('#returnElements').css("display", "none");
           
                totalParts = maxcount;
                break;
            //Add Part
            case "2":
                currentcount = 0;
                maxcount = parseInt(form.find('input[name="AvailableQuantity"]').val());
                totalParts = maxcount;

                //Hide Container
                $('#returnElements').css("display", "none");
                break;
            //Return Part Not Used
            case "3":
                returnselect.value = '0';


                // Set the select to the first actual option
                returnselect.selectedIndex = 0; // Set to the first actual option
                var instance = M.FormSelect.getInstance(returnselect);
                instance.input.value = returnselect.options[0].text; // Update the displayed value

                //Display return text box, hide tech select box
                otherreturnbn.addEventListener('click', (event) => {

                    if (returnSelectWrapper.style.display === 'none') {
                        returnSelectWrapper.style.display = 'block';
                        returnotherwrapper.style.display = 'none';
                        returnothertext.value = '';
                    } else {
                        returnselect.selectedIndex = 0; // Set to the first actual option
                        var instance = M.FormSelect.getInstance(returnselect);
                        instance.input.value = returnselect.options[0].text; // Update the displayed value
                        returnSelectWrapper.style.display = 'none';
                        returnotherwrapper.style.display = 'block';
                    }
                });


                //Clears return text box and retirm select box
                clearreturn.addEventListener('click', (event) => {

                    returnSelectWrapper.style.display = 'block';

                    // Set the select to the first actual option
                    returnselect.selectedIndex = 0; // Set to the first actual option
                    var instance = M.FormSelect.getInstance(returnselect);
                    instance.input.value = returnselect.options[0].text; // Update the displayed value

                    returnotherwrapper.style.display = 'none';
                    returnothertext.value = '';
                });


                currentcount = 0;
                maxcount = parseInt(form.find('input[name="AvailableQuantity"]').val());
                totalParts = maxcount;


                //Show Container
                $('#returnElements').css("display", "block");

                //Generate Return Number PartId-POId-JobId-date ex.(8/7/2024 = 872024)
                var returnnumber;

                var partid = form.find('input[name="JobPartId"]').val();
               // var jobid = form.find('input[name="JobId"]').val();
                //var poid = form.find('input[name="PurchaseOrderId"]').val();

                // Get the current date
                const currentDate = new Date();
                // Extract month, day, year, hours, minutes, and seconds
                let month = currentDate.getMonth() + 1; // Months are zero-based, so we add 1
                let day = currentDate.getDate();
                const year = currentDate.getFullYear();
                //let hours = currentDate.getHours();
                //let minutes = currentDate.getMinutes();
                let seconds = currentDate.getSeconds();

                // Convert month, day, hours, minutes, and seconds to strings and pad with zeros if necessary
                month = month < 10 ? '0' + month : '' + month;
                day = day < 10 ? '0' + day : '' + day;
                seconds = seconds < 10 ? '0' + seconds : '' + seconds;

                // Concatenate month, day, year, hours, minutes, and seconds
                const formattedDate = `${month}${day}${year}${seconds}`;

                

                returnnumber = partid + formattedDate;

                console.log(returnnumber); // Outputs: 08072024123045 (for example)

                form.find('input[name="ReturnNumber"]').val(returnnumber);

                break;

            //Update Part
            case "4":
                currentcount = parseInt(form.find('input[name="AssignedParts"]').val());
                maxcount = parseInt(form.find('input[name="AvailableQuantity"]').val());
                totalParts = currentcount + maxcount;

                //Hide Container
                $('#returnElements').css("display", "none");
                break;
        }



        //Dialog functions
        // Get assigned parts and available parts values
        //var assignedParts = parseInt(form.find('input[name="AssignedParts"]').val());
        //var availableParts = parseInt(form.find('input[name="AvailableQuantity"]').val());
        //var totalParts = assignedParts + availableParts;



        $('#inputQty').val(currentcount);
        $('#maxQty').val(" / " + totalParts);

        $('#assignPart').on('click.assign', function () {
          
            if (currentcount + 1 <= totalParts) {
                currentcount += 1;
                maxcount -= 1;

                //switch (funcType) {
                //    //Sign Part
                //    case "1":
                //        form.find('input[name="SignedOff"]').val(currentcount);
                //        break;
                //    //Add Part
                //    case "2":
                //        form.find('input[name="AssignedParts"]').val(currentcount);
                //        break;
                //    //Return Part Not Used
                //    case "3":
                //        //currentcount = 0;
                //        form.find('input[name="AssignedParts"]').val(currentcount);
                //        break;
                //    //Update Part
                //    case "4":
                //        form.find('input[name="AssignedParts"]').val(currentcount);
                //        form.find('input[name="AvailableQuantity"]').val(maxcount-=1);
                //        break;
                //}

                $('#inputQty').val(currentcount);
                //formData = form.serialize();
            }
        });

        $('#removePart').on('click.remove', function () {
            if (currentcount > 0) {
                currentcount -= 1;
                maxcount += 1;

                //switch (funcType) {
                //    //Sign Part
                //    case "1":
                //        form.find('input[name="SignedOff"]').val(currentcount);
                //        break;
                //    //Add Part
                //    case "2":
                //        form.find('input[name="AssignedParts"]').val(currentcount);
                //        break;
                //    //Return Part Not Used
                //    case "3":
                //        //currentcount = 0;
                //        form.find('input[name="AssignedParts"]').val(currentcount);
                //        break;
                //    //Update Part
                //    case "4":
                //        form.find('input[name="AssignedParts"]').val(currentcount);
                //        form.find('input[name="AvailableQuantity"]').val(maxcount+=1);

                //        break;
                //}

                $('#inputQty').val(currentcount);
                //formData = form.serialize();
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



    //Dialog Functions for Checking In Parts
    function ReturnSubmit() {
        // Get the part number and info from the form
        partName = form.find('input[name="PartNumber"]').val();

        var returnnumber = form.find('input[name="ReturnPartNumber"]').val();

        var jobnumber = form.find('input[name="JobNumber"]').val();

        $('#ReturnNumberHeader').text(returnnumber);

        $('#PartHeader').text(partName);

        $('#JobHeader').text(jobnumber);

        var currentcount;
        var maxcount;
        var totalParts;


        //Update Part

        currentcount = 0;
        maxcount = parseInt(form.find('input[name="QuantityReturned"]').val());
        totalParts = currentcount + maxcount;

        //Hide Container
        $('#returnElements').css("display", "none");


        $('#inputQty').val(currentcount);
        $('#maxQty').val(" / " + totalParts);

        $('#assignPart').click(function () {

            if (currentcount + 1 <= totalParts) {
                currentcount += 1;

                form.find('input[name="CheckedInQTY"]').val(currentcount);
                form.find('input[name="AvailableQuantity"]').val(maxcount -= 1);

                $('#inputQty').val(currentcount);
                formData = form.serialize();
            }

        });

        $('#removePart').click(function () {
            if (currentcount > 0) {
                currentcount -= 1;

                form.find('input[name="CheckedInQTY"]').val(currentcount);
                form.find('input[name="AvailableQuantity"]').val(maxcount += 1);

                $('#inputQty').val(currentcount);
               // formData = form.serialize();
            }
        });

        // Set the job number to the header
        //$("#JobPartHeader").text("Part: " + partName);

        // Get the instance of the quantity modal and open it
        var QuantityModalInst = M.Modal.getInstance($("#quantityModal"));
        if (QuantityModalInst) {
            QuantityModalInst.open();
        } else {
            console.error("Quantity modal instance is undefined.");
        }
        console.log(formData);
    }





    $('#updatePartReturn').click(function () {
        if (formData) {

            //Hide Container
            $('#returnElements').css("display", "none");

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
                    //$('#divPartial').html(response);
                    // Reinitialize modals after updating the partial view
                    // Initialize all modal elements within the updated partial view

                    // Redirect to the Returns controller's Index action
                    window.location.href = '/Returns/Index';

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.error('Error submitting form:', textStatus, errorThrown);
                }
            });
        }
    });

    // Clean up event listeners when modal is closed
    //$("#quantityModal").on('close', function () {
    //    $('#assignPart').off('click.assign');
    //    $('#removePart').off('click.remove');
    //});

    function resetQuantityDialog() {
        $('#assignPart').off('click.assign');
        $('#removePart').off('click.remove');
    }

});


