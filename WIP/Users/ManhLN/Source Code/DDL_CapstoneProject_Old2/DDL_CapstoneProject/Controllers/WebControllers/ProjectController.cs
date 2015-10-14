using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DDL_CapstoneProject.Controllers.WebControllers
{
    public class ProjectController : Controller
    {
        // GET: Projects/Create
        public ActionResult Create()
        {
            return PartialView("_CreateProject");
        }
    }
}