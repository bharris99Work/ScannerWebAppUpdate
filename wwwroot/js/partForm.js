document.addEventListener('DOMContentLoaded', function () {
    var elems = document.querySelectorAll('select');
    var instances = M.FormSelect.init(elems);

    var partnumber = document.getElementById('partNumber');
    var jobnumber = document.getElementById('jobNumber');
    var partquantity = document.getElementById('partQuantity');

    var subquantity = document.getElementById('subQuantity');
    var addquantity = document.getElementById('addQuantity');

    var cleartech = document.getElementById('clearTech');
    var clearreturn = document.getElementById('clearReturn');
    var othertechbn = document.getElementById('otherTech');
    var otherreturnbn = document.getElementById('otherReturn');

    var techselect = document.getElementById('techOptions');
    var returnselect = document.getElementById('returnOptions');

    var techothertext = document.getElementById('techOptionText');
    var returnothertext = document.getElementById('returnOptionText');


    returnselect.value = '0';
    techselect.value = '0';

    partnumber.setAttribute('readonly', true);
    jobnumber.setAttribute('readonly', true);


    //Subtract and add to Quantity buttons
    subquantity.addEventListener('click', (event) => {
        event.preventDefault();
        partquantity.value = Math.max(0, parseInt(partquantity.value) - 1);
    });
    addquantity.addEventListener('click', (event) => {
        event.preventDefault();
        partquantity.value = parseInt(partquantity.value) + 1;
    });


    //Display tech text box, hide tech select box
    othertechbn.addEventListener('click', (event) => {
        event.preventDefault();
        var techSelectWrapper = techselect.parentElement; // Select the parent wrapper

        if (techSelectWrapper.style.display === 'none') {
            techSelectWrapper.style.display = 'block';
            techothertext.style.display = 'none';
            techothertext.value = '';
        } else {
            techSelectWrapper.style.display = 'none';
            techothertext.style.display = 'block';
        }
    });


    //Display return text box, hide tech select box
    otherreturnbn.addEventListener('click', (event) => {
        event.preventDefault();
        var returnSelectWrapper = returnselect.parentElement; // Select the parent wrapper

        if (returnSelectWrapper.style.display === 'none') {
            returnSelectWrapper.style.display = 'block';
            returnothertext.style.display = 'none';
            returnothertext.value = '';
        } else {
            returnSelectWrapper.style.display = 'none';
            returnothertext.style.display = 'block';
        }
    });


    //Clears tech text box and tech select box
    cleartech.addEventListener('click', (event) => {
        event.preventDefault();

        var techSelectWrapper = techselect.parentElement; // Select the parent wrapper
        techSelectWrapper.style.display = 'block';

        var instance = M.FormSelect.getInstance(techselect);
        instance.input.value = 'Select Tech Option:'; // Update the displayed value

        techothertext.style.display = 'none';
        techothertext.value = '';

    });


    //Clears return text box and retirm select box
    clearreturn.addEventListener('click', (event) => {
        event.preventDefault();

        var returnSelectWrapper = returnselect.parentElement; // Select the parent wrapper
        returnSelectWrapper.style.display = 'block';

        var instance = M.FormSelect.getInstance(returnselect);
        instance.input.value = 'Select Return Option: '; // Update the displayed value

        returnothertext.style.display = 'none';
        returnothertext.value = '';
    });


    //Checks selected tech option of scanned part and either selects it or displays string.
    var instance = M.FormSelect.getInstance(techselect);
    var foundTech = false;
    //Find selected Tech option and set accordingly
    if (ScannedPartTechOption != "" && ScannedPartTechOption != null) {
        for (var i = 0; i < techselect.options.length; i++) {
            if (techselect.options[i].textContent.trim() === ScannedPartTechOption.trim()) {
                // Update Materialize instance to reflect the selected value
                instance.input.value = techselect[i].textContent;
                foundTech = true;
                console.log("Tech option Found");
                break;
            }
        }

        if (!foundTech) {
            //Hide Select Box
            var techSelectWrapper = techselect.parentElement; // Select the parent wrapper
            techSelectWrapper.style.display = 'none';

            //Display Text Box
            techothertext.style.display = 'block';

            //Set Text Box Value = selected part value
            techothertext.value = ScannedPartTechOption;
        }
    }
    else {
        console.log("No string nor selected tech option");
        techselect.value = '0'; // Set to the placeholder option value
        instance.input.value = 'Select Tech Option: '; // Update the displayed value
    }

    //Checks selected return option of scanned part and either selects it or displays string.
    var foundReturn = false;
    // Update Materialize instance to reflect the selected value
    var instance = M.FormSelect.getInstance(returnselect);
    //Find selected Return Option and set Accordingly
    if (ScannedPartReturnOption != "" && ScannedPartReturnOption != null) {
        for (var i = 0; i < returnselect.options.length; i++) {
            if (returnselect.options[i].textContent.trim() === ScannedPartReturnOption.trim()) {
                instance.input.value = returnselect[i].textContent;
                foundReturn = true;
                console.log("Return option Found");
                break;
            }
        }
        if (!foundReturn) {
            //Hide Select Box
            var returnSelectWrapper = returnselect.parentElement; // Select the parent wrapper
            returnSelectWrapper.style.display = 'none';

            //Display Text Box
            returnothertext.style.display = 'block';

            //Set Text Box Value = selected part value
            returnothertext.value = ScannedPartReturnOption;
          
        }
    }
    else {
        console.log("No string nor selected return option");
        returnselect.value = '0';
        instance.input.value = 'Select Return Option: '; // Update the displayed value
    }

});