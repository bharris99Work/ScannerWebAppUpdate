import * as SDCCore from "scandit-web-datacapture-core";
import * as SDCBarcode from "scandit-web-datacapture-barcode";

document.addEventListener('DOMContentLoaded', async function () {


    var continousbn = document.getElementById('continousBN');
    var isContinous = false;

    let scanoptions = document.getElementById('scanOptions');

    //camera attributes
    var cameraview = document.getElementById("reader");
    var camerascan = document.getElementById('cameraScan');
    let camera = SDCCore.Camera.default;
    let cameraSettings = SDCBarcode.BarcodeCapture.recommendedCameraSettings;
    await camera.applySettings(cameraSettings);

    var pausedoverlay = document.getElementById('scannerPause');
    var workingoverlay = document.getElementById('scannerScanning');

    var startscanningbn = document.getElementById('startScanning');
    var scanning = false;

    //Reader attributes
    let openreader = document.getElementById('readerScan');
    let readeroptions = document.getElementById('readerOptions');
    let cancelreader = document.getElementById('cancelReader');


    var resulttext = document.getElementById('resultText');


    var elems = document.querySelectorAll('.modal');
    const options = { onCloseEnd: stopRec };
    var instances = M.Modal.init(elems, options);


    //Grabs instance of scanner dialog
    var scanmodal = document.getElementById('ScanDialog');
    var scanmodalinst = M.Modal.getInstance(scanmodal);

    //Grab instance of SearchDialog
    var searchmodal = document.getElementById('SearchDialog');
    var searchmodalinst = M.Modal.getInstance(searchmodal)

    var submitbn = document.getElementById('SpecificSearch');

    //Result helpers
    //var responseText = document.getElementById('responseText');
    var responseText = document.getElementById('resultStatus');
    var resultheader = document.getElementById('resultHeader');
    var resultcard = document.getElementById('resultCard');
    var testbtn = document.getElementById('testBtn');

    var closeresults = document.getElementById('closeResults');
    var exitresults = document.getElementById('exitResults');

    var context;
    var view;
    var barcodeCapture

    var canScan = false;

    var scanType = document.getElementById('ScanType');
    scanType.value = 'NotContinous';

    var resultstatus = document.getElementById('resultStatus');


    // Store reference to the keydown event listener
    let keydownEventListener;

    try {
        ScanReady();
        await initializeScanner();
    } catch (error) {
        console.error("Error initializing scanner:", error);
    }


    testbtn.addEventListener('click', function () {
        
    });

    closeresults.addEventListener('click', function () {
        resultcard.style.display = 'none';
    });

    exitresults.addEventListener('click', function () {
        resultcard.style.display = 'none';
    });

    //Reader functions and logic
    openreader.addEventListener('click', function () {
        readeroptions.style.display = 'block';
        scanoptions.style.display = 'none';
        let lastKeyTime = Date.now(); // Initialize lastKeyTime

        var savedString = '';

        //Max time allowed between strokes
        var maxTime = 700;

        //Start reader
        //document.addEventListener('keydown', function (event) {
        //    const currentTime = Date.now();
        //    const timeDiff = currentTime - lastKeyTime;

        //    //Time difference is too large to be from scanner
        //    if (timeDiff > maxTime) {
        //        //reset string
        //        savedString = '';
        //        console.log('Time to far apart of characters: ' + event.key + "Time Diff: " + timeDiff)
        //    }
        //    else {
        //        //Add char to string
        //        if (event.key.length === 1) {
        //            savedString += event.key;
        //            console.log('Wrting: ' + event.key)
        //        }

        //        // Do something with the input string if it meets criteria
        //        if (savedString.length > 2) {
        //            console.log('Current Input String:', savedString);
        //            resulttext.value = savedString;




        //            if (event.key === 'Enter') {
        //            var correctlyAdded = scannedPartSearch();

        //            if (correctlyAdded) {
        //                //handle correct
        //                responseText.textContent = 'Reader Correctly Found: ' + savedString;
        //                console.log('Reader Correctly Found: ' + savedString);
        //            }
        //            else {
        //                //handle error
        //                responseText.textContent = 'Reader Failed To Find: ' + savedString;

        //                resultcard.style.display = 'block';
        //                resultstatus.textContent = "Failed To Find Part";
        //                var partgoogle = document.getElementById('partGoogle');
        //                var pnumber = resulttext.value;
        //                partgoogle.innerHTML = pnumber;
        //                var googleSearchURL = 'https://www.google.com/search?tbm=shop&q=' + encodeURIComponent(pnumber);
        //                partgoogle.href = googleSearchURL;
        //                console.log('Reader Failed To Find: ' + savedString);
        //            }

        //            }
        //        }
        //    }
        //    lastKeyTime = currentTime; // Update the last keypress time

        //});

        // Define the keydown event listener function
        keydownEventListener = function (event) {
            const currentTime = Date.now();
            const timeDiff = currentTime - lastKeyTime;

            // Time difference is too large to be from scanner
            if (timeDiff > maxTime) {
                // Reset string
                savedString = '';
                console.log('Time too far apart of characters: ' + event.key + " Time Diff: " + timeDiff);
            } else {
                // Add char to string
                if (event.key.length === 1) {
                    savedString += event.key;
                    console.log('Writing: ' + event.key);
                }

                // Do something with the input string if it meets criteria
                if (savedString.length > 2) {
                    console.log('Current Input String:', savedString);
                    resulttext.value = savedString;

                    if (event.key === 'Enter') {
                        var correctlyAdded = scannedPartSearch();

                        if (correctlyAdded) {
                            // Handle correct
                            responseText.textContent = 'Reader Correctly Found: ' + savedString;
                            console.log('Reader Correctly Found: ' + savedString);
                        } else {
                            // Handle error
                            responseText.textContent = 'Reader Failed To Find: ' + savedString;

                            resultcard.style.display = 'block';
                            resultstatus.textContent = "Failed To Find Part";
                            var partgoogle = document.getElementById('partGoogle');
                            var pnumber = resulttext.value;
                            partgoogle.innerHTML = pnumber;
                            var googleSearchURL = 'https://www.google.com/search?tbm=shop&q=' + encodeURIComponent(pnumber);
                            partgoogle.href = googleSearchURL;
                            console.log('Reader Failed To Find: ' + savedString);
                        }
                    }
                }
            }
            lastKeyTime = currentTime; // Update the last keypress time
        };

        // Add the keydown event listener
        document.onkeydown = keydownEventListener;
   

        //Close Keyboard

        //If time between keypresses is > maxtime, do not remove character from string

        //If time betwen is less, and more than 2 characters, add to string
    });

    cancelreader.addEventListener('click', function () {
        readeroptions.style.display = 'none';
        scanoptions.style.display = 'block';

        //Open keyboard cancel reader
        console.log('Removing ability to of scanner');
        //document.removeEventListener('keydown');
        document.onkeydown = null;
        console.log('Scanner no longer reading.');

    });


    startscanningbn.addEventListener('click', async function () {

        if (!scanning) {
            

            startScanner();
            startscanningbn.innerHTML = '...Scanning...';
        }
        else {
            
            pauseScanner();
            startscanningbn.innerHTML ='Start Scanner';
        }

    });


  

    async function pauseScanner() {
        scanning = false;
        startscanningbn.innerHTML = 'Start Scanner';

        await barcodeCapture.setEnabled(false);

        //pausedoverlay.style.display = 'block';
       // workingoverlay.style.display = 'none';


    }

    async function startScanner() {
        scanning = true;
        startscanningbn.innerHTML = '...Scanning...';
        await barcodeCapture.setEnabled(true);

       // pausedoverlay.style.display = 'none';
        //workingoverlay.style.display = 'block';


    }


    camerascan.addEventListener('click', async function () {
        try {
            
            await StartCamera();
        } catch (error) {
            console.error("Error initializing scanner:", error);
        }
    });


    async function stopRec() {
        //Reset Values

        resulttext.value = '';

        readeroptions.style.display = 'none';

        scanoptions.style.display = 'block';

        //Open keyboard cancel reader
        console.log('Removing ability to of scanner');

        // $(document).off('keydown');
        document.onkeydown = null;

        console.log('Scanner no longer reading.');


        console.log("Turning off camera...");
        await camera.switchToDesiredState(SDCCore.FrameSourceState.Standby);
        await barcodeCapture.setEnabled(false);
    }


    async function initializeScanner() {

        console.log("Configuring Scandit SDK...");
        await SDCCore.configure({
            libraryLocation: new URL("/lib/engine/", document.baseURI).toString(),
            licenseKey: "AuwUBWZPQ38aOwpSKznFhogWqDlQIdAV1g/WwP0F3cywBkLcAElvM1IdKQOMLYk5Enzh8xtJ+QslEQMl8m7CHJJ3tJdCMj+i8jVAVwASQmEFGIUo4xNzGyELEBGqdIh180WRwblCpRkWQa8SnkPJVXh62Bn0TQFDkFnvvEhb+obEDDIhnRkShiphiNlEbaYMr0zF70xMwQpAWuq14kWbliRu1erjF8CmHWfUrQdhhUgLeEGNYV4I++dkfaZNf5bi4nNAy6BwGImJExK9f2go4H4ggb4/atdSGnAQ2gpFNO7sS/9zCnhDygNEiNHdeSJvn0lOT7NA5U2uf09klk55H+t4qewhdcLqMlfIeV1qHsqGSnNo02Oi5vg7EZqRYeE2JAuyDVZ7VR2MevyOhSfax8cT9PHoTxL74hNUrUhlU0WvFvuTUEQdjKQcZq3IBWo/XVuTPFskGFfUe4LoKUkh+69/UBj1QhcRe1Dm20JPMADsd1haSyGhaulqPSRkOVc4NENcU51/G5FYZGNogDYtFmgM5WH6MRodhjYQLCQdgT08yQi+l+WP2rYEO+JYQA2Mcwee+2c7vjJso88qdzzLJLbqoPgZzaZQn6iybQ/t6d2vqK4OMNiBvxlJKZfMCCLpc38913Ifoz0PaF4EAr9Ry8yct+viSGiBJbJj3bhmiGysFZQqZvOpdF11I/6bXzXKk8OiV7tglf0nAvB45zla3plsDVHVyh20yjrrQr7IovGDVobVRtO6QbquQ/lILDUoJU6J98q5RnH2aQNoWlMmKY9rrhNHEImCUWDUQDor8CeegX435Y8c36UWTGWFvkOY39EE++76B6NxEAWobeU4zzk6DnAzrOan8jfsUUSUfGn/NfeTluZZclaFyns4bmyuYqNPyyhBnTyacF3VSHYEQeYbqCTYkTyLcHnofh4l6T+cF+Or3k/jh1KrfYP2CPvVmkQTocOVz8gltXuTyWHxgjSIqw+jrbLgGYsHwBUEYHAa7x+huVW8g5balODCwietKZQTkFv9+ZMTf7Vge58mDujWjutfWTLltGOytsWm0suUwXRrvRaZrS1twbQJZxilgyBIfGd1B3683rpSBUV/opsubp7aAvNEG3AmCvRB41HYXUKX/2w5Baf9+knMziRn28kIUtprmCC/zXuI2I/yyhnhDjl3EK9UHjBB/rHIbj3iURoVhe8sSjLFIO7XS5D0SEo=",
            moduleLoaders: [SDCBarcode.barcodeCaptureLoader()]
        });

        console.log("Creating data capture context...");
        context = await SDCCore.DataCaptureContext.create();

        console.log("Setting up data capture view...");
        view = await SDCCore.DataCaptureView.forContext(context);
        view.connectToElement(cameraview);
        view.showProgressBar();

        console.log("Setting up camera...");
        //const camera = SDCCore.Camera.default;
        //const cameraSettings = SDCBarcode.BarcodeCapture.recommendedCameraSettings;
        //await camera.applySettings(cameraSettings);
        await context.setFrameSource(camera);

        console.log("Setting up barcode capture settings...");
        const settings = new SDCBarcode.BarcodeCaptureSettings();
        settings.enableSymbologies([
            SDCBarcode.Symbology.Code128,
            SDCBarcode.Symbology.Code39,
            SDCBarcode.Symbology.QR,
            SDCBarcode.Symbology.EAN8,
            SDCBarcode.Symbology.UPCE,
            SDCBarcode.Symbology.EAN13UPCA
        ]);

        settings.locationSelection = new SDCCore.RadiusLocationSelection(
            new SDCCore.NumberWithUnit(5, SDCCore.MeasureUnit.Pixel)
        );
        settings.codeDuplicateFilter = 1000;

        console.log("Creating barcode capture...");
        barcodeCapture = await SDCBarcode.BarcodeCapture.forContext(context, settings);

        // Asynchronous helper function
        const handleScanResult = async (recognizedBarcodes) => {
            console.log(recognizedBarcodes[0]._data);  // Do something with the barcodes
            resulttext.value = "";
            resulttext.value = recognizedBarcodes[0]._data;

            //Pause camera
            console.log("Pausing Camera.....");
            await pauseScanner();

            console.log("Starting Search...");
                var correctlyAdded = scannedPartSearch();

                if (correctlyAdded) {
                   // responsetextcamera.textContent = 'Camera Correctly Found Part';
                    //pauseScanner();
                    scanmodalinst.close();
                }
                else {
                    resultcard.style.display = 'block';
                    resultstatus.textContent = "Failed To Find Part";
                    //responsetextcamera.textContent = 'Camera Did Not Find Part';
                    var partgoogle = document.getElementById('partGoogle');
                    var pnumber = resulttext.value;
                    partgoogle.innerHTML = pnumber;
                    var googleSearchURL = 'https://www.google.com/search?tbm=shop&q=' + encodeURIComponent(pnumber);
                    partgoogle.href = googleSearchURL;

                   // pauseScanner();
                }
        };

        // Listener to handle barcode scan results
        const listener = {
            didScan: (barcodeCapture, session) => {
                const recognizedBarcodes = session.newlyRecognizedBarcodes;
                handleScanResult(recognizedBarcodes);
            }
        };
        barcodeCapture.addListener(listener);

        console.log("Adding camera switch control...");
        view.addControl(new SDCCore.CameraSwitchControl());

        // console.log("Turning on the camera...");
        // await camera.switchToDesiredState(SDCCore.FrameSourceState.On);
        // await barcodeCapture.setEnabled(true);

        console.log("Hiding progress bar...");
        view.hideProgressBar();

        console.log("Adding barcode capture overlay...");
        const overlay = await SDCBarcode.BarcodeCaptureOverlay.withBarcodeCaptureForView(barcodeCapture, view);
        overlay.setViewfinder(new SDCCore.AimerViewfinder());
        await overlay.setShouldShowScanAreaGuides(true);

        // Handle click events to move the viewfinder
        cameraview.addEventListener('click', function (event) {
            const rect = cameraview.getBoundingClientRect();
            const x = event.clientX - rect.left;
            const y = event.clientY - rect.top;

            // Update the viewfinder's position
            overlay.viewfinder.centerPoint = new SDCCore.PointWithUnit(
                new SDCCore.NumberWithUnit(x, SDCCore.MeasureUnit.Pixel),
                new SDCCore.NumberWithUnit(y, SDCCore.MeasureUnit.Pixel)
            );
        });

        console.log("Scanner ready")
        await barcodeCapture.setEnabled(false);
        canScan = true;
        ScanReady();
        return Promise.resolve();
    }

    async function StartCamera() {
        try {
            //await initializeScanner();

            console.log("Turning on the camera...");
            if (context && camera && barcodeCapture) {
                await camera.switchToDesiredState(SDCCore.FrameSourceState.On);
                //await barcodeCapture.setEnabled(false);
                resultstatus.value = "";
                scanmodalinst.open();


            }
            
        } catch (error) {
            console.error("Error initializing scanner:", error);
        }
    }

    continousbn.addEventListener('click', function () {

        if (!isContinous) {
            isContinous = true;
            continousbn.innerHTML = "True";
            scanType.value = 'Continous';
            console.log('Scan Type (Should Be Continous): ' + scanType.value)
        }
        else {
            isContinous = false;
            continousbn.innerHTML = "false";
            scanType.value = 'NotContinous';
            console.log('Scan Type (Should Be NotContinous): ' + scanType.value)
        }
    })

    function ScanReady() {
       
        if (!canScan) {
            console.log("Changing scan to no");
            camerascan.disabled = true;
        }
        else {
            console.log("Changing scan to yes");
            camerascan.disabled = false;
        }
        
    }

});
