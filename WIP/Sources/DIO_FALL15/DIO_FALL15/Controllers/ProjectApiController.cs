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
                CurrentFund = 10000,
                Status = Status.Active,
                ImageLink = "Image1.jpg"
            };
            Db.Projects.Add(newProject);
            Db.SaveChanges();

            //return CreatedAtRoute("DefaultApi", new { id = project.Id }, project);
            return Ok("Create project successfully!");
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
    }
}
