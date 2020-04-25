const assetsCache = "AssetsCache";
const assetsList = [
    "/",
    "/lib/bootstrap/dist/css/bootstrap.min.css",
    "/lib/fontawesome/css/all.css",
    "/css/site.css",
    "/lib/jquery/dist/jquery.min.js",
    "/lib/bootstrap/dist/js/bootstrap.bundle.min.js",
    "/lib/fontawesome/js/all.js",
    "/js/site.js",
    "/img/favicon.png",
    "/img/icons/icon-96.png",
    "/img/icons/icon-128.png",
    "/img/icons/icon-144.png",
    "/img/icons/icon-152.png",
    "/img/icons/icon-192.png",
    "/img/icons/icon-384.png",
    "/img/icons/icon-512.png"
];
self.addEventListener('install', (event) => {
    event.waitUntil(caches.open(assetsCache).then(cache => {
        cache.addAll(assetsList);
    }));
});

self.addEventListener('activate', (event) => {
    console.log('activate handler', event);
});

self.addEventListener("fetch", (event) => {
    event.respondWith(
        caches.match(event.request).then(cacheResponse => {
            return cacheResponse || fetch(event.request);
        }))
});