document.addEventListener('DOMContentLoaded', function () {

    document.getElementById('shareButton').addEventListener('click', async () => {
        if (navigator.share) {
            const img = document.getElementById('qrImage');
            const base64Image = img.src.split(',')[1];
            const blob = await fetch(`data:image/png;base64,${base64Image}`).then(res => res.blob());
            const file = new File([blob], 'QRCode.png', { type: blob.type });

            try {
                await navigator.share({
                    title: 'Check This Out',
                    text: 'This right hur',
                    files: [file],

                });
                console.log('content shared');
            } catch (error) {
                console.error('error sharing', error)
            }
        } else {
            alert('Sharing not allowed in this browser')
        }
    });

 document.getElementById('printButton').addEventListener('click', () => {
        const img = document.getElementById('qrImage');
        if (img.complete) {
            openPrintWindow(img.src);
        } else {
            img.addEventListener('load', () => {
                openPrintWindow(img.src);
            });
        }
    });

    function openPrintWindow(src) {
        const printWindow = window.open('', '_blank');
        printWindow.document.write('<html><head><title>Print QR Code</title></head><body>');
        printWindow.document.write('<img src="' + src + '" style="max-width: 100%; height: auto;">');
        printWindow.document.write('</body></html>');
        printWindow.document.close();
        printWindow.focus();
        printWindow.print();
    }

});