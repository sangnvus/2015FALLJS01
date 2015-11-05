using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DDL_CapstoneProject.Helper;

namespace DDL_CapstoneProject.Controllers.WebControllers
{
    public class AdminPartialController : BaseController
    {
        // GET: AdminPartial/ReportProject
        public ActionResult ReportProject()
        {
            return PartialView("~/Views/AdminPartial/_ReportProject.cshtml");
        }
        // GET: AdminPartial/ReportUser
        public ActionResult ReportUser()
        {
            return PartialView("~/Views/AdminPartial/_ReportUser.cshtml");
        }
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
        
        // GET: AdminPartial/Slide
        public ActionResult Slide()
        {
            return PartialView("~/Views/AdminPartial/_Slide.cshtml");
        }
        // GET: AdminPartial/MessageList
        public ActionResult MessageList()
        {
            return PartialView("~/Views/AdminPartial/Message/_MessageList.cshtml");
        }

        // GET: AdminPartial/MessageDetail
        public ActionResult MessageDetail()
        {
            return PartialView("~/Views/AdminPartial/Message/_MessageDetail.cshtml");
        }

        public ActionResult UserList()
        {
            return PartialView("~/Views/AdminPartial/User/_UserList.cshtml");
        }

        public ActionResult UserProfile()
        {
            return PartialView("~/Views/AdminPartial/User/_UserProfile.cshtml");
        }

        public ActionResult UserDashboard()
        {
            return PartialView("~/Views/AdminPartial/User/_UserDashboard.cshtml");
        }

        // GET: AdminPartial/NotFound
        public ActionResult NotFound()
        {
            return PartialView("~/Views/AdminPartial/Error/_NotFound.cshtml");
        }

        // GET: AdminPartial/NotFound
        public ActionResult Error()
        {
            return PartialView("~/Views/AdminPartial/Error/_Error.cshtml");
        }

        public ActionResult BackingList()
        {
            return PartialView("~/Views/AdminPartial/_BackingList.cshtml");
        }
    }
}