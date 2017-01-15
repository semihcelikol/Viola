using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Viola.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Error404(string aspxerrorpath)
        {
            return View();
        }
    }
}