using System;
using System.Collections.Generic;
using System.Linq;

using MetaGraffiti.Web.Admin.Models;
using Microsoft.AspNetCore.Mvc;

namespace MetaGraffiti.Web.Admin.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(new AdminViewModel());
        }
    }
}