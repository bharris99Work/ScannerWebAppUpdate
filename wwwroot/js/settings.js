document.addEventListener('DOMContentLoaded', function () {
    // Initialize Materialize components
    var elems = document.querySelectorAll('.modal');
    var instances = M.Modal.init(elems);

    // Add event listener to file input
    document.getElementById('file-input').addEventListener('change', function (event) {
        var filePath = event.target.value;
        // Display the full file path in the text input
        document.querySelector('.file-path').value = filePath;
    });
});