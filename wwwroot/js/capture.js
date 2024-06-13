// Use Html5QrcodeScanner
import { Html5QrcodeScanner } from "html5-qrcode";
import { Html5Qrcode } from "html5-qrcode";

(() => {

    let width = 320;    // We will scale the photo width to this
    let height = 0;     // This will be computed based on the input stream

    let streaming = false; //If streaming or not

   
    let startbutton = null; //used to capture image

    let resulttext = null;

    let scanbutton = null;
    let newstream = null;

    let selectedDeviceId;


    //Gets camera premision and sets initial states.
    function startup() {

       
        startbutton = document.getElementById('startbutton');
        scanbutton = document.getElementById('startScan');
        resulttext = document.getElementById('resultText')
     
    }



  
    function onScanSuccess(decodedText, decodedResult) {
        // handle the scanned code as you likee:

        document.getElementById('resultText').textContent = `${decodedText}`, decodedResult;
        console.log(`${decodedText}`, decodedResult);
    }

    function onScanFailure(error) {
        // handle scan failure, usually better to ignore and keep scanning.
        // for example:
       // console.warn(`Code scan error = ${error}`);
    }

    let html5QrcodeScanner = new Html5QrcodeScanner(
        "reader",
        { fps: 10, qrbox: { width: 350, height: 350 } },
  /* verbose= */ false);

    html5QrcodeScanner.render(onScanSuccess, onScanFailure);
    


    // Set up  event listener to run the startup process
    // once loading is complete.
    window.addEventListener("load", startup, false);
})();