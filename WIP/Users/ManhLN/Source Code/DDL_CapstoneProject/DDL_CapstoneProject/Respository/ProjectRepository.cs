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

        public Project CreateEmptyProject()
        {
            var project = new Project
            {
                ProjectCode = string.Empty,
                CategoryID = 0,
                CreatorID = 0,
                Title = string.Empty,
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

            return null;
        }

        #endregion
    }
}