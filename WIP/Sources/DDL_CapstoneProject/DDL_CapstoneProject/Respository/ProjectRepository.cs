using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DDL_CapstoneProject.Helpers;
using DDL_CapstoneProject.Models;
using DDL_CapstoneProject.Models.DTOs;
using DDL_CapstoneProject.Ultilities;

namespace DDL_CapstoneProject.Respository
{
    public class ProjectRepository: SingletonBase<ProjectRepository>
    {
        private DDLDataContext db;

        #region "Constructors"
        private ProjectRepository()
        {
            db = new DDLDataContext();
        }
        #endregion

        #region "Methods"

        /// <summary>
        /// Initial a empty project
        /// </summary>
        /// <returns>emptyProject</returns>
        public Project CreateEmptyProject()
        {
            var project = new Project
            {
                ProjectCode = string.Empty,
                CategoryID = 0,
                CreatorID = 0,
                Title = string.Empty,
                CreatedDate = DateTime.Now,
                Risk = string.Empty,
                ImageUrl = string.Empty,
                SubDescription = string.Empty,
                Location = string.Empty,
                IsExprired = false,
                CurrentFunded = 0,
                ExpireDate = null,
                FundingGoal = 0,
                Description = string.Empty,
                VideoUrl = string.Empty,
                PopularPoint = 0,
                Status = DDLConstants.ProjectStatus.DRAFT,
                RewardPkgs = new List<RewardPkg>
                {
                    new RewardPkg
                    {
                        Type = string.Empty,
                        IsHide = false,
                        EstimatedDelivery = null,
                        Quantity = 0,
                        Description = string.Empty,
                    }
                },
                UpdateLogs = new List<UpdateLog>
                {
                    new UpdateLog
                    {
                        Title = string.Empty,
                        Description = string.Empty,
                        CreatedDate = DateTime.Now,
                    }
                },
                Timelines = new List<Timeline>
                {
                    new Timeline
                    {
                        Title = string.Empty,
                        Description = string.Empty,
                        DueDate = DateTime.Now,
                    }
                },
                Questions = new List<Question>
                {
                    new Question
                    {
                        Answer = string.Empty,
                        QuestionContent = string.Empty,
                        CreatedDate = DateTime.Now,
                    }
                },
            };

            return project;
        }

        /// <summary>
        /// Create a new project
        /// </summary>
        /// <returns>project</returns>
        public Project CreatProject(ProjectCreateDTO newProject)
        {
            Project project;
            project = CreateEmptyProject();
            project.CreatorID = newProject.CategoryID;
            project.Title = newProject.Title;
            project.CategoryID = newProject.CategoryID;
            project.CreatorID = newProject.CreatorID;
            project.FundingGoal = newProject.FundingGoal;

            db.Projects.Add(project);
            db.SaveChanges();

            return project;
        }

        /// <summary>
        /// Edit a project
        /// </summary>
        /// <param name="ProjectID">int</param>
        /// <param name="project">object</param>
        /// <returns>updateProject</returns>
        public Project EditProject(int ProjectID, ProjectEditDTO project)
        {
            var updateProject = db.Projects.SingleOrDefault(u => u.ProjectID == ProjectID);
            
            if (updateProject == null)
            {
                throw new Exception();
            }

            updateProject.RewardPkgs = project.RewardPkgs;
            updateProject.CategoryID = project.CategoryID;
            updateProject.Description = project.Description;
            updateProject.ExpireDate = project.ExpireDate;
            updateProject.FundingGoal = project.FundingGoal;
            updateProject.ImageUrl = project.ImageUrl;
            updateProject.Location = project.Location;
            updateProject.Questions = project.Questions;
            updateProject.Risk = project.Risk;
            updateProject.Status = project.Status;
            updateProject.SubDescription = project.SubDescription;
            updateProject.Timelines = project.Timelines;
            updateProject.Title = project.Title;
            updateProject.UpdateLogs = project.UpdateLogs;
            updateProject.VideoUrl = project.VideoUrl;

            db.SaveChanges();

            return updateProject;
        }

        /// <summary>
        /// Get a project
        /// </summary>
        /// <param name="ProjectID">int</param>
        /// <returns>project</returns>
        public Project GetProject(int ProjectID)
        {
            var project = db.Projects.SingleOrDefault(x => x.ProjectID == ProjectID);

            if (project == null)
            {
                throw new Exception();
            }

            return project;
        }

        #endregion
    }
}