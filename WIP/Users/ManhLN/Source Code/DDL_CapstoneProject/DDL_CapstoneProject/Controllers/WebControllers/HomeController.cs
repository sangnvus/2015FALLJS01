using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DDL_CapstoneProject.Helper;
using DDL_CapstoneProject.Respository;

namespace DDL_CapstoneProject.Controllers.WebControllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            getCurrentUser();
            ViewBag.BaseUrl = GetBaseUrl();
            return View();
        }
        public ActionResult Discover()
        {

            return PartialView("Discover");
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}