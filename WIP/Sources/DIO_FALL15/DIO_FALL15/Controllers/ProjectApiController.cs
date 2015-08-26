using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;
using System.Web.Http.Description;
using DIO_FALL15.Models;
using DIO_FALL15.Models.DTOs;
using DIO_FALL15.Respository;

namespace DIO_FALL15.Controllers
{
    public class ProjectApiController : ApiController
    {
        public DatabaseContext Db = new DatabaseContext();

        // GET: api/ProjectApi/GetAllProjects
        [HttpGet]
        [ResponseType(typeof(ProjectDetailDTO))]
        public IHttpActionResult GetAllProjects()
        {
            var listProject = Db.Projects.ToList();
            var listProjectDTO = new List<ProjectDetailDTO>();

            foreach (var project in listProject)
            {
                listProjectDTO.Add(
                    new ProjectDetailDTO
                    {
                        Id = project.Id,
                        Title = project.Title,
                        UserId = project.UserId,
                        Username = project.User.Username,
                        CategoryId = project.CategoryId,
                        Category = project.Category.Name,
                        Status = project.Status.ToString(),
                        Description = project.Description,
                        ImageLink = project.ImageLink,
                        CreatedDate = project.CreatedDate,
                        Deadline = project.Deadline,
                        TargetFund = project.TargetFund,
                        CurrentFund = project.CurrentFund
                    });
            }

            return Ok(listProjectDTO);
        }
    }
}
