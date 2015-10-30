using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DDL_CapstoneProject.Helper;
using DDL_CapstoneProject.Respository;
using DDL_CapstoneProject.Ultilities;

namespace DDL_CapstoneProject.Controllers.WebControllers
{
    public class HomeController : BaseController
    {
        [Route("")]
        public ActionResult Index()
        {
            getCurrentUser();
            ViewBag.BaseUrl = GetBaseUrl();
            return View();
        }

        [Route("Admin")]
        public ActionResult AdminIndex()
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Redirect("/admin/login");
            }
            var currentUser = getCurrentUser();
            if (currentUser.UserType != DDLConstants.UserType.ADMIN)
            {
                return Redirect("/admin/login");
            }
            ViewBag.BaseUrl = GetBaseUrl() + "admin/";
            return View();
        }

        public ActionResult Discover()
        {

            return PartialView("_Discover");
        }

        [Route("Error")]
        public ActionResult Error()
        {

            return View("~/Views/Shared/Error.cshtml");
        }
    }
}