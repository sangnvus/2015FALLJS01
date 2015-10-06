using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using DDL_CapstoneProject.Helper;
using DDL_CapstoneProject.Models.DTOs;
using DDL_CapstoneProject.Respository;

namespace DDL_CapstoneProject.Controllers.ApiControllers
{
    public class ProjectApiController : BaseApiController
    {
        // POST: api/ProjectApi/CreateProject
        [ResponseType(typeof (ProjectCreateDTO))]
        [HttpPost]
        public IHttpActionResult CreateProject(ProjectCreateDTO newProject)
        {

            var currentUser = getCurrentUser();

            if (currentUser == null)
            {
                return Ok(new HttpMessageDTO { Status = "error", Message = "Chưa đăng nhập!", Type = "UserNotFound" }); 
            }

            if (!ModelState.IsValid)
            {
                return Ok(new HttpMessageDTO { Status = "error", Message = "", Type = "Bad-Request" });
            }

            try
            {
                newProject.CreatorID = currentUser.DDL_UserID;
                var project = ProjectRepository.Instance.CreatProject(newProject);
            }
            catch (Exception)
            {
                
                throw;
            }
            return Ok(new HttpMessageDTO { Status = "success", Message = "", Type = "" });
        }

        // PUT: api/ProjectApi/Edit  
        [ResponseType(typeof(ProjectEditDTO))]
        [HttpPut]
        public IHttpActionResult EditProject(int ProjectID, ProjectEditDTO project)
        {
            if (project == null)
            {
                return Ok(new HttpMessageDTO { Status = "error", Message = "", Type = "Bad-Request" });
            }

            var updateProject = ProjectRepository.Instance.EditProject(ProjectID, project);

            return Ok(new HttpMessageDTO { Status = "success", Message = "", Type = "" });
        }
    }
}