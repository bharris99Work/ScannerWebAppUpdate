﻿// Use Html5QrcodeScanner
import { Html5Qrcode } from "html5-qrcode";

document.addEventListener('DOMContentLoaded', function () {


    //Displays Camera and Reader Buttons
    var scanoptions = document.getElementById('scanOptions');
    //Text Area where result from scan will be displayed
    var resultext = document.getElementById('resultText');

    //Button to start scan with camera
    var camerascan = document.getElementById('cameraScan');
    //Switch Camera Option
    var switchcamera = document.getElementById('switchCamera');
    //Keeps track of if a camera has been found
    var camerafound = false;

    //Button to start scan with reader
    var openreader = document.getElementById('readerScan');
    //Reader Options
    var readeroptions = document.getElementById('readerOptions');
    //Stop Reader
    var cancelreader = document.getElementById('cancelReader');

    

    

  

    //init Modal
    var elems = document.querySelectorAll('.modal');
    const options = { onCloseEnd: stopCamera };
    var instances = M.Modal.init(elems, options);

    //Grabs instance of scanner dialog
    var scanmodal = document.getElementById('ScanDialog');
    var scanmodalinst = M.Modal.getInstance(scanmodal);

    //Hides switch camera button, unhides if multiple devices are found
    switchcamera.style.display = 'none';

    var scanning = false;
    var availabledevices;
    var cameraid;
    var camerafound = false;
    var back = true;


    //Starts Reader
    openreader.addEventListener('click', function () {
        readeroptions.style.display = 'block';
        scanoptions.style.display = 'none';
        let lastKeyTime = Date.now(); // Initialize lastKeyTime

        var savedString = '';

        //Max time allowed between strokes
        var maxTime = 700;

        //Start reader
        document.addEventListener('keydown', function (event) {
            const currentTime = Date.now();
            const timeDiff = currentTime - lastKeyTime;

            //Time difference is too large to be from scanner
            if (timeDiff > maxTime) {
                //reset string
                savedString = '';
                console.log('Time to far apart of characters: ' + event.key + "Time Diff: " + timeDiff)
            }
            else {
                //Add char to string
                if (event.key.length === 1) {
                    savedString += event.key;
                    console.log('Wrting: ' + event.key)
                }

                // Do something with the input string if it meets criteria
                if (savedString.length > 2) {
                    console.log('Current Input String:', savedString);
                    resultext.value = savedString;
                    if (event.key === 'Enter') {
                        //submit.click();
                    }
                }
            }
            lastKeyTime = currentTime; // Update the last keypress time

        });

        //Close Keyboard

        //If time between keypresses is > maxtime, do not remove character from string

        //If time betwen is less, and more than 2 characters, add to string
    });


    //Stops Reader and opens scan options again
    cancelreader.addEventListener('click', function () {
        readeroptions.style.display = 'none';
        scanoptions.style.display = 'block';

        //Open keyboard cancel reader

    });



    //Dynamically set size of QR Code box
    // Square QR box with edge size = 70% of the smaller edge of the viewfinder.
    let qrboxFunction = function (viewfinderWidth, viewfinderHeight) {
        let minEdgePercentage = 0.7; // 70%
        let minEdgeSize = Math.min(viewfinderWidth, viewfinderHeight);
        let qrboxSize = Math.floor(minEdgeSize * minEdgePercentage);
        return {
            width: qrboxSize,
            height: qrboxSize
        };
    }
    //Config for scanner reader
    const config = { fps: 10, showTorchButtonIfSupported: true, qrbox: qrboxFunction };

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
        camerafound = true;


    }).catch(err => {
        // handle err
        console.log("No Camera Found");
        camerafound = false;
        // scanmodalinst.close();
    });

    //Starts scan using camera
    camerascan.addEventListener('click', function () {

        if (camerafound) {
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
        }
        else {
            console.log("No Camera Found");
        }

    });

    //Returned if succesful scan of number
    const qrCodeSuccessCallback = (decodedText, decodedResult) => {
        /* handle success */

        //If Value is not null or empty
        if (decodedText != "" && decodedText != null) {
            resultext.value = decodedText;

            //Hide dialog
            scanmodalinst.close();

            //submit.click();

        }
    };

    //init reader
    const html5QrCode = new Html5Qrcode("reader");

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

    //Stops camera scan
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