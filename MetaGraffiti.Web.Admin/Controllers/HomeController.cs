using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MetaGraffiti.Web.Admin.Models;

namespace MetaGraffiti.Web.Admin.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View(new AdminViewModel());
        }
    }
}