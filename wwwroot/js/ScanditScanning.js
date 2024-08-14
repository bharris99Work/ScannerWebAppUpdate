import * as SDCCore from "scandit-web-datacapture-core";
import * as SDCBarcode from "scandit-web-datacapture-barcode";

document.addEventListener('DOMContentLoaded', async function () {
    let scanoptions = document.getElementById('scanOptions');

    //camera attributes
    var cameraview = document.getElementById("reader");
    var camerascan = document.getElementById('cameraScan');
   

    var startscanningbn = document.getElementById('startScanning');
    var scanning = false;

    //Reader attributes
    let openreader = document.getElementById('readerScan');
    let readeroptions = document.getElementById('readerOptions');
    let cancelreader = document.getElementById('cancelReader');


    var resulttext = document.getElementById('resultText');

    
    //Modal init
    var scanmodal = document.getElementById('ScanDialog');

    var searchmodal = document.getElementById('SearchDialog');

    var scansearchopt = { onCloseEnd: stopRec };

    var scandialoginit = M.Modal.init(scanmodal, scansearchopt);
    var searchdialoginit = M.Modal.init(searchmodal, scansearchopt);


    //Grabs instance of scanner dialog
    var scanmodalinst = M.Modal.getInstance(scanmodal);

    //Grab instance of SearchDialog
    var searchmodalinst = M.Modal.getInstance(searchmodal)


    // Initialize the first modal with its own onCloseEnd callback
    $('#ScanDialog').modal({
        onCloseEnd: stopRec()
    });

    $('#SearchDialog').modal({
        onCloseEnd: stopRec()
    });

    //Result helpers
    var responseText = document.getElementById('resultStatus');
    var resultcard = document.getElementById('resultContainer');
    //var testbtn = document.getElementById('testBtn');

    var closeresults = document.getElementById('closeResults');
    var exitresults = document.getElementById('exitResults');

    var resultstatus = document.getElementById('resultStatus');


    var canScan = false;
 

    // Store reference to the keydown event listener
    let keydownEventListener;




    camerascan.disabled = true;
    console.log("Configuring Scandit SDK...");
    await SDCCore.configure({
        libraryLocation: new URL("/lib/engine/", document.baseURI).toString(),
        licenseKey: "AuwUBWZPQ38aOwpSKznFhogWqDlQIdAV1g/WwP0F3cywBkLcAElvM1IdKQOMLYk5Enzh8xtJ+QslEQMl8m7CHJJ3tJdCMj+i8jVAVwASQmEFGIUo4xNzGyELEBGqdIh180WRwblCpRkWQa8SnkPJVXh62Bn0TQFDkFnvvEhb+obEDDIhnRkShiphiNlEbaYMr0zF70xMwQpAWuq14kWbliRu1erjF8CmHWfUrQdhhUgLeEGNYV4I++dkfaZNf5bi4nNAy6BwGImJExK9f2go4H4ggb4/atdSGnAQ2gpFNO7sS/9zCnhDygNEiNHdeSJvn0lOT7NA5U2uf09klk55H+t4qewhdcLqMlfIeV1qHsqGSnNo02Oi5vg7EZqRYeE2JAuyDVZ7VR2MevyOhSfax8cT9PHoTxL74hNUrUhlU0WvFvuTUEQdjKQcZq3IBWo/XVuTPFskGFfUe4LoKUkh+69/UBj1QhcRe1Dm20JPMADsd1haSyGhaulqPSRkOVc4NENcU51/G5FYZGNogDYtFmgM5WH6MRodhjYQLCQdgT08yQi+l+WP2rYEO+JYQA2Mcwee+2c7vjJso88qdzzLJLbqoPgZzaZQn6iybQ/t6d2vqK4OMNiBvxlJKZfMCCLpc38913Ifoz0PaF4EAr9Ry8yct+viSGiBJbJj3bhmiGysFZQqZvOpdF11I/6bXzXKk8OiV7tglf0nAvB45zla3plsDVHVyh20yjrrQr7IovGDVobVRtO6QbquQ/lILDUoJU6J98q5RnH2aQNoWlMmKY9rrhNHEImCUWDUQDor8CeegX435Y8c36UWTGWFvkOY39EE++76B6NxEAWobeU4zzk6DnAzrOan8jfsUUSUfGn/NfeTluZZclaFyns4bmyuYqNPyyhBnTyacF3VSHYEQeYbqCTYkTyLcHnofh4l6T+cF+Or3k/jh1KrfYP2CPvVmkQTocOVz8gltXuTyWHxgjSIqw+jrbLgGYsHwBUEYHAa7x+huVW8g5balODCwietKZQTkFv9+ZMTf7Vge58mDujWjutfWTLltGOytsWm0suUwXRrvRaZrS1twbQJZxilgyBIfGd1B3683rpSBUV/opsubp7aAvNEG3AmCvRB41HYXUKX/2w5Baf9+knMziRn28kIUtprmCC/zXuI2I/yyhnhDjl3EK9UHjBB/rHIbj3iURoVhe8sSjLFIO7XS5D0SEo=",
        moduleLoaders: [SDCBarcode.barcodeCaptureLoader()]
    });

    console.log("Creating data capture context...");
    const context = await SDCCore.DataCaptureContext.create();

    console.log("Setting up data capture view...");
    const view = await SDCCore.DataCaptureView.forContext(context);
    view.connectToElement(cameraview);
    view.showProgressBar();

    console.log("Setting up camera...");
    const camera = SDCCore.Camera.default;
    const cameraSettings = SDCBarcode.BarcodeCapture.recommendedCameraSettings;
    await camera.applySettings(cameraSettings);
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
    //barcodeCapture = await SDCBarcode.BarcodeCapture.forContext(context, settings);

    const barcodeCapture = await SDCBarcode.BarcodeCapture.forContext(context, settings);

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
            scanmodalinst.close();
        }
        else {
            resultcard.style.display = 'block';
            resultstatus.textContent = "Failed To Find Part";
            var partgoogle = document.getElementById('partGoogle');
            var pnumber = resulttext.value;
            partgoogle.innerHTML = "Google: " + pnumber;
            var googleSearchURL = 'https://www.google.com/search?tbm=shop&q=' + encodeURIComponent(pnumber);
            partgoogle.href = googleSearchURL;
        }
    };

    // Listener to handle barcode scan results
    const listener = {
        didScan: (barcodeCapture, session) => {
            const recognizedBarcodes = session.newlyRecognizedBarcodes;
            handleScanResult(recognizedBarcodes);
        }
    };

    console.log("Adding listener to brcode capture")
    barcodeCapture.addListener(listener);





    console.log("Adding camera switch control...");
    view.addControl(new SDCCore.CameraSwitchControl());

    console.log("Hiding progress bar...");
    view.hideProgressBar();

    console.log("Adding barcode capture overlay...");
    const overlay = await SDCBarcode.BarcodeCaptureOverlay.withBarcodeCaptureForView(barcodeCapture, view);
    overlay.setViewfinder(new SDCCore.AimerViewfinder());


 

    console.log("Scanner ready")
    await barcodeCapture.setEnabled(false);
    canScan = true;
    ScanReady();























































    //Close Result Pop-up Div
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


  
    //Pause or Starts barcode scanning listener
    async function pauseScanner() {
        scanning = false;
        startscanningbn.innerHTML = 'Start Scanner';

        await barcodeCapture.setEnabled(false);
    }

    async function startScanner() {
        scanning = true;
        startscanningbn.innerHTML = '...Scanning...';

        await barcodeCapture.setEnabled(true);

    }

    //When Camera button is clicked
    camerascan.addEventListener('click', async function () {
        try {
            //Start Camera scanning, load camera
            await StartCamera();
        } catch (error) {
            console.error("Error initializing scanner:", error);
        }
    });


    //When Dialogs are closed - Clear reset vals
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

       // resultcard.style.display = 'none';

        console.log("Turning off camera...");
        await camera.switchToDesiredState(SDCCore.FrameSourceState.Standby);
        await pauseScanner();
    }

    //Start Camera Start Up
    async function initializeScanner() {

        return Promise.resolve();
    }

    async function StartCamera() {
        try {
            console.log("Turning on the camera...");
            if (context && camera && barcodeCapture) {

                //Turns On Camera
                await camera.switchToDesiredState(SDCCore.FrameSourceState.On);
                resultstatus.value = "";
                scanmodalinst.open();
            }
            
        } catch (error) {
            console.error("Error initializing scanner:", error);
        }
    }

   //Checks If scanner is ready to be used
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
