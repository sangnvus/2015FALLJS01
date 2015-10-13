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
    public class ProjectRepository : SingletonBase<ProjectRepository>
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
                CreatedDate = DateTime.Today,
                Risk = string.Empty,
                ImageUrl = string.Empty,
                SubDescription = string.Empty,
                Location = string.Empty,
                IsExprired = false,
                CurrentFunded = 0,
                ExpireDate = DateTime.Today,
                FundingGoal = 0,
                Description = string.Empty,
                VideoUrl = string.Empty,
                PopularPoint = 0,
                Status = DDLConstants.ProjectStatus.DRAFT,
                RewardPkgs = new List<RewardPkg>
                {
                    new RewardPkg
                    {
                        Type = DDLConstants.RewardType.NO_REWARD,
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
                        CreatedDate = DateTime.Today,
                    }
                },
                Timelines = new List<Timeline>
                {
                    new Timeline
                    {
                        Title = string.Empty,
                        Description = string.Empty,
                        DueDate = DateTime.Today,
                    }
                },
                Questions = new List<Question>
                {
                    new Question
                    {
                        Answer = string.Empty,
                        QuestionContent = string.Empty,
                        CreatedDate = DateTime.Today,
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

            // Create projectCode
            string projectIDString = project.ProjectID.ToString();
            int plusLength = 6 - projectIDString.Length;
            string plusCode = string.Concat(Enumerable.Repeat("0", plusLength));
            project.ProjectCode = "PRJ" + plusCode + projectIDString;

            db.SaveChanges();

            return project;
        }

        /// <summary>
        /// Edit a project
        /// </summary>
        /// <param name="project">object</param>
        /// <returns>updateProject</returns>
        public Project EditProjectBasic(ProjectDetailDTO project, string uploadImageName)
        {
            var updateProject = db.Projects.SingleOrDefault(u => u.ProjectID == project.ProjectID);

            if (updateProject == null)
            {
                throw new Exception();
            }

            if (uploadImageName != string.Empty)
            {
                updateProject.ImageUrl = DDLConstants.FileType.PROJECT + uploadImageName;
            }

            updateProject.CategoryID = project.CategoryID;
            updateProject.ExpireDate = project.ExpireDate;
            updateProject.FundingGoal = project.FundingGoal;
            updateProject.Location = project.Location;
            updateProject.Status = project.Status;
            updateProject.SubDescription = project.SubDescription;
            updateProject.Title = project.Title;


            db.SaveChanges();

            return updateProject;
        }

        /// <summary>
        /// Edit project's story
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public Project EditProjectStory(ProjectStoryDTO project)
        {
            var updateProject = db.Projects.SingleOrDefault(u => u.ProjectID == project.ProjectID);

            if (updateProject == null)
            {
                throw new Exception();
            }

            updateProject.Risk = project.Risk;
            updateProject.VideoUrl = project.VideoUrl;
            updateProject.Description = project.Description;

            db.SaveChanges();

            return updateProject;
        }

        /// <summary>
        /// Get a project
        /// </summary>
        /// <param name="ProjectID">int</param>
        /// <returns>project</returns>
        public ProjectEditDTO GetProjectBasic(int ProjectID)
        {
            var project = db.Projects.SingleOrDefault(x => x.ProjectID == ProjectID);

            if (project == null)
            {
                throw new KeyNotFoundException();
            }

            var projectDTO = new ProjectEditDTO
            {
                ProjectID = project.ProjectID,
                ProjectCode = project.ProjectCode,
                CategoryID = project.CategoryID,
                Title = project.Title,
                ImageUrl = project.ImageUrl,
                SubDescription = project.SubDescription,
                Location = project.Location,
                ExpireDate = project.ExpireDate,
                FundingGoal = project.FundingGoal,
                Status = project.Status,
                CurrentFunded = project.CurrentFunded,
            };

            return projectDTO;
        }

        public ProjectStoryDTO GetProjectStory(int ProjectID)
        {
            var project = db.Projects.SingleOrDefault(x => x.ProjectID == ProjectID);

            if (project == null)
            {
                throw new KeyNotFoundException();
            }

            var projectBasicDTO = new ProjectStoryDTO
            {
                ProjectID = project.ProjectID,
                Description = project.Description,
                Risk = project.Risk,
                VideoUrl = project.VideoUrl
            };

            return projectBasicDTO;
        }

        #endregion
    }
}