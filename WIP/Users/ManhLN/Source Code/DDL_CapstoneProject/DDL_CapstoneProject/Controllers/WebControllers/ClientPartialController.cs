using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DDL_CapstoneProject.Helper;

namespace DDL_CapstoneProject.Controllers.WebControllers
{
    public class ClientPartialController : BaseController
    {
        // GET: ClientPartial
        public ActionResult Home()
        {
            return PartialView("~/Views/Home/_Home.cshtml");
        }
        public ActionResult Discover()
        {
            return PartialView("~/Views/Home/_Discover.cshtml");
        }

        public ActionResult Statistics()
        {
            return PartialView("~/Views/Home/_Statistics.cshtml");
        }

        public ActionResult Search()
        {
            return PartialView("~/Views/Home/_Search.cshtml");
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

        // GET: ClientPartial/PublicProfile
        public ActionResult PublicProfile()
        {
            return PartialView("~/Views/User/_PublicProfile.cshtml");
        }

        public ActionResult EditProfile()
        {
            return PartialView("~/Views/User/_EditProfile.cshtml");
        }

        public ActionResult BackedProject()
        {
            return PartialView("~/Views/Project/_BackedProject.cshtml");
        }

        public ActionResult BackedProjectHistory()
        {
            return PartialView("~/Views/Project/_BackedProjectHistory.cshtml");
        }

        public ActionResult StarredProject()
        {
            return PartialView("~/Views/Project/_StarredProject.cshtml");
        }

        public ActionResult CreatedProject()
        {
            return PartialView("~/Views/Project/_CreatedProject.cshtml");
        }

        public ActionResult ListBacker()
        {
            return PartialView("~/Views/Project/_ListBacker.cshtml");
        }

        public ActionResult BackProject()
        {
            return PartialView("~/Views/Project/_BackProject.cshtml");
        }
        public ActionResult PaymentProject()
        {
            return PartialView("~/Views/Project/_PaymentProject.cshtml");
        }

        public ActionResult EditPassword()
        {
            return PartialView("~/Views/User/_EditPassword.cshtml");
        }

        public ActionResult Error()
        {
            return PartialView("~/Views/Shared/_Error.cshtml");
        }

        public ActionResult NotFound()
        {
            return PartialView("~/Views/Shared/_NotFound.cshtml");
        }
    }
}