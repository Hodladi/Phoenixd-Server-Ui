const cacheName = 'app-cache-v3';
const appShellFiles = [
    '/',
    '/manifest.json',
    '/css/app.css',
    '/css/font.css',
    '/media/icon.png',
    '/fonts/PixeloidMono.ttf',
    '/fonts/PixeloidSans.ttf',
    '/fonts/PixeloidSans-Bold.ttf',
    '/js/qr.js',
    '/js/script.js'
];

self.addEventListener('install', (e) => {
    console.log('Service Worker: Installed');
    e.waitUntil(
        caches.open(cacheName).then((cache) => {
            console.log('Service Worker: Caching Files');
            return cache.addAll(appShellFiles);
        })
    );
});

self.addEventListener('activate', (event) => {
    console.log('Service Worker: Activating');
    event.waitUntil(
        caches.keys().then((keyList) => {
            return Promise.all(keyList.map((key) => {
                if (key !== cacheName) {
                    console.log('Service Worker: Removing Old Cache', key);
                    return caches.delete(key);
                }
            }));
        })
    );
    return self.clients.claim();
});

self.addEventListener('fetch', (e) => {
    console.log('Service Worker: Fetching');
    e.respondWith(
        caches.match(e.request).then((response) => {
            return response || fetch(e.request);
        })
    );
});