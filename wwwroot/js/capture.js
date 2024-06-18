// Use Html5QrcodeScanner
import { Html5Qrcode } from "html5-qrcode";


document.addEventListener('DOMContentLoaded', function () {
    var selectedSide = "back";
   
    var cameraselect = document.getElementById('cameraSelect');

    // This method will trigger user permissions
    Html5Qrcode.getCameras().then(devices => {
        /**
         * devices would be an array of objects of type:
         * { id: "id", label: "label" }
         */
        // Clear the select element first
        cameraselect.innerHTML = '';

        // Iterate over the devices array
        devices.forEach(device => {
            // Create an option element
            var option = document.createElement('option');
            option.value = device.id;
            option.text = device.label;

            // Append the option to the select element
            cameraselect.appendChild(option);
        });
        
       
        
    }).catch(err => {
        // handle err
    });



    const html5QrCode = new Html5Qrcode("reader");
    const qrCodeSuccessCallback = (decodedText, decodedResult) => {
        /* handle success */
    };
    const config = { fps: 10, qrbox: { width: 250, height: 250 } };

    document.getElementById('switchCamera').addEventListener('click', (event) => {
        if (selectedSide === "back") {
            // Select front camera or fail with `OverconstrainedError`.
            html5QrCode.start({ facingMode: { exact: "user" } }, config, qrCodeSuccessCallback);
            selectedSide = "front";
        }
        else {
            // Select back camera or fail with `OverconstrainedError`.
            html5QrCode.start({ facingMode: { exact: "environment" } }, config, qrCodeSuccessCallback);
            selectedSide = "back";
        }

    });

      
    

 
    
});