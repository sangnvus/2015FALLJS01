using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DDL_CapstoneProject.Helper;
using DDL_CapstoneProject.Helpers;
using DDL_CapstoneProject.Models;
using DDL_CapstoneProject.Models.DTOs;
using DDL_CapstoneProject.Ultilities;
using System.Data.Entity;
using System.Diagnostics;

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
        /// 


        //TrungVn


        public List<ProjectBasicViewDTO> GetProject(int take, int categoryid, string order)
        {
            DateTime currentDate = DateTime.Now;
            var ProjectList = from project in db.Projects
                              where !project.IsExprired && (categoryid == 0 || project.CategoryID == categoryid)
                              select new ProjectBasicViewDTO
                              {
                                  ProjectID = project.ProjectID,
                                  ProjectCode = project.ProjectCode,
                                  CreatorName = project.Creator.UserInfo.FullName,
                                  Title = project.Title,
                                  ImageUrl = project.ImageUrl,
                                  SubDescription = project.SubDescription,
                                  Location = project.Location,
                                  CurrentFunded = Decimal.Round((project.CurrentFunded / project.FundingGoal) * 100),
                                  CurrentFundedNumber = project.CurrentFunded,
                                  ExpireDate = DbFunctions.DiffDays(DateTime.Now,project.ExpireDate),
                                  FundingGoal = project.FundingGoal,
                                  Category = project.Category.Name,
                                  Backers = project.Backings.Count,
                                  CreatedDate = project.CreatedDate,
                                  PopularPoint = project.PopularPoint
                              };
            try
            {
                if (ProjectList.Any())
                {
                    return orderBy(order, ProjectList).Take(take).ToList();
                }
            }
            catch (Exception ex) { }
            return new List<ProjectBasicViewDTO>();
        }


        public List<ProjectBasicViewDTO> GetProjectHightestAndTotalFund()
        {
            var list = new List<ProjectBasicViewDTO>();
            var HeightestFund = from project in db.Projects
                                where project.IsExprired && project.CurrentFunded > project.FundingGoal
                                orderby project.CurrentFunded
                                select new ProjectBasicViewDTO
                                {
                                    CurrentFundedNumber = project.CurrentFunded
                                };
            try
            {
                if (HeightestFund.Any())
                {
                    list.Add(HeightestFund.Take(1).ToList()[0]);
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex); }
            var TotalFund = from project in db.Projects
                            select project.CurrentFunded;
            try
            {
                var pro = new ProjectBasicViewDTO();
                if (TotalFund.Any())
                {
                    pro.CurrentFundedNumber = (Convert.ToDecimal(TotalFund.Sum()));
                }
                list.Add(pro);
            }
            catch (Exception ex) { Debug.WriteLine(ex); }
            if (!list.Any())
            {
                list.Add(new ProjectBasicViewDTO());
            }
            return list;
        }

        private IQueryable<ProjectBasicViewDTO> orderBy(string order, IQueryable<ProjectBasicViewDTO> ProjectList)
        {
            if (order == "CurrentFunded")
            {
                return from project in ProjectList
                       orderby project.CurrentFundedNumber descending
                       select project;
            }
            if (order == "CreatedDate")
            {
                return from project in ProjectList
                       orderby project.CreatedDate descending
                       select project;
            }
            if (order == "PopularPoint")
            {
                return from project in ProjectList
                       orderby project.PopularPoint descending
                       select project;
            }
            if (order == "ExpireDate")
            {
                return from project in ProjectList
                       orderby project.ExpireDate
                       select project;
            }
            if (order == "FundingGoal")
            {
                return from project in ProjectList
                       orderby project.FundingGoal
                       select project;
            }
            return ProjectList;
        }
        public List<ProjectBasicViewDTO> GetProjectByCategory()
        {
            var ProjectList = new List<ProjectBasicViewDTO>();
            try
            {
                List<Category> cat = db.Categories.ToList();
                for (int i = 0; i < cat.Count(); i++)
                {
                    List<ProjectBasicViewDTO> getProject = GetProject(1, cat[i].CategoryID, "PopularPoint");
                    if (getProject.Any())
                        ProjectList.Add(getProject[0]);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return ProjectList;
        }

        public List<List<ProjectBasicViewDTO>> GetProjectStatisticList()
        {
            var ProjectList = new List<List<ProjectBasicViewDTO>>();
            ProjectList.Add(GetProject(3, 0, "PopularPoint"));
            ProjectList.Add(GetProject(3, 0, "CreatedDate"));
            ProjectList.Add(GetProject(3, 0, "CurrentFunded"));
            ProjectList.Add(GetProject(3, 0, "ExpireDate"));

            return ProjectList;
        }
        public List<List<ProjectBasicViewDTO>> GetStatisticListForHome()
        {
            var ProjectList = new List<List<ProjectBasicViewDTO>>();
            ProjectList.Add(GetProject(3, 0, "PopularPoint"));
            ProjectList.Add(GetProjectByCategory());
            ProjectList.Add(GetProject(1, 0, "CurrentFunded"));
            ProjectList.Add(GetProjectHightestAndTotalFund());
            return ProjectList;
        }
        //TrungVn

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
                                     NumberBacked = project.Backings.Count,
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

            // Get updateLog list.
            var updateLogsList = (from updateLog in db.UpdateLogs
                                  where updateLog.ProjectID == projectDetail.ProjectID
                                  orderby updateLog.CreatedDate descending
                                  select new UpdateLogDTO
                                  {
                                      CreatedDate = updateLog.CreatedDate,
                                      Description = updateLog.Description,
                                      Title = updateLog.Title,
                                      UpdateLogID = updateLog.UpdateLogID
                                  }).ToList();

            // Insert updatelog list into projectDTO
            projectDetail.UpdateLogsList = updateLogsList;

            // Set number exprire day.
            var timespan = projectDetail.ExpireDate - DateTime.Today;
            projectDetail.NumberDays = timespan.GetValueOrDefault().Days;

            return projectDetail;
        }

        #region Comment

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
                                    IsHide = comment.IsHide,
                                    IsEdited = comment.IsEdited
                                };

            //var commentsList = showHide ? commentsQuery.Take(10).ToList() : commentsQuery.Where(x => !x.IsHide).Take(10).ToList();
            var commentsList = showHide ? commentsQuery.ToList() : commentsQuery.Where(x => !x.IsHide).ToList();

            return commentsList;
        }

        public List<CommentDTO> AddComment(string projectCode, CommentDTO comment, DateTime lastCommentDateTime)
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
            var newComment = db.Comments.Create();
            newComment.CommentContent = comment.CommentContent;
            newComment.CreatedDate = DateTime.Now;
            newComment.IsHide = false;
            newComment.UserID = user.DDL_UserID;
            newComment.ProjectID = project.ProjectID;
            newComment.IsEdited = false;
            newComment.UpdatedDate = DateTime.Now;


            // Add to DB.
            db.Comments.Add(newComment);
            db.SaveChanges();

            // Get list new comment.
            var commentsQuery = from commentItem in db.Comments
                                where commentItem.Project.ProjectCode == projectCode && commentItem.CreatedDate > lastCommentDateTime
                                orderby commentItem.CreatedDate descending
                                select new CommentDTO
                                {
                                    IsHide = commentItem.IsHide,
                                    FullName = commentItem.User.UserInfo.FullName,
                                    CreatedDate = commentItem.CreatedDate,
                                    ProfileImage = commentItem.User.UserInfo.ProfileImage,
                                    UserName = commentItem.User.Username,
                                    CommentContent = commentItem.CommentContent,
                                    CommentID = commentItem.CommentID,
                                };

            var commentsList = (comment.UserName == project.Creator.Username)
                ? commentsQuery.ToList()
                : commentsQuery.Where(c => !c.IsHide).ToList();

            return commentsList;
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
                throw new NotPermissionException();
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
                IsEdited = comment.IsEdited
            };

            return commentDto;
        }

        /// <summary>
        /// EditComment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userName"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public CommentDTO EditComment(int id, string userName, string content)
        {

            // Get comment.
            var comment = db.Comments.FirstOrDefault(c => c.CommentID == id);
            if (comment == null)
            {
                throw new KeyNotFoundException();
            }

            // Check permission.
            if (!comment.User.Username.Equals(userName, StringComparison.OrdinalIgnoreCase))
            {
                throw new NotPermissionException();
            }

            // Change content
            comment.CommentContent = content;
            comment.UpdatedDate = DateTime.Now;
            comment.IsEdited = true;

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
                IsEdited = comment.IsEdited
            };

            return commentDto;
        }

        /// <summary>
        /// DeleteComment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userName"></param>
        /// <returns>boolean</returns>
        public bool DeleteComment(int id, string userName)
        {
            // Get comment.
            var comment = db.Comments.FirstOrDefault(c => c.CommentID == id);
            if (comment == null)
            {
                throw new KeyNotFoundException();
            }

            // Check creator.
            if (!comment.User.Username.Equals(userName, StringComparison.OrdinalIgnoreCase))
            {
                throw new NotPermissionException();
            }

            // Delete comment
            db.Comments.Remove(comment);

            // Save in DB
            db.SaveChanges();

            return true;
        }
        #endregion

        #endregion
    }
}