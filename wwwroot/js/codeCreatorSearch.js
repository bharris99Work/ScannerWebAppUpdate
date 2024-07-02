document.addEventListener('DOMContentLoaded', function () {

    var itemnum = document.getElementById('itemNumSearch');
    var jobnum = document.getElementById('jobNumSearch');
    var description = document.getElementById('description');
    var submitbn = document.getElementById('submitbn');
    var errortext = document.getElementById('searchError');

    submitbn.addEventListener('click', function (event) {

        if (itemnum.value.length === 1) {
            errortext.textContent = "Must have more than one character when searching"
            event.preventDefault();
        }
        if (jobnum.value.length === 1) {
            errortext.textContent = "Must have more than one character when searching"
            event.preventDefault();
        }
        if (description.value.length === 1) {
            errortext.textContent = "Must have more than one character when searching"
            event.preventDefault();
        }
    });
});
