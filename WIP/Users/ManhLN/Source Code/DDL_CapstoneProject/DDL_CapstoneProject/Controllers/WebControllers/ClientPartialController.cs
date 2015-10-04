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
    }
}