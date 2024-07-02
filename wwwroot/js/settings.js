document.addEventListener('DOMContentLoaded', function () {
    // Initialize Materialize components
    var elems = document.querySelectorAll('.modal');
    var instances = M.Modal.init(elems);

    var submit = document.getElementById('submitBN');

    var filetext = document.getElementById('file-input');

    var progressbar = document.getElementById('progressBar');

    progressbar.style.display = 'none';

    submit.addEventListener('click', function (event) {

        if (filetext.value == '') {
            event.preventDefault();
        }
        else {
            progressbar.style.display = 'block';
        }

    })

    // Add event listener to file input
    document.getElementById('file-input').addEventListener('change', function (event) {
        var filePath = event.target.value;
        // Display the full file path in the text input
        document.querySelector('.file-path').value = filePath;
    });
});