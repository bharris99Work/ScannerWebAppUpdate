import * as SDCCore from "scandit-web-datacapture-core";
import * as SDCBarcode from "scandit-web-datacapture-barcode";

document.addEventListener('DOMContentLoaded', async function () {
    var cameraview = document.getElementById("cameraView");
    var resulttext = document.getElementById('resultText');

    async function initializeScanner() {
        console.log("Configuring Scandit SDK...");
        await SDCCore.configure({
            licenseKey: "AuwUBWZPQ38aOwpSKznFhogWqDlQIdAV1g/WwP0F3cywBkLcAElvM1IdKQOMLYk5Enzh8xtJ+QslEQMl8m7CHJJ3tJdCMj+i8jVAVwASQmEFGIUo4xNzGyELEBGqdIh180WRwblCpRkWQa8SnkPJVXh62Bn0TQFDkFnvvEhb+obEDDIhnRkShiphiNlEbaYMr0zF70xMwQpAWuq14kWbliRu1erjF8CmHWfUrQdhhUgLeEGNYV4I++dkfaZNf5bi4nNAy6BwGImJExK9f2go4H4ggb4/atdSGnAQ2gpFNO7sS/9zCnhDygNEiNHdeSJvn0lOT7NA5U2uf09klk55H+t4qewhdcLqMlfIeV1qHsqGSnNo02Oi5vg7EZqRYeE2JAuyDVZ7VR2MevyOhSfax8cT9PHoTxL74hNUrUhlU0WvFvuTUEQdjKQcZq3IBWo/XVuTPFskGFfUe4LoKUkh+69/UBj1QhcRe1Dm20JPMADsd1haSyGhaulqPSRkOVc4NENcU51/G5FYZGNogDYtFmgM5WH6MRodhjYQLCQdgT08yQi+l+WP2rYEO+JYQA2Mcwee+2c7vjJso88qdzzLJLbqoPgZzaZQn6iybQ/t6d2vqK4OMNiBvxlJKZfMCCLpc38913Ifoz0PaF4EAr9Ry8yct+viSGiBJbJj3bhmiGysFZQqZvOpdF11I/6bXzXKk8OiV7tglf0nAvB45zla3plsDVHVyh20yjrrQr7IovGDVobVRtO6QbquQ/lILDUoJU6J98q5RnH2aQNoWlMmKY9rrhNHEImCUWDUQDor8CeegX435Y8c36UWTGWFvkOY39EE++76B6NxEAWobeU4zzk6DnAzrOan8jfsUUSUfGn/NfeTluZZclaFyns4bmyuYqNPyyhBnTyacF3VSHYEQeYbqCTYkTyLcHnofh4l6T+cF+Or3k/jh1KrfYP2CPvVmkQTocOVz8gltXuTyWHxgjSIqw+jrbLgGYsHwBUEYHAa7x+huVW8g5balODCwietKZQTkFv9+ZMTf7Vge58mDujWjutfWTLltGOytsWm0suUwXRrvRaZrS1twbQJZxilgyBIfGd1B3683rpSBUV/opsubp7aAvNEG3AmCvRB41HYXUKX/2w5Baf9+knMziRn28kIUtprmCC/zXuI2I/yyhnhDjl3EK9UHjBB/rHIbj3iURoVhe8sSjLFIO7XS5D0SEo=",
            libraryLocation: new URL("/lib/engine/", document.baseURI).toString(),
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
        settings.codeDuplicateFilter = 500;

        console.log("Creating barcode capture...");
        const barcodeCapture = await SDCBarcode.BarcodeCapture.forContext(context, settings);

        // Listener to handle barcode scan results
        const listener = {
            didScan: (barcodeCapture, session) => {
                const recognizedBarcodes = session.newlyRecognizedBarcodes;
                console.log(recognizedBarcodes[0]._data);  // Do something with the barcodes
                resulttext.value = "";
                resulttext.value = recognizedBarcodes[0]._data;
            }
        };
        barcodeCapture.addListener(listener);

        console.log("Adding camera switch control...");
        view.addControl(new SDCCore.CameraSwitchControl());

        console.log("Turning on the camera...");
        await camera.switchToDesiredState(SDCCore.FrameSourceState.On);
        await barcodeCapture.setEnabled(true);

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
    }

    try {
        await initializeScanner();
    } catch (error) {
        console.error("Error initializing scanner:", error);
    }
});
