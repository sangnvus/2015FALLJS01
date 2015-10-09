using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DDL_CapstoneProject.Controllers.WebControllers
{
    public class ClientPartialController : Controller
    {
        // GET: ClientPartial
        public ActionResult Home()
        {
            return PartialView("~/Views/Home/_Home.cshtml");
        }

        // GET: ClientPartial
        public ActionResult Register()
        {
            return PartialView("~/Views/Login/_Register.cshtml");
        }

        public ActionResult RegisterSuccess()
        {
            return PartialView("~/Views/Login/_RegisterSuccess.cshtml");
        }

        // GET: ClientPartial/CreateProject
        public ActionResult CreateProject()
        {
            return PartialView("~/Views/Project/_CreateProject.cshtml");
        }
		public ActionResult Message()
        {
            return PartialView("~/Views/Message/_MessageList.cshtml");
        }
        public ActionResult MessageDetail()
        {
            return PartialView("~/Views/Message/_MessageDetail.cshtml");
        }

		// GET: ClientPartial/EditProject
        public ActionResult EditProject()
        {
            return PartialView("~/Views/Project/_EditProject.cshtml");
        }
        // GET: ClientPartial/ProjectDetail
        public ActionResult ProjectDetail()
        {
            return PartialView("~/Views/Project/_ProjectDetail.cshtml");
        }
    }
}