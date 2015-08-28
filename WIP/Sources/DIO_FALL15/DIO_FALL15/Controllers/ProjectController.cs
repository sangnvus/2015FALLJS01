using DIO_FALL15.Respository;
using System;
using System.Collections.Generic;
using System.IO;
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

        // GET: Projects/Edit
        public ActionResult Edit()
        {
            return PartialView("_EditProject");
        }

        // GET: All current user projects
        public ActionResult ShowAllCurrentUserProject()
        {
            return PartialView("_ShowAllCurrentUserProject");
        }


        // Post: Upload Image
        [HttpPost]
        public ActionResult fileUpload(HttpPostedFileBase file)
        {
            if (file != null)
            {
                if (file.ContentLength > 0)
                {
                    string FileName = Path.GetFileName(file.FileName);
                    string Extension = Path.GetExtension(file.FileName);
                    string FilePath = Server.MapPath("~/Images/" + FileName);

                    file.SaveAs(FilePath);
                    file.InputStream.Close();
                    file.InputStream.Dispose();

                }
            }
            return RedirectToAction("");

        }
        public ActionResult ProjectDetail()
        {
            return PartialView("_ProjectDetail");
        }


    }
}