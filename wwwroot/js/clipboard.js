window.copyToClipboard = (text) => {
    navigator.clipboard.writeText(text).then(() => {
        console.log('Copied to clipboard successfully!');
    }, (err) => {
        console.error('Failed to copy text: ', err);
    });
}
