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
2. Reference to the manifest file in HTML.
3. JavaScript service worker file.
4. Initialization of service worker in HTML.

### Manifest File
The manifest file **manifest.json** is located in the **wwwroot** folder as a static resource. The manifest file is a basic requirement for a PWA app:

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
