/*
 * THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE
 * AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE
 * OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

// The name of the cache. This name (the version part) should be changed whenever there is a change
// in the list of resources to be cached so that the browser creates a brand new cache.
const cacheName = 'cache-v1';

// The list of resources to be cached.
const resourcesToPrecache = [
    '/',
    '/home',
    '/home/index',
    '/error/offline',
    '/lib/bootstrap/dist/css/bootstrap.min.css',
    '/lib/fontawesome/css/all.css',
    '/css/site.css',
    '/lib/jquery/dist/jquery.min.js',
    '/lib/bootstrap/dist/js/bootstrap.bundle.min.js',
    '/lib/fontawesome/js/all.js',
    '/js/site.js',
    '/img/offline-image.png',
    '/img/favicon.png',
    '/img/icons/icon-96.png',
    '/img/icons/icon-128.png',
    '/img/icons/icon-144.png',
    '/img/icons/icon-152.png',
    '/img/icons/icon-192.png',
    '/img/icons/icon-384.png',
    '/img/icons/icon-512.png'
];

// The event that is fired after the service worker is registered for the first time or there is change in the service worker.
// In this event, all the resources in the resourcesToPrecache array are cached.
self.addEventListener('install', event => {
    event.waitUntil(
        caches.open(cacheName)
            .then(cache => {
                return cache.addAll(resourcesToPrecache);
            })
    );
});

// After installation, the next event to fire is "activate". However, if an older version of the
// service worker is still being used by one or more pages then this event will be in "waiting" until
// those pages are closed.
// In this event, we are iterating through all the caches where the key doesn't equal to the current key.
// This effectively ensures that all the older caches are removed. This is why making a change to the 
// cache name whenever an update to the cache is required.
self.addEventListener('activate', event => {
    event.waitUntil(
        caches.keys().then(keys => {
            return Promise.all(keys
                .filter(key => key !== cacheName)
                .map(key => caches.delete(key))
            )
        })
    );
});

// The "fetch" event is fired every time a resource is requested from the web server. Therefore, its the ideal event
// apply cache related logic.
// In this event, we try to load a resource from the cache first and if it doesn't exist in the cache then we allow the resource
// to be fetched from the web server. As expected, there would be an error if the internet connection is not working which is why
// the catch block allows us to load an offline resource (part of the cache).
self.addEventListener('fetch', event => {
    event.respondWith(
        caches.match(event.request)
            .then(cacheResponse => {
                return cacheResponse || fetch(event.request);
            })
            .catch(() => {
                // If the resource is an image that can't be fetched from the server then load offline-image.png instead.
                if (event.request.url.indexOf('.png') > -1 || event.request.url.indexOf('.jpg') > -1 || event.request.url.indexOf('.jpeg') > -1) {
                    return caches.match('/img/offline-image.png');
                }
                else if (event.request.url.indexOf('.css') > -1 || event.request.url.indexOf('.js') > -1) {
                    // ignore the non-availability of CSS or JS files.
                }
                else {
                    // It probably is a page and therefore show the offline view isntead which is part of the cache.
                    return caches.match('/error/offline');
                }
            })
    );
});
