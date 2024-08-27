// Import everything
// And/or import only needed items (examples)
//import { DataCaptureContext, Camera } from 'scandit-web-datacapture-barcode';
//import { BarcodeCapture } from 'scandit-web-datacapture-barcode';
import * as SDCCore from "scandit-web-datacapture-core";
import * as SDCBarcode from "scandit-web-datacapture-barcode";


document.addEventListener('DOMContentLoaded', async function () {

    var resulttext = document.getElementById('resultText');

    var checkBody = document.getElementById('checkinBody');


    //Reader attributes
    let openreader = document.getElementById('readerScan');
    let readeroptions = document.getElementById('readerOptions');
    let cancelreader = document.getElementById('cancelReader');

    // Store reference to the keydown event listener
    let keydownEventListener;

    //Error Response Dialog
    var responseDialog = document.getElementById('responseDialog');
    var responseDialogInit = M.Modal.init(responseDialog);
    var responseDialogInst = M.Modal.getInstance(responseDialog);


    ////Init of Camera Scanner\\\\
    console.log("Configuring Scandit SDK...");
    await SDCCore.configure({
        licenseKey: "AuwUBWZPQ38aOwpSKznFhogWqDlQIdAV1g/WwP0F3cywBkLcAElvM1IdKQOMLYk5Enzh8xtJ+QslEQMl8m7CHJJ3tJdCMj+i8jVAVwASQmEFGIUo4xNzGyELEBGqdIh180WRwblCpRkWQa8SnkPJVXh62Bn0TQFDkFnvvEhb+obEDDIhnRkShiphiNlEbaYMr0zF70xMwQpAWuq14kWbliRu1erjF8CmHWfUrQdhhUgLeEGNYV4I++dkfaZNf5bi4nNAy6BwGImJExK9f2go4H4ggb4/atdSGnAQ2gpFNO7sS/9zCnhDygNEiNHdeSJvn0lOT7NA5U2uf09klk55H+t4qewhdcLqMlfIeV1qHsqGSnNo02Oi5vg7EZqRYeE2JAuyDVZ7VR2MevyOhSfax8cT9PHoTxL74hNUrUhlU0WvFvuTUEQdjKQcZq3IBWo/XVuTPFskGFfUe4LoKUkh+69/UBj1QhcRe1Dm20JPMADsd1haSyGhaulqPSRkOVc4NENcU51/G5FYZGNogDYtFmgM5WH6MRodhjYQLCQdgT08yQi+l+WP2rYEO+JYQA2Mcwee+2c7vjJso88qdzzLJLbqoPgZzaZQn6iybQ/t6d2vqK4OMNiBvxlJKZfMCCLpc38913Ifoz0PaF4EAr9Ry8yct+viSGiBJbJj3bhmiGysFZQqZvOpdF11I/6bXzXKk8OiV7tglf0nAvB45zla3plsDVHVyh20yjrrQr7IovGDVobVRtO6QbquQ/lILDUoJU6J98q5RnH2aQNoWlMmKY9rrhNHEImCUWDUQDor8CeegX435Y8c36UWTGWFvkOY39EE++76B6NxEAWobeU4zzk6DnAzrOan8jfsUUSUfGn/NfeTluZZclaFyns4bmyuYqNPyyhBnTyacF3VSHYEQeYbqCTYkTyLcHnofh4l6T+cF+Or3k/jh1KrfYP2CPvVmkQTocOVz8gltXuTyWHxgjSIqw+jrbLgGYsHwBUEYHAa7x+huVW8g5balODCwietKZQTkFv9+ZMTf7Vge58mDujWjutfWTLltGOytsWm0suUwXRrvRaZrS1twbQJZxilgyBIfGd1B3683rpSBUV/opsubp7aAvNEG3AmCvRB41HYXUKX/2w5Baf9+knMziRn28kIUtprmCC/zXuI2I/yyhnhDjl3EK9UHjBB/rHIbj3iURoVhe8sSjLFIO7XS5D0SEo=",
        libraryLocation: new URL("/lib/engine/", document.baseURI).toString(),
        moduleLoaders: [SDCBarcode.barcodeCaptureLoader()]
    });

    const dataCaptureContext = await SDCCore.DataCaptureContext.create();

    const sparkScanSettings = new SDCBarcode.SparkScanSettings();

    sparkScanSettings.enableSymbologies([
        SDCBarcode.Symbology.Code128,
        SDCBarcode.Symbology.Code39,
        SDCBarcode.Symbology.QR,
        SDCBarcode.Symbology.EAN8,
        SDCBarcode.Symbology.UPCE,
        SDCBarcode.Symbology.EAN13UPCA
    ]);

    const sparkScan = SDCBarcode.SparkScan.forSettings(sparkScanSettings);

    const sparkScanViewSettings = new SDCBarcode.SparkScanViewSettings();


    // Register a listener object to monitor the spark scan session.
    const listener = {
        didScan: (sparkScan, session, getFrameData) => {
            cancelreader.click();


            // Gather the recognized barcode
            const barcode = session.newlyRecognizedBarcodes[0];

            // Handle the barcode
            console.log('Scanned Barcode: ' + barcode._data);

            //Start Part Search
            resulttext.value = "";
            resulttext.value = barcode._data;

            console.log("Starting Search...");
            var correctlyAdded = scannedPartSearch();


            if (correctlyAdded) {
                //Do Something to prevent extra scans?
                cancelreader.click();

                sparkScanView.pauseScanning();
                //sparkScanView.prepareScanning();

            }
            else {

                //Part Not Found
                $('#responseHeader').text('Part Not On Job');

                $('#responsePartHeader').text(resulttext.value);

                responseDialogInst.open();

                console.log('Reader Failed To Find: ' + resulttext.value);

            }

          
        },
    };

    sparkScan.addListener(listener);



    const sparkScanView = SDCBarcode.SparkScanView.forElement(
        checkBody,
        dataCaptureContext,
        sparkScan,
        sparkScanViewSettings
    );

    sparkScanView.scanningBehaviorButtonVisible = false;
    sparkScanView.targetModeButtonVisible = false;


    await sparkScanView.prepareScanning();
    ////End Init\\\\




    //Reader functions and logic
    openreader.addEventListener('click', async function () {
        readeroptions.style.display = 'block';

        //Disable Camera
        //$('#cameraScan').prop('disabled', true);

        //Hide reader button
        $('#readerScan').css('display', 'none');

        //Put Camera On Standby
        //await StopCamera();

        // scanoptions.style.display = 'none';
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
                            //responseText.textContent = 'Reader Correctly Found: ' + savedString;
                            console.log('Reader Correctly Found: ' + savedString);
                        } else {
                            // Handle error
                            //Part Not Found
                            $('#responseHeader').text('Part Not On Job');

                            $('#responsePartHeader').text(resulttext.value);

                            responseDialogInst.open();

                            console.log('Reader Failed To Find: ' + savedString);
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
        //scanoptions.style.display = 'block';
        $('#readerScan').css('display', 'block');

        //Disable Camera
        //$('#cameraScan').prop('disabled', false);

        //Open keyboard cancel reader
        console.log('Removing ability to of scanner');
        //document.removeEventListener('keydown');
        document.onkeydown = null;
        console.log('Scanner no longer reading.');


        //Close Reader Click
        //Hide Reader Options
        //Stop listeners
        //Enable camera
        //Show reader button

    });
});