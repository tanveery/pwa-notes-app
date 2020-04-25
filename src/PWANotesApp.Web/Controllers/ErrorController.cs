using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PWANotesApp.Web.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Offline()
        {
            return View();
        }
    }
}
