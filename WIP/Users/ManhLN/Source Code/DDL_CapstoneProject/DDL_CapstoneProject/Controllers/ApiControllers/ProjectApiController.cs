﻿using System;
using System.Collections.Generic;
using System.Linq;
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
            if (!ModelState.IsValid)
            {
                return Ok(new HttpMessageDTO { Status = "error", Message = "", Type = "Bad-Request" });
            }

            try
            {
                var project = ProjectRepository.Instance.CreatProject(newProject);
            }
            catch (Exception)
            {
                
                throw;
            }
            return Ok(new HttpMessageDTO { Status = "success", Message = "", Type = "" });
        }
    }
}