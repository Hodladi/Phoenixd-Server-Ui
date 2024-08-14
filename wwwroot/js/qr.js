function startQrScanner(dotNetHelper) {
    const video = document.getElementById('qr-video');
    const constraints = {
        video: { facingMode: "environment" }
    };

    navigator.mediaDevices.getUserMedia(constraints).then((stream) => {
        video.srcObject = stream;
        video.setAttribute("playsinline", true);
        video.play();
        requestAnimationFrame(tick);

        window.currentStream = stream;
    });

    function tick() {
        if (video.readyState === video.HAVE_ENOUGH_DATA) {
            const canvas = document.createElement('canvas');
            canvas.height = video.videoHeight;
            canvas.width = video.videoWidth;
            const context = canvas.getContext('2d');
            context.drawImage(video, 0, 0, canvas.width, canvas.height);

            const imageData = context.getImageData(0, 0, canvas.width, canvas.height);
            const code = jsQR(imageData.data, imageData.width, imageData.height, {
                inversionAttempts: "dontInvert",
            });

            if (code) {
                video.srcObject.getTracks().forEach(track => track.stop());
                dotNetHelper.invokeMethodAsync('HandleQrCodeResult', code.data);
                hideModal('qrScannerModal');
            }
        }

        requestAnimationFrame(tick);
    }
}

function stopQrScanner() {
    if (window.currentStream) {
        window.currentStream.getTracks().forEach(track => track.stop());
        window.currentStream = null;
    }
}

function hideModal(modalId) {
    var modal = bootstrap.Modal.getInstance(document.getElementById(modalId));
    if (modal) {
        modal.hide();
    }
}

window.copyToClipboard = (text) => {
    navigator.clipboard.writeText(text).then(() => {
        console.log('Copied to clipboard successfully!');
    }, (err) => {
        console.error('Failed to copy text: ', err);
    });
}

window.pasteFromClipboard = async () => {
    try {
        const text = await navigator.clipboard.readText();
        return text;
    } catch (err) {
        console.error('Failed to read text from clipboard: ', err);
        return '';
    }
}
