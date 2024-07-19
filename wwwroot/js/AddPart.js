document.addEventListener('DOMContentLoaded', function () {
    var addpart = document.getElementById('addPartsub');
    var addpartdialog = document.getElementById('AddPartDialog');
    var addpartdialoginst = M.Modal.getInstance(addpartdialog);
    var submitqty = document.getElementById('submitqty');

    var assignedqty = document.getElementById('assignedQuantity');

    var qtyinput = document.getElementById('quantityinp');


    var submit = document.getElementById('submit');








    addpart.addEventListener('click', function (event) {
        event.preventDefault();
        console.log("Opened Add part");
        addpartdialoginst.open();

    });

    submitqty.addEventListener('click', function () {
        var newqty = qtyinput.value;
        assignedqty.value = newqty;

        console.log("Submit btn click");


       // submit.click();

    });












});