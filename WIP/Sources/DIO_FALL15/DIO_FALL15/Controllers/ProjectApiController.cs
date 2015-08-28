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
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Web;

namespace DIO_FALL15.Controllers
{
    public class ProjectApiController : ApiController
    {
        public DatabaseContext Db = new DatabaseContext();

        // GET: api/ProjectApi/GetAllProjects
        [HttpGet]
        [ResponseType(typeof(ProjectCardDTO))]
        public IHttpActionResult GetAllProjects()
        {
            var listProject = Db.Projects.ToList();
            var listProjectDTO = new List<ProjectCardDTO>();

            foreach (var project in listProject)
            {
                listProjectDTO.Add(
                    new ProjectCardDTO
                    {
                        Id = project.Id,
                        Title = project.Title,
                        ImageLink = project.ImageLink,
                        Deadline = project.Deadline,
                        TargetFund = project.TargetFund,
                        CurrentFund = project.CurrentFund
                    });
            }

            return Ok(listProjectDTO);
        }

        // GET: api/ProjectApi/Project/:id
        [HttpGet]
        [ResponseType(typeof(ProjectCardDTO))]
        public IHttpActionResult GetProject(int id)
        {
            var project = Db.Projects.SingleOrDefault(x => x.Id == id);
            if (project == null)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Project not found!"));
            }

            var projectDTO = new ProjectDetailDTO
            {
                Id = project.Id,
                UserId = project.UserId,
                CategoryId = project.CategoryId,
                Username = project.User.Username,
                Category = project.Category.Name,
                Title = project.Title,
                ImageLink = project.ImageLink,
                Status = project.Status.ToString(),
                Description = project.Description,
                TargetFund = project.TargetFund,
                CurrentFund = project.CurrentFund,
                CreatedDate = project.CreatedDate,
                Deadline = project.Deadline,
                
            };

            return Ok(projectDTO);
        }

        // PUT: api/ProjectApi/Edit  
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProject(int id, Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != project.Id)
            {
                return BadRequest();
            }

            Db.Entry(project).State = EntityState.Modified;

            try
            {
                Db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {

            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ProjectApi/CreateProject 
        [ResponseType(typeof(CreateProjectDTO))]
        [HttpPost]
        public IHttpActionResult CreateProject(CreateProjectDTO project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var newProject = new Project
            {
                UserId = project.UserId,
                CategoryId = project.CategoryId,
                Title = project.Title,
                Description = project.Description,
                Deadline = project.Deadline,
                TargetFund = project.TargetFund,
                // hardcode
                CreatedDate = DateTime.Now,
                CurrentFund = 0,
                Status = Status.Active,
                ImageLink = project.ImageLink
            };
            Db.Projects.Add(newProject);
            Db.SaveChanges();

            //return CreatedAtRoute("DefaultApi", new { id = project.Id }, project);
            return Ok("Create project successfully!");
        }

        // PUT: api/ProjectApi/Edit  
        [ResponseType(typeof(ProjectDetailDTO))]
        [HttpPut]
        public IHttpActionResult EditProject(int id, ProjectDetailDTO project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updateProject =
                Db.Projects.SingleOrDefault(u => u.Id == id);

            if (updateProject == null)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Project not found!"));
            }

            updateProject.Title = project.Title;
            updateProject.CategoryId = project.CategoryId;
            updateProject.Description = project.Description;
            updateProject.Deadline = project.Deadline;
            updateProject.TargetFund = project.TargetFund;
            updateProject.ImageLink = project.ImageLink;

            Db.SaveChanges();

            return Ok("Updated Project!");
        }

        // GET: api/ProjectApi/GetAllCategories
        [HttpGet]
        [ResponseType(typeof(CategoryDTO))]
        public IHttpActionResult GetAllCategories()
        {
            var listCategories = Db.Categories.ToList();
            var listCategoryDTO = new List<CategoryDTO>();

            foreach (var Category in listCategories)
            {
                listCategoryDTO.Add(
                    new CategoryDTO
                    {
                        CategoryId = Category.Id,
                        Category = Category.Name,
                    });
            }

            return Ok(listCategoryDTO);
        }

        // GET: api/ProjectApi/GetAllCategories
        [HttpPut]
        [ResponseType(typeof(ProjectDetailDTO))]
        public IHttpActionResult BackProject(int id, Decimal amount)
        {
            var updateProject =
                Db.Projects.SingleOrDefault(u => u.Id == id);

            if (updateProject == null)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Project not found!"));
            }

            updateProject.CurrentFund += amount;
            Db.SaveChanges();

            return Ok(updateProject.CurrentFund);
        }
    }
}
