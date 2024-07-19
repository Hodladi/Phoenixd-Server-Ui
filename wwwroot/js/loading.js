function showCustomLoading() {
    var loadingDiv = document.createElement('div');
    loadingDiv.className = 'custom-loading';
    loadingDiv.id = 'customLoadingDiv';
    loadingDiv.innerHTML = 'Reconnecting...';
    document.body.appendChild(loadingDiv);
}

function hideCustomLoading() {
    var loadingDiv = document.getElementById('customLoadingDiv');
    if (loadingDiv) {
        document.body.removeChild(loadingDiv);
    }
}

document.addEventListener('DOMContentLoaded', (event) => {
    Blazor.start().then(() => {
        Blazor.defaultReconnectionHandler.onReconnecting = () => {
            showCustomLoading();
        };
        Blazor.defaultReconnectionHandler.onReconnected = () => {
            hideCustomLoading();
        };
        Blazor.defaultReconnectionHandler.onDisconnected = () => {
            hideCustomLoading();
        };
    });
});
