using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DDL_CapstoneProject.Controllers.WebControllers
{
    public class AdminPartialController : Controller
    {
        // GET: AdminPartial/Dashboard
        public ActionResult Dashboard()
        {
            return PartialView("~/Views/AdminPartial/_Dashboard.cshtml");
        }

        // GET: AdminPartial/Category
        public ActionResult Category()
        {
            return PartialView("~/Views/AdminPartial/_Category.cshtml");
        }
    }
}