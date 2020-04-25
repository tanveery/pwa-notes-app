/*
 * THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE
 * AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE
 * OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

const assetsCacheName = 'AssetsCache-v1';
const dynamicCacheName = 'DynamicCache-v1';

const assetsList = [
    '/',
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

self.addEventListener('install', event => {
    event.waitUntil(caches.open(assetsCacheName).then(cache => {
        cache.addAll(assetsList);
    }));
});

self.addEventListener('activate', event => {
    event.waitUntil(
        caches.keys().then(keys => {
            return Promise.all(keys
                .filter(key => key !== assetsCacheName && key !== dynamicCacheName)
                .map(key => caches.delete(key))
            )
        })
    );
});

self.addEventListener('fetch', event => {
    event.respondWith(
        caches.match(event.request).then(cacheResponse => {
            return cacheResponse || fetch(event.request).then(serverResponse => {
                return caches.open(dynamicCacheName).then(cache => {
                    cache.put(event.request.url, serverResponse.clone());
                    return serverResponse;
                })
            });
        }).catch(() => {
            if (event.request.url.indexOf('.png') > -1 || event.request.url.indexOf('.jpg') > -1 || event.request.url.indexOf('.jpeg') > -1) {
                return caches.match('/img/offline-image.png');
            }
            else if (event.request.url.indexOf('.css') > -1 || event.request.url.indexOf('.js') > -1) {
                // ignore the non-availability of CSS or JS files.
            }
            else {
                return caches.match('/error/offline');
            }
        })
    );
});