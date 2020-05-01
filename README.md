# PWA Notes App
Welcome to the PWA Notes app project. The app allows you to add notes with text, images, and geo location. The purpose of the project is to provide an open source and free-to-use reference implementation for building Proegressive Web Apps (PWAs) using Microsoft ASP.NET Core.

## Technology Stack
The following is the full list of client-side technologies used in the project:
* **Core web technologies:** HTML, CSS, JavaScript
* **Layout / styling:** Bootstrap 4
* **Icons:** FontAwesome 5

The following is the full list of server-side technologies used in the project:
* **Web server framework:** Microsoft ASP.NET Core 3.1
* **Programming Language:** C#
* **ORM:** Entity Framework Core
* **Database:** SQL Server
* **Cloud platform:** Azure (optional)

## PWA-Specific Code and Resources in the Project
1. JSON-based manifest file.
2. JavaScript service worker file.
3. Registration of service worker.

### 1. Manifest File
The manifest file **/wwwroot/manifest.json** is added as a static resource. The manifest file is a basic requirement for a PWA app:

```
{
  "name": "PWA Notes App",
  "short_name": "Notes",
  "start_url": "/",
  "display": "standalone",
  "background_color": "#2270bf",
  "theme_color": "#2270bf",
  "orientation": "portrait-primary",
  "scope": "/",
  "prefer_related_applications": false,
  "icons": [
    {
      "src": "/img/icons/icon-72.png",
      "type": "image/png",
      "sizes": "72x72"
    },
    {
      "src": "/img/icons/icon-96.png",
      "type": "image/png",
      "sizes": "96x96"
    },
    {
      "src": "/img/icons/icon-128.png",
      "type": "image/png",
      "sizes": "128x128"
    },
    {
      "src": "/img/icons/icon-144.png",
      "type": "image/png",
      "sizes": "144x144"
    },
    {
      "src": "/img/icons/icon-152.png",
      "type": "image/png",
      "sizes": "152x152"
    },
    {
      "src": "/img/icons/icon-192.png",
      "type": "image/png",
      "sizes": "192x192"
    },
    {
      "src": "/img/icons/icon-384.png",
      "type": "image/png",
      "sizes": "384x384"
    },
    {
      "src": "/img/icons/icon-512.png",
      "type": "image/png",
      "sizes": "512x512"
    }
  ]
}
```
The following is the explanation of some of these manifest properties:
* **name** is used when the app is installed. Required if **short_name** is not specified.
* **short_name** is used on the user's home screen or launcher. Required if **name** is not specified.
* **icons** are use on the home screen, launcher, task switcher, splash screen, etc. Icons of 192x192 and 512x512 pixel sizes are required by Chrome.
* **start_url** specifies the starting URL when the app is launched. This property is required.
* **display** is used to select what browser controls are shown when the app is launched. The possible values can be **fullscreen**, **standalone**, **minimal-u**, and **browser**.
* **scope** defines the URL structure that encompasses all the entry and exit points in the web app. **start_url** must be within the scope.

Reference to the manifest file is in the **/Views/Shared/\_Layout.cshtml** file to ensure that its replicated in all the views:
```
 <link rel="manifest" href="~/manifest.json">
 ```

### 2. Service Worker File
The service worker file **/wwwroot/service-worker.js** is added as a static resource. Besides the manifest file, a service worker is another basic requirement for a PWA app:
```
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
```
### 3. Registeration of Service Worker
The service worker is registered in the **/wwwroot/js/site.js**:
```
if ('serviceWorker' in navigator) {
    navigator.serviceWorker.register('/service-worker.js')
        .then(reg => console.log('service worker registered', reg))
        .catch(err => console.log('service worker not registered', err));
}
```
The **site.js** script is referenced in the **/Views/Shared/\_Shared/Layout.cshtml** file:
```
<script src="~/js/site.js" asp-append-version="true"></script>
```
