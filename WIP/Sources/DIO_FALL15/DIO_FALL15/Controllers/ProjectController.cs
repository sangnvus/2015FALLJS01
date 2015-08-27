using DIO_FALL15.Respository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DIO_FALL15.Controllers
{
    public class ProjectController : Controller
    {
        private readonly DatabaseContext _dbContext = new DatabaseContext();
        // GET: Projects/Create
        public ActionResult Create()
        {
            return PartialView("_CreateProject");
        }
    }
}