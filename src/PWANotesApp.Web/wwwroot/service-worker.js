self.addEventListener('install', (event) => {
    console.log('install handler', event);
});

self.addEventListener('activate', (event) => {
    console.log('activate handler', event);
});

self.addEventListener("fetch", (event) => {
    console.log('fetch handler', event);
});