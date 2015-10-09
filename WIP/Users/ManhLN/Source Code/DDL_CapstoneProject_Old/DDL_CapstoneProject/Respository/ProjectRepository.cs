using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc.Html;
using DDL_CapstoneProject.Helper;
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

            updateProject.CategoryID = project.CategoryID;
            updateProject.Description = project.Description;
            updateProject.ExpireDate = project.ExpireDate;
            updateProject.FundingGoal = project.FundingGoal;
            updateProject.ImageUrl = project.ImageUrl;
            updateProject.Location = project.Location;
            updateProject.Risk = project.Risk;
            updateProject.Status = project.Status;
            updateProject.SubDescription = project.SubDescription;
            updateProject.Title = project.Title;
            updateProject.VideoUrl = project.VideoUrl;


            db.SaveChanges();

            return updateProject;
        }

        /// <summary>
        /// Get a project
        /// </summary>
        /// <param name="ProjectID">int</param>
        /// <returns>project</returns>
        public ProjectEditDTO GetProject(int ProjectID)
        {
            var project = db.Projects.SingleOrDefault(x => x.ProjectID == ProjectID);

            if (project == null)
            {
                throw new KeyNotFoundException();
            }

            var projectDTO = new ProjectEditDTO
            {
                ProjectID = project.ProjectID,
                CreatorID = project.CreatorID,
                CategoryID = project.CategoryID,
                Title = project.Title,
                Risk = project.Risk,
                ImageUrl = project.Risk,
                SubDescription = project.SubDescription,
                Location = project.Location,
                ExpireDate = project.ExpireDate,
                FundingGoal = project.FundingGoal,
                Description = project.Description,
                VideoUrl = project.VideoUrl,
                Status = project.Status
            };

            // Get rewardPkg list
            var rewardList = from RewardPkg in db.RewardPkgs
                             where RewardPkg.ProjectID == ProjectID
                             orderby RewardPkg.Type ascending
                             select new RewardPkgDTO()
                             {
                                 Description = RewardPkg.Description,
                                 EstimatedDelivery = RewardPkg.EstimatedDelivery,
                                 IsHide = RewardPkg.IsHide,
                                 Quantity = RewardPkg.Quantity,
                                 RewardPkgID = RewardPkg.RewardPkgID,
                                 Type = RewardPkg.Type
                             };
            // Get updatelog list
            var updateLogList = from UpdateLog in db.UpdateLogs
                                where UpdateLog.ProjectID == ProjectID
                                orderby UpdateLog.CreatedDate descending
                                select new UpdateLogDTO()
                                {
                                    Description = UpdateLog.Description,
                                    Title = UpdateLog.Title,
                                    CreatedDate = UpdateLog.CreatedDate,
                                    UpdateLogID = UpdateLog.UpdateLogID,
                                };
            // Get timeline list
            var timelineList = from Timeline in db.Timelines
                               where Timeline.ProjectID == ProjectID
                               orderby Timeline.DueDate ascending
                               select new TimeLineDTO()
                               {
                                   Description = Timeline.Description,
                                   Title = Timeline.Title,
                                   ImageUrl = Timeline.ImageUrl,
                                   DueDate = Timeline.DueDate,
                                   TimelineID = Timeline.TimelineID
                               };
            // Get question list
            var questionList = from Question in db.Questions
                               where Question.ProjectID == ProjectID
                               orderby Question.CreatedDate descending
                               select new QuestionDTO()
                               {
                                   CreatedDate = Question.CreatedDate,
                                   Answer = Question.Answer,
                                   QuestionContent = Question.QuestionContent,
                                   QuestionID = Question.QuestionID
                               };
            // Get comment list
            var commentList = from Comment in db.Comments
                              where Comment.ProjectID == ProjectID
                              orderby Comment.CreatedDate descending
                              select new CommentDTO()
                              {
                                  CreatedDate = Comment.CreatedDate,
                                  CommentContent = Comment.CommentContent,
                                  IsHide = Comment.IsHide,
                                  CommentID = Comment.CommentID,
                                  //UserID = Comment.UserID
                              };

            projectDTO.RewardPkgs = rewardList.ToList();
            projectDTO.UpdateLogs = updateLogList.ToList();
            projectDTO.Timelines = timelineList.ToList();
            projectDTO.Questions = questionList.ToList();
            projectDTO.Comments = commentList.ToList();

            return projectDTO;
        }

        public ProjectDetailDTO GetProjectByCode(string projectCode, string userName)
        {
            if (string.IsNullOrEmpty(projectCode))
            {
                throw new KeyNotFoundException();
            }

            // Create Project query from dB.
            var projectDetail = (from project in db.Projects
                                 where project.ProjectCode.Equals(projectCode.ToUpper())
                                 select new ProjectDetailDTO
                                 {
                                     CategoryID = project.CategoryID,
                                     CreatedDate = project.CreatedDate,
                                     Description = project.Description,
                                     Title = project.Title,
                                     ImageUrl = project.ImageUrl,
                                     Status = project.Status,
                                     SubDescription = project.SubDescription,
                                     ProjectCode = project.ProjectCode,
                                     CurrentFunded = project.CurrentFunded,
                                     ExpireDate = project.ExpireDate,
                                     Risk = project.Risk,
                                     FundingGoal = project.FundingGoal,
                                     Location = project.Location,
                                     IsExprired = project.IsExprired,
                                     VideoUrl = project.VideoUrl,
                                     CategoryName = project.Category.Name,
                                     ProjectID = project.ProjectID,
                                     Creator = new CreatorDTO
                                     {
                                         FullName = project.Creator.UserInfo.FullName,
                                         UserName = project.Creator.Username,
                                         NumberBacked = project.Creator.Backings.Count,
                                         NumberCreated = project.Creator.CreatedProjects.Count,
                                         ProfileImage = project.Creator.UserInfo.ProfileImage
                                     }
                                 }).FirstOrDefault();

            // Check project exist?
            if (projectDetail == null)
            {
                throw new KeyNotFoundException();
            }

            // Get comments.
            var commentsList = (userName != null && projectDetail.Creator.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase))
                                ? GetComments(projectDetail.ProjectID, true)
                                : GetComments(projectDetail.ProjectID, false);

            // Insert list comments into project
            projectDetail.CommentsList = commentsList;


            // Set number exprire day.
            var timespan = projectDetail.ExpireDate - DateTime.Today;
            projectDetail.NumberDays = timespan.GetValueOrDefault().Days;

            return projectDetail;
        }

        private List<CommentDTO> GetComments(int projectID, bool showHide)
        {
            var commentsQuery = from comment in db.Comments
                                where comment.ProjectID == projectID
                                orderby comment.CreatedDate descending
                                select new CommentDTO
                                {
                                    FullName = comment.User.UserInfo.FullName,
                                    ProfileImage = comment.User.UserInfo.ProfileImage,
                                    CreatedDate = comment.CreatedDate,
                                    UserName = comment.User.Username,
                                    CommentContent = comment.CommentContent,
                                    CommentID = comment.CommentID,
                                    IsHide = comment.IsHide
                                };

            var commentsList = showHide ? commentsQuery.Take(10).ToList() : commentsQuery.Where(x => !x.IsHide).Take(10).ToList();

            return commentsList;
        }

        public CommentDTO AddComment(string projectCode, CommentDTO comment)
        {
            // Check user exist.
            var user = UserRepository.Instance.GetByUserNameOrEmail(comment.UserName);
            if (user == null)
            {
                throw new UserNotFoundException();
            }

            // Get project
            Project project = null;
            if (projectCode != null)
            {
                // Check conversation exist.
                project = db.Projects.FirstOrDefault(c => c.ProjectCode.Equals(projectCode, StringComparison.OrdinalIgnoreCase));
            }
            if (project == null)
            {
                throw new KeyNotFoundException();
            }

            // Create comment.
            var newComment = new Comment
            {
                CommentContent = comment.CommentContent,
                CreatedDate = DateTime.Now,
                IsHide = false,
                UserID = user.DDL_UserID,
                ProjectID = project.ProjectID
            };


            // Add to DB.
            db.Comments.AddOrUpdate(newComment);
            db.SaveChanges();

            // Create return message data.
            var commentDTO = new CommentDTO
            {
                IsHide = newComment.IsHide,
                FullName = user.UserInfo.FullName,
                CreatedDate = newComment.CreatedDate,
                ProfileImage = user.UserInfo.ProfileImage,
                UserName = user.Username,
                CommentContent = newComment.CommentContent,
                CommentID = newComment.CommentID,
            };

            return commentDTO;
        }

        /// <summary>
        /// ShowHideComment function
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns>CommentDTO</returns>
        public CommentDTO ShowHideComment(int id, string userName)
        {

            // Get comment.
            var comment = db.Comments.FirstOrDefault(c => c.CommentID == id);
            if (comment == null)
            {
                throw new KeyNotFoundException();
            }

            // Check creator.
            if (!comment.Project.Creator.Username.Equals(userName, StringComparison.OrdinalIgnoreCase))
            {
                throw new NotPermissonException();
            }

            // Change hide status
            comment.IsHide = !comment.IsHide;

            // Save in DB
            db.SaveChanges();

            // Map DTO
            var commentDto = new CommentDTO
            {
                IsHide = comment.IsHide,
                FullName = comment.User.UserInfo.FullName,
                CreatedDate = comment.CreatedDate,
                ProfileImage = comment.User.UserInfo.ProfileImage,
                UserName = comment.User.Username,
                CommentContent = comment.CommentContent,
                CommentID = comment.CommentID,
            };

            return commentDto;
        }

        #endregion
    }
}