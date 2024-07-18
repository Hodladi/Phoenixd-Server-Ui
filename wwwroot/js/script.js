function showModal(modalId) {
    var modal = new bootstrap.Modal(document.getElementById(modalId));
    modal.show();
}

window.scrollToTransactions = function () {
    var transactionsList = document.querySelector('.transactions-list');
    if (transactionsList) {
        transactionsList.classList.add('show');
        window.scrollTo({
            top: transactionsList.offsetTop,
            behavior: 'smooth'
        });
    }
}

window.scrollToTop = function () {
    var transactionsList = document.querySelector('.transactions-list');
    if (transactionsList) {
        transactionsList.classList.remove('show');
        window.scrollTo({
            top: 0,
            behavior: 'smooth'
        });
    }
}

window.showMenu = function () {
    var menuList = document.querySelector('.side-menu-content');
    if (menuList) {
        menuList.classList.add('show');
        window.scrollTo({
            top: 0,
            behavior: 'smooth'
        });
    }
}

window.hideMenu = function () {
    var menuList = document.querySelector('.side-menu-content');
    if (menuList) {
        menuList.classList.remove('show');
        window.scrollTo({
            top: menuList.offsetTop,
            behavior: 'smooth'
        });
    }
}

window.pasteFromClipboard = async function () {
    if (navigator.clipboard && navigator.clipboard.readText) {
        try {
            const text = await navigator.clipboard.readText();
            return text;
        } catch (err) {
            console.error('Failed to read clipboard contents using Clipboard API: ', err);
        }
    }

    // Fallback for browsers that do not support the Clipboard API
    try {
        const textArea = document.createElement('textarea');
        document.body.appendChild(textArea);
        textArea.focus();
        document.execCommand('paste');
        const text = textArea.value;
        document.body.removeChild(textArea);
        return text;
    } catch (err) {
        console.error('Failed to read clipboard contents using execCommand fallback: ', err);
        return '';
    }
};

function checkLocalStorage() {
    if (!localStorage.getItem('baseUrl') || !localStorage.getItem('password')) {
        DotNet.invokeMethodAsync('Wallet', 'ShowSettingsModal');
    }
}

function saveToLocalStorage(key, value) {
    localStorage.setItem(key, value);
}

document.addEventListener('focusin', (event) => {
    if (event.target.tagName === 'INPUT' || event.target.tagName === 'TEXTAREA') {
        setTimeout(() => {
            event.target.scrollIntoView({ behavior: 'smooth', block: 'center' });
        }, 300);
    }
});

if ('serviceWorker' in navigator && 'periodicSync' in navigator.serviceWorker) {
    navigator.serviceWorker.ready.then((registration) => {
        registration.periodicSync.register('keep-alive', {
            minInterval: 24 * 60 * 60 * 1000 // 24 hours
        });
    });
}