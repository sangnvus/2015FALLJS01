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

        // GET: AdminPartial/ProjectDashboard
        public ActionResult ProjectDashboard()
        {
            return PartialView("~/Views/AdminPartial/Project/_ProjectDashboard.cshtml");
        }

        // GET: AdminPartial/ProjectList
        public ActionResult ProjectList()
        {
            return PartialView("~/Views/AdminPartial/Project/_ProjectList.cshtml");
        }

        // GET: AdminPartial/ProjectDetail
        public ActionResult ProjectDetail()
        {
            return PartialView("~/Views/AdminPartial/Project/_ProjectDetail.cshtml");
        }
    }
}