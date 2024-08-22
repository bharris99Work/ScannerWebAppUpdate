$(document).ready(function () {

    var printquantitydialog = document.getElementById('PrintQuantityDialog');
    
    var printqtydialoginit = M.Modal.init(printquantitydialog);

    var printqtydialoginst = M.Modal.getInstance(printquantitydialog);

    var returnForm;
    var form;
    var formData;

    //Function if Use part is selected
    $(document).on('submit', '.return-print-form', function (event) {

        event.preventDefault();

        returnForm = this;

        // Store the form reference
        form = $(this);
        formData = form.serialize();

        // Open the dialog
        printqtydialoginst.open();

        
    
    });

    // Attach click listener to the 'printLabels' button
    // This listener should be attached outside the form submission to avoid multiple bindings
    $('#printLabels').one('click', function () {

        form.find('input[name="PrintQuantity"]').val($('#printQuantity').val());


        //Submit the form
        returnForm.submit();
    });


    //Grab form attatched
    //Load dialog that changes attatched quantity
    //Update the attatched quantity associated with that form
   
});