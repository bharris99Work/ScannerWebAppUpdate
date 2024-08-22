$(document).ready(function () {

    var formData;
    var form;
    var partnumber;
    var currentcount;
    var maxcount;

    var countdialog = document.getElementById('CountDialog');
    var countdialogopt = { onCloseEnd: ResetCountDialog };
    var countdialoginit = M.Modal.init(countdialog, countdialogopt);


    //Function if scanned prt
    $(document).on('submit', '.update-scanpart-form', function (event) {
        event.preventDefault();

        form = $(this);

        partName = form.find('input[name="PartName"]').val();
        formData = form.serialize();



        ShowCountDialog();
    });


    function ShowCountDialog() {

        var availableqty = parseInt(form.find('input[name="Ordered"]').val()) - parseInt(form.find('input[name="CheckedIn"]').val())


        //Set Part Number
        partnumber = form.find('input[name="PartName"]').val();

        $('#partNumberHeader').text(partnumber);

        //Set Initial and Max Quantity
        currentcount = 0;
        maxcount = availableqty;

        $('#checkingin').val(currentcount);
        $('#availableqty').val(maxcount);



        //Open
        var countdialoginst = M.Modal.getInstance($('#CountDialog'));
        if (countdialoginst) {
            countdialoginst.open();
        }
        else {
            console.error("Unable to open count dialog");
        }

        console.log(formData);
    }


    $('#addCheckIn').on('click', function () {
        if (currentcount + 1 <= maxcount) {
            currentcount++;
            $('#checkingin').val(currentcount);
        }
    });

    $('#subCheckIn').on('click', function () {
        if (currentcount - 1 >= 0) {
            currentcount--;
            $('#checkingin').val(currentcount);
        }
    });



    $('#updateCount').on('click', function () {

        var checkedin = $('#checkingin').val();
        form.find('input[name="toCheckIn"]').val(checkedin);

        formData = form.serialize();

        var countdialoginst = M.Modal.getInstance(countdialog);

        if (countdialoginst) {
            countdialoginst.close();
        }


        // Optionally send the form data via AJAX
        $.ajax({
            type: 'POST',
            url: form.attr('action'),
            data: formData,
            success: function (response) {
                console.log('Form submitted successfully:', response);
                // UploadResult();

                // Optionally update the partial view with the new data
                $('#divPartial').html(response);
                // Reinitialize modals after updating the partial view
                // Initialize all modal elements within the updated partial view

                // Redirect to the Returns controller's Index action
               // window.location.href = '/Returns/Index';

            },
            error: function (jqXHR, textStatus, errorThrown) {
                // UploadResult();

                console.error('Error submitting form:', textStatus, errorThrown);
            }
        });

    });




    function SubmitForm() {

    }

    function ResetCountDialog() {

    }





});