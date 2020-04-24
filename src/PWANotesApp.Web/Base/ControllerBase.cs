using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PWANotesApp.Web.Base
{
    public class ControllerBase : Controller
    {
        public string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public string GetUserName()
        {
            return User.FindFirstValue(ClaimTypes.Name);
        }
    }
}
