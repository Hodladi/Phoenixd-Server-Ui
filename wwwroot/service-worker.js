importScripts('https://storage.googleapis.com/workbox-cdn/releases/5.1.2/workbox-sw.js');

const cacheName = 'wallet-cache-v4'; // Increment cache name to ensure new cache is used
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

const offlineFallbackPage = "/offline";

// Pre-cache app shell files
workbox.precaching.precacheAndRoute(appShellFiles.map(url => {
    return { url, revision: null };
}));

self.addEventListener('install', event => {
    console.log('Service Worker: Installed');
    event.waitUntil(
        caches.open(cacheName).then(cache => {
            console.log('Service Worker: Caching Files');
            return cache.addAll(appShellFiles)
                .then(() => cache.add(new Request(offlineFallbackPage, { mode: 'no-cors' })))
                .catch(error => {
                    console.error('Failed to cache offline fallback page:', error);
                });
        })
    );
    self.skipWaiting();
});

// Remove old caches during activate
self.addEventListener('activate', event => {
    console.log('Service Worker: Activating');
    event.waitUntil(
        caches.keys().then(keyList => {
            return Promise.all(keyList.map(key => {
                if (key !== cacheName) {
                    console.log('Service Worker: Removing Old Cache', key);
                    return caches.delete(key);
                }
            }));
        })
    );
    return self.clients.claim();
});

// Handle fetch events
self.addEventListener('fetch', event => {
    console.log('Service Worker: Fetching', event.request.url);
    if (event.request.mode === 'navigate') {
        event.respondWith((async () => {
            try {
                const preloadResp = await event.preloadResponse;
                if (preloadResp) {
                    return preloadResp;
                }
                const networkResp = await fetch(event.request);
                return networkResp;
            } catch (error) {
                console.log('Fetch failed; returning offline page instead.', error);
                const cache = await caches.open(cacheName);
                const cachedResp = await cache.match(offlineFallbackPage);
                return cachedResp;
            }
        })());
    } else {
        event.respondWith(
            caches.match(event.request).then(response => {
                return response || fetch(event.request);
            })
        );
    }
});

// Skip waiting and activate new service worker immediately
self.addEventListener('message', event => {
    if (event.data && event.data.type === "SKIP_WAITING") {
        self.skipWaiting();
    }
});

// Periodic sync to keep the app alive
self.addEventListener('periodicsync', event => {
    if (event.tag === 'keep-alive') {
        event.waitUntil(fetch('/'));
    }
});
