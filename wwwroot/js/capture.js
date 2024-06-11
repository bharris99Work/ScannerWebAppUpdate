// Use Html5QrcodeScanner
import { Html5QrcodeScanner } from "html5-qrcode";

(() => {

    let width = 320;    // We will scale the photo width to this
    let height = 0;     // This will be computed based on the input stream

    let streaming = false; //If streaming or not

    let video = null;
    let canvas = null;
    let photo = null;
    let startbutton = null; //used to capture image

    let resulttext = null;

    let scanbutton = null;
    let newstream = null;

    let selectedDeviceId;


    //Gets camera premision and sets initial states.
    function startup() {

        video = document.getElementById('video');
        canvas = document.getElementById('canvas');
        photo = document.getElementById('photo');
        startbutton = document.getElementById('startbutton');
        scanbutton = document.getElementById('startScan');
        resulttext = document.getElementById('resultText')

        /*
        //Gets access to camera permissions, returns true or false; No audio
        navigator.mediaDevices
            .getUserMedia({ video: true, audio: false })
            .then((stream) => {
                newstream = stream;
                //video.srcObject = stream;
                //video.play();
                //sets stream an plays if access granted
            })
            .catch((err) => {
                console.error(`An error occurred: ${err}`);
            });*?


/*
        //Checks if video can play yet; because of delay
        video.addEventListener(
            "canplay",
            (ev) => {
                if (!streaming) {
                    height = (video.videoHeight / video.videoWidth) * width;

                    // Firefox currently has a bug where the height can't be read from
                    // the video, so we will make assumptions if this happens.

                    if (isNaN(height)) {
                        height = width / (4 / 3);
                    }

                    video.setAttribute("width", width);
                    video.setAttribute("height", height);
                    canvas.setAttribute("width", width);
                    canvas.setAttribute("height", height);
                    streaming = true;
                }
            },
            false,
        );*/

                /*
                //Snaps picture, prevent default stops multiple takes
                startbutton.addEventListener(
                    "click",
                    (ev) => {
                        takepicture();
                        ev.preventDefault();
                    },
                    false,
                );*/


       // clearphoto();
    }

    ////Stes canvas to gray, saves image, displays that image to clear captured image.
    //function clearphoto() {

    //    const context = canvas.getContext("2d");
    //    context.fillStyle = "#AAA";
    //    context.fillRect(0, 0, canvas.width, canvas.height);

    //    //Sets to PNG
    //    const data = canvas.toDataURL("image/png");
    //    photo.setAttribute("src", data);
    //}

  
    function onScanSuccess(decodedText, decodedResult) {
        // handle the scanned code as you likee:

        document.getElementById('resultText').textContent = `Code matched = ${decodedText}`, decodedResult;
        console.log(`Code matched = ${decodedText}`, decodedResult);
    }

    function onScanFailure(error) {
        // handle scan failure, usually better to ignore and keep scanning.
        // for example:
       // console.warn(`Code scan error = ${error}`);
    }

    let html5QrcodeScanner = new Html5QrcodeScanner(
        "reader",
        { fps: 10, showTorchButtonIfSupported: true, showZoomSliderIfSupported: true,qrbox: { width: 550, height: 550 } },
  /* verbose= */ false);

    html5QrcodeScanner.render(onScanSuccess, onScanFailure);


    // Set up  event listener to run the startup process
    // once loading is complete.
    window.addEventListener("load", startup, false);
})();