// Use Html5QrcodeScanner
import { Html5Qrcode } from "html5-qrcode";


document.addEventListener('DOMContentLoaded', function () {
    var back = true;
   
    //var cameraselect = document.getElementById('cameraSelect');
    var switchcamera = document.getElementById('switchCamera');
    //var scanstart = document.getElementById('startScan');
   // var scanstop = document.getElementById('stopScan');
    var resultext = document.getElementById('resultText');

    var submit = document.getElementById('submitbn');

    var scanning = false;
    var availabledevices;
    var cameraid;

   

    //Hides switch camera button, unhides if multiple devices are found
    switchcamera.style.display = 'none';

    var elems = document.querySelectorAll('.modal');
    const options = { onCloseEnd: stopCamera };
    var instances = M.Modal.init(elems, options);


   


    // This method will trigger user permissions
    Html5Qrcode.getCameras().then(devices => {
        /**
         * devices would be an array of objects of type:
         * { id: "id", label: "label" }
         */
        // Clear the select element first
       // cameraselect.innerHTML = '';

        // Check if labels are present and distinguishable
        const frontCameraLabels = ["front", "selfie"];
        const backCameraLabels = ["back", "rear", "environment"];


        //List of cameras so user can select?
        //Sets select box = to list of devices
        // Iterate over the devices array
        //devices.forEach(device => {
            //// Create an option element
            //var option = document.createElement('option');
            //option.value = device.id;
            //option.text = device.label || `Camera ${device.id}`;

            //// Add additional labeling to help distinguish cameras
            //if (frontCameraLabels.some(label => device.label.toLowerCase().includes(label))) {
            //    option.text = `Front Camera (${device.label})`;
            //} else if (backCameraLabels.some(label => device.label.toLowerCase().includes(label))) {
            //    option.text = `Back Camera (${device.label})`;
            //}

            //// Append the option to the select element
            //cameraselect.appendChild(option);
        //});

        //Finds all camera devices
        availabledevices = devices;
        //Sets cameraid = to first avalible device found
        cameraid = availabledevices[0].id
        if (availabledevices.length > 1) {
            //Display switch button
            switchcamera.style.display = 'block';
            // starts camera with back camera or fail with `OverconstrainedError`.
            //availabledevices.forEach(device => {

            //});
            html5QrCode.start({ facingMode: { exact: "environment" } }, config, qrCodeSuccessCallback);

            back = true;
            scanning = true;
            scanmodalinst.open();
        }
        else {
            //Starts camera with default selected device
            html5QrCode.start(cameraid, config, qrCodeSuccessCallback);
            scanning = true;
            scanmodalinst.open();
        }

    }).catch(err => {
        // handle err
        console.log("No Camera Found");
        scanmodalinst.close();
    });


    //Grabs instance of scanner dialog
    var scanmodal = document.getElementById('ScanDialog');
    var scanmodalinst = M.Modal.getInstance(scanmodal);


    const html5QrCode = new Html5Qrcode("reader");

    //Returned if succesful scan of number
    const qrCodeSuccessCallback = (decodedText, decodedResult) => {
        /* handle success */

        //If Value is not null or empty
        if (decodedText != "" && decodedText != null) {
            resultext.value = decodedText;

            //Hide dialog
            scanmodalinst.close();

            submit.click();
            
        }
    };

    //Config for scanner reader
    const config = { fps: 10, showTorchButtonIfSupported: true, qrbox: { width: 200, height: 200 } };

    //Switches between front and back camera TODO: NEED TO HANDLE LOGIC IF USER HAS TWO CAMERAS ON A DESKTOP
    switchcamera.addEventListener('click', function () {
        //If back camera is selected
        if (back) {
            //If camera is scanning
            if (scanning) {
                //Stop scanner
                html5QrCode.stop().then((ignore) => {
                    // QR Code scanning is stopped.
                    scanning = false;
                    //start again with front camera 
                    html5QrCode.start({ facingMode: { exact: "user" } }, config, qrCodeSuccessCallback);
                    back = false;
                    scanning = true;
                }).catch((err) => {
                    // Stop failed, handle it.
                });
            }
            else {
                // Select front camera or fail with `OverconstrainedError`.
                html5QrCode.start({ facingMode: { exact: "user" } }, config, qrCodeSuccessCallback);
                back = false;
                scanning = true;
            }
            //cameraid = 'facingMode: { exact: "user" }';
        }
        else {
            if (scanning) {
                html5QrCode.stop().then((ignore) => {
                    // QR Code scanning is stopped.
                    scanning = false;
                    //Started again
                    html5QrCode.start({ facingMode: { exact: "environment" } }, config, qrCodeSuccessCallback);
                    back = true;
                    scanning = true;
                }).catch((err) => {
                    // Stop failed, handle it.
                });
            }
            else {
                // Select back camera or fail with `OverconstrainedError`.
                html5QrCode.start({ facingMode: { exact: "environment" } }, config, qrCodeSuccessCallback);
                back = true;
                scanning = true;
            }
            //cameraid = 'facingMode: { exact: "environment" }';
        }
    });

    //Button to start scan?
    //scanstart.addEventListener('click', (event) => {
    //    if (scanning) {
    //        html5QrCode.stop().then((ignore) => {
    //            // QR Code scanning is stopped.
    //            // Select back camera or fail with `OverconstrainedError`.
    //            html5QrCode.start(cameraid, config, qrCodeSuccessCallback);
    //        }).catch((err) => {
    //            // Stop failed, handle it.
    //        });

    //        scanning = true;

    //    }
    //    else {
    //        // Select back camera or fail with `OverconstrainedError`.
    //        html5QrCode.start(cameraid, config, qrCodeSuccessCallback);
    //        scanning = true;
    //    }
       
    //});

    scanstop.addEventListener('click', function () {
        stopCamera();
    });


    function stopCamera() {

        if (scanning) {
            html5QrCode.stop().then((ignore) => {
                // QR Code scanning is stopped.
            }).catch((err) => {
                // Stop failed, handle it.
            });
            scanning = false;
        }
        else
            console.log("Not Scanning");
    }
});