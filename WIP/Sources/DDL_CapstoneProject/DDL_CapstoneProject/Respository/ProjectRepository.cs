﻿using System;
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
        #region TrungVN

        public Dictionary<string, int> getStatisticsInfor()
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();
            //projectSuccesedCount
            int projectSuccesedCount = GetProject(0, 0, "All", "", "", "", true, "true").Count;
            //total funded
            var totalFund = from project in db.Projects
                            select project.CurrentFunded;
            //backingusercount and userbackmuchcount 
            var backingUser = from backing in db.Backings
                              group backing by backing.UserID into b
                              select b;
            int backingUserCount = backingUser.Count();
            var backingUserCount2 = (from backing in backingUser
                                     select backing.GroupBy(x => x.ProjectID)).ToList();
            List<int> UserBackmuchList = new List<int>();
            foreach (var item in backingUserCount2)
            {
                if (item.Count() >= 2)
                    UserBackmuchList.Add(item.Count());
            }
            int UserBackmuchCount = UserBackmuchList.Count();

            //numberofbacking
            int NumberOfBacking = (from backing in db.Backings
                                   select backing.BackingID).Count();

            dic.Add("SuccesedCount", projectSuccesedCount);
            dic.Add("TotalFunded", Convert.ToInt32(totalFund.Sum()));
            dic.Add("BackingUserCount", backingUserCount);
            dic.Add("UserBackmuchCount", UserBackmuchCount);
            dic.Add("NumberOfBacking", NumberOfBacking);
            return dic;
        }


        public List<ProjectBasicViewDTO> GetProjectTop(String categoryid)
        {
            categoryid = "|" + categoryid + "|";
            return GetProject(10, 0, categoryid, "CurrentFunded", "", "", true, "");
        }



        public List<ProjectBasicViewDTO> GetProject(int take, int from, String categoryidList, string order,
                                                    string pathofprojectname, string status,
                                                    bool isExprired, string isFunded)
        {
            if (pathofprojectname == null)
            {
                pathofprojectname = "";
            }
            else
            {
                if (pathofprojectname == "null")
                    pathofprojectname = "";
            }
            if (status == null) status = "";
            if (isFunded == null) isFunded = "";


            DateTime currentDate = DateTime.Now;
            var ProjectList = from project in db.Projects
                              where (categoryidList.ToLower().Contains("all") || categoryidList.Contains("|" + project.CategoryID + "|"))
                              && project.IsExprired == isExprired && project.Title.Contains(pathofprojectname)
                              && project.Status.Contains(status) && project.IsFunded.ToString().ToLower().Contains(isFunded)
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
                                  ExpireDate = DbFunctions.DiffDays(DateTime.Now, project.ExpireDate),
                                  FundingGoal = project.FundingGoal,
                                  Category = project.Category.Name,
                                  Backers = project.Backings.Count,
                                  CreatedDate = project.CreatedDate,
                                  PopularPoint = project.PopularPoint
                              };
            int leng = ProjectList.Count();
            if (take == 0)
                take = leng;
            else if (take > leng)
                take = leng;

            return orderBy(order, ProjectList).Take(take).ToList();
        }


        public List<ProjectBasicViewDTO> GetProjectHightestAndTotalFund()
        {
            var list = new List<ProjectBasicViewDTO>();
            var HeightestFund = from project in db.Projects
                                where project.IsExprired && project.IsFunded
                                orderby project.CurrentFunded
                                select new ProjectBasicViewDTO
                                {
                                    CurrentFundedNumber = project.CurrentFunded
                                };
            var TotalFund = from project in db.Projects
                            select project.CurrentFunded;
            var totalFund = new ProjectBasicViewDTO();
            totalFund.CurrentFundedNumber = (Convert.ToDecimal(TotalFund.Sum()));

            list.Add(HeightestFund.First());
            list.Add(totalFund);
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
                    List<ProjectBasicViewDTO> getProject = GetProject(1, 0, "|" + cat[i].CategoryID + "|", "PopularPoint", "", "", false, "");
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
            ProjectList.Add(GetProject(3, 0, "All", "PopularPoint", "", "", false, ""));
            ProjectList.Add(GetProject(3, 0, "All", "CreatedDate", "", "", false, ""));
            ProjectList.Add(GetProject(3, 0, "All", "CurrentFunded", "", "", false, ""));
            ProjectList.Add(GetProject(3, 0, "All", "ExpireDate", "", "", false, ""));

            return ProjectList;
        }
        public List<List<ProjectBasicViewDTO>> GetStatisticListForHome()
        {
            var ProjectList = new List<List<ProjectBasicViewDTO>>();
            ProjectList.Add(GetProject(3, 0, "All", "PopularPoint", "", "", false, ""));
            ProjectList.Add(GetProjectByCategory());
            ProjectList.Add(GetProject(1, 0, "All", "CurrentFunded", "", "", false, ""));
            ProjectList.Add(GetProjectHightestAndTotalFund());
            return ProjectList;
        }
        #endregion

        #region HuyNM
        /// <summary>
        /// Initial a empty project
        /// </summary>
        /// <returns>emptyProject</returns>
        public Project CreateEmptyProject()
        {
            // Create project 
            var project = db.Projects.Create();
            project.ProjectCode = string.Empty;
            project.CategoryID = 0;
            project.CreatorID = 0;
            project.Title = string.Empty;
            project.CreatedDate = DateTime.Today;
            project.Risk = string.Empty;
            project.ImageUrl = string.Empty;
            project.SubDescription = string.Empty;
            project.Location = string.Empty;
            project.IsExprired = false;
            project.CurrentFunded = 0;
            project.FundingGoal = 0;
            project.Description = string.Empty;
            project.VideoUrl = string.Empty;
            project.PopularPoint = 0;
            project.Status = DDLConstants.ProjectStatus.DRAFT;

            return project;
        }

        /// <summary>
        /// Create a new project
        /// </summary>
        /// <returns>project</returns>
        public string CreatProject(ProjectCreateDTO newProject)
        {
            var project = CreateEmptyProject();
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

            return project.ProjectCode;
        }

        /// <summary>
        /// Edit a project
        /// </summary>
        /// <param name="project">object</param>
        /// <returns>updateProject</returns>
        public ProjectEditDTO EditProjectBasic(ProjectEditDTO project, string uploadImageName)
        {
            var updateProject = db.Projects.SingleOrDefault(u => u.ProjectID == project.ProjectID);

            if (updateProject == null)
            {
                throw new KeyNotFoundException();
            }

            if (uploadImageName != string.Empty)
            {
                updateProject.ImageUrl = uploadImageName;
            }

            updateProject.CategoryID = project.CategoryID;
            updateProject.ExpireDate = project.ExpireDate;
            updateProject.FundingGoal = project.FundingGoal;
            updateProject.Location = project.Location;
            updateProject.Status = project.Status;
            updateProject.SubDescription = project.SubDescription;
            updateProject.Title = project.Title;
            updateProject.UpdatedDate = DateTime.Today;

            db.SaveChanges();

            var updateProjectDTO = new ProjectEditDTO
            {
                CategoryID = updateProject.CategoryID,
                ExpireDate = updateProject.ExpireDate,
                FundingGoal = updateProject.FundingGoal,
                Location = updateProject.Location,
                Status = updateProject.Status,
                SubDescription = updateProject.SubDescription,
                Title = updateProject.Title,
                ProjectID = updateProject.ProjectID,
                ProjectCode = updateProject.ProjectCode,
                ImageUrl = updateProject.ImageUrl,
                CurrentFunded = project.CurrentFunded,
            };

            // Set number exprire day.
            var timespan = updateProjectDTO.ExpireDate - DateTime.Today;
            updateProjectDTO.NumberDays = timespan.GetValueOrDefault().Days;

            return updateProjectDTO;
        }

        /// <summary>
        /// Edit project's story
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public ProjectStoryDTO EditProjectStory(ProjectStoryDTO project)
        {
            var updateProject = db.Projects.SingleOrDefault(u => u.ProjectID == project.ProjectID);

            if (updateProject == null)
            {
                throw new KeyNotFoundException();
            }

            updateProject.Risk = project.Risk;
            updateProject.VideoUrl = project.VideoUrl;
            updateProject.Description = project.Description;

            db.SaveChanges();

            var updateProjectDTO = new ProjectStoryDTO
            {
                Risk = updateProject.Risk,
                VideoUrl = updateProject.VideoUrl,
                Description = updateProject.Description,
                ProjectID = updateProject.ProjectID
            };

            return updateProjectDTO;
        }

        /// <summary>
        /// Get a project
        /// </summary>
        /// <param name="ProjectID">int</param>
        /// <returns>project</returns>
        public ProjectEditDTO GetProjectBasic(string code, string UserName)
        {
            var project = db.Projects.SingleOrDefault(x => x.ProjectCode == code);

            if (project == null)
            {
                throw new KeyNotFoundException();
            }

            var user = db.DDL_Users.SingleOrDefault(x => x.Username == UserName);

            if (project.CreatorID != user.DDL_UserID)
            {
                throw new NotPermissionException();
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

            // Set number exprire day.
            var timespan = projectDTO.ExpireDate - DateTime.Today;
            projectDTO.NumberDays = timespan.GetValueOrDefault().Days;

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

        public List<string> SubmitProject(ProjectEditDTO submitProject)
        {
            var project = db.Projects.SingleOrDefault(u => u.ProjectID == submitProject.ProjectID);

            if (project == null)
            {
                throw new KeyNotFoundException();
            }

            List<string> mylist = new List<string>();

            string messageBasic = string.Empty;
            if (string.IsNullOrEmpty(project.Title) || string.IsNullOrEmpty(project.ImageUrl) || string.IsNullOrEmpty(project.SubDescription)
                || string.IsNullOrEmpty(project.Location) || project.ExpireDate == null || project.FundingGoal <= 0)
            {
                messageBasic = "Xin hãy xem lại trang thông tin cơ bản, các trường (kể cả ảnh dự án) PHẢI được điền đầy đủ và hợp lệ";
                mylist.Add(messageBasic);

            }

            var rewardList = RewardPkgRepository.Instance.GetRewardPkg(project.ProjectID);
            string messageReward = string.Empty;
            if (rewardList.Any(reward => reward.PledgeAmount <= 0 || string.IsNullOrEmpty(reward.Description)
                                         || reward.EstimatedDelivery < project.ExpireDate))
            {
                messageReward = "Xin hãy xem lại trang gói quà! Tất cả các trường PHẢI được điền đầy đủ và hợp lệ(Ngày giao phải sau ngày đóng gây quỹ)";
                mylist.Add(messageReward);
            }

            string messageStory = string.Empty;
            if (string.IsNullOrEmpty(project.Description) || string.IsNullOrEmpty(project.Risk))
            {
                messageStory = "Xin hãy xem lại trang câu chuyện! Các trường PHẢI được nhập đầy đủ (trừ video)";
                mylist.Add(messageStory);
            }

            if (string.IsNullOrEmpty(messageBasic) && string.IsNullOrEmpty(messageReward) && string.IsNullOrEmpty(messageStory))
            {
                project.Status = DDLConstants.ProjectStatus.PENDING;

                db.SaveChanges();
            }

            return mylist;
        }

        public string BackProject(ProjectBackDTO backingData)
        {
            var project = db.Projects.SingleOrDefault(x => x.ProjectCode == backingData.ProjectCode);

            if (project == null)
            {
                throw new KeyNotFoundException();
            }

            var user = db.DDL_Users.SingleOrDefault((x => x.Email == backingData.Email));
            if (user == null)
            {
                throw new KeyNotFoundException();
            }

            // Create new backing record
            var backing = db.Backings.Create();
            backing.UserID = user.DDL_UserID;
            backing.ProjectID = project.ProjectID;
            backing.BackedDate = DateTime.Today;
            backing.IsPublic = backingData.IsPublic;

            // Create new backingDetail recored
            var backingDetail = db.BackingDetails.Create();
            backingDetail.RewardPkgID = backingData.RewardPkgID;
            backingDetail.PledgedAmount = backingData.PledgeAmount;
            backingDetail.Quantity = backingData.Quantity;
            backingDetail.Description = backingData.Description;
            backingDetail.Address = backingData.Address;
            backingDetail.Email = backingData.Email;
            backingDetail.PhoneNumber = backingData.PhoneNumber;

            // Add backingDetail to backing
            backing.BackingDetail = backingDetail;

            db.Backings.Add(backing);

            // Caculate project current fund
            project.CurrentFunded += backingDetail.PledgedAmount;

            db.SaveChanges();

            return project.ProjectCode;
        }
        #endregion       

        public ProjectDetailDTO GetProjectByCode(string projectCode, string userName)
        {
            if (string.IsNullOrEmpty(projectCode))
            {
                throw new KeyNotFoundException();
            }
            var userCurrent = db.DDL_Users.FirstOrDefault(x => x.Username == userName);
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
                                     NumberComment = project.Creator.Username == userName ?project.Comments.Count: project.Comments.Count(x => !x.IsHide),
                                     NumberUpdate = project.UpdateLogs.Count,
                                     Creator = new CreatorDTO
                                     {
                                         FullName = project.Creator.UserInfo.FullName,
                                         UserName = project.Creator.Username,
                                         NumberBacked = project.Creator.Backings.Count,
                                         NumberCreated = project.Creator.CreatedProjects.Count(x => x.Status != DDLConstants.ProjectStatus.DRAFT
                                             && x.Status != DDLConstants.ProjectStatus.REJECTED
                                             && x.Status != DDLConstants.ProjectStatus.PENDING),
                                         ProfileImage = project.Creator.UserInfo.ProfileImage
                                     },
                                     Reminded = project.Reminds.Count(x => x.ProjectID == project.ProjectID && x.UserID == userCurrent.DDL_UserID),
                                 }).FirstOrDefault();
            List<RewardPkg> rewardDetail = db.RewardPkgs.Where(x => x.Project.ProjectCode == projectCode).ToList();
            List<RewardPkgDTO> RewardDTO = new List<RewardPkgDTO>();
            foreach (RewardPkg reward in rewardDetail)
            {
                RewardPkgDTO Reward = new RewardPkgDTO
                {
                    Backers = reward.BackingDetails.Count(),
                    Description = reward.Description,
                    EstimatedDelivery = CommonUtils.ConvertDateTimeFromUtc(reward.EstimatedDelivery.GetValueOrDefault()),
                    PledgeAmount = reward.PledgeAmount,
                    Quantity = reward.Quantity
                };
                RewardDTO.Add(Reward);
            }
            projectDetail.RewardDetail = RewardDTO;
            // Check project exist?
            if (projectDetail == null)
            {
                throw new KeyNotFoundException();
            }

            // Convert datetime to gmt+7
            projectDetail.CreatedDate = CommonUtils.ConvertDateTimeFromUtc(projectDetail.CreatedDate);
            projectDetail.ExpireDate = CommonUtils.ConvertDateTimeFromUtc(projectDetail.ExpireDate.GetValueOrDefault());

            // Set number exprire day.
            var timespan = projectDetail.ExpireDate - CommonUtils.DateTodayGMT7();
            projectDetail.NumberDays = timespan.GetValueOrDefault().Days;

            return projectDetail;
        }

        public List<CommentDTO> GetListComment(string projectCode, DateTime? lastDateTime, string userName)
        {
            var project = db.Projects.FirstOrDefault(x => x.ProjectCode == projectCode);
            if (project == null)
            {
                throw new KeyNotFoundException();
            }
            // Get comments.
            var commentsList = (userName != null && project.Creator.Username.Equals(userName, StringComparison.OrdinalIgnoreCase))
                                ? GetComments(project.ProjectID, true, lastDateTime)
                                : GetComments(project.ProjectID, false, lastDateTime);

            return commentsList;
        }

        public List<UpdateLogDTO> GetListUpdateLog(string projectCode, string userName)
        {
            var project = db.Projects.FirstOrDefault(x => x.ProjectCode == projectCode);
            if (project == null)
            {
                throw new KeyNotFoundException();
            }

            // Get updateLog list.
            var updateLogsList = (from updateLog in db.UpdateLogs
                                  where updateLog.ProjectID == project.ProjectID
                                  orderby updateLog.CreatedDate descending
                                  select new UpdateLogDTO
                                  {
                                      CreatedDate = updateLog.CreatedDate,
                                      Description = updateLog.Description,
                                      Title = updateLog.Title,
                                      UpdateLogID = updateLog.UpdateLogID
                                  }).ToList();
            updateLogsList.ForEach(x => x.CreatedDate = CommonUtils.ConvertDateTimeFromUtc(x.CreatedDate));
            return updateLogsList;
        }


        // 17/10/2015 - MaiCTP - Get BackedProject
        public List<ProjectBasicViewDTO> GetBackedProject(String userName)
        {
          
            var Project = (from backing in db.Backings
                           from project in db.Projects
                           where backing.User.Username == userName  && project.ProjectID == backing.ProjectID
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
                                ExpireDate = DbFunctions.DiffDays(DateTime.Now, project.ExpireDate),
                                FundingGoal = project.FundingGoal,
                                Category = project.Category.Name,
                                Backers = project.Backings.Count,
                                CreatedDate = project.CreatedDate,
                                PopularPoint = project.PopularPoint
                            }).Distinct().ToList();


            return Project;

        }


        //18/10/2015 - MaiCTP - Get StarredProject
        public List<ProjectBasicViewDTO> GetStarredProject(String userName)
        {
            var Project = (from remind in db.Reminds
                           from project in db.Projects
                           where remind.User.Username == userName && project.ProjectID == remind.ProjectID
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
                               ExpireDate = DbFunctions.DiffDays(DateTime.Now, project.ExpireDate),
                               FundingGoal = project.FundingGoal,
                               Category = project.Category.Name,
                               Backers = project.Backings.Count,
                               CreatedDate = project.CreatedDate,
                               PopularPoint = project.PopularPoint
                           }).Distinct().ToList();


            return Project;
        }


        // 18/10/2015 - MaiCTP - Get CreatedProject
        public List<ProjectBasicViewDTO> GetCreatedProject(String userName)
        {
            List<ProjectBasicViewDTO> ProjectList = new List<ProjectBasicViewDTO>();

            var projectList = (from project in db.Projects
                               where project.Creator.Username.Equals(userName, StringComparison.OrdinalIgnoreCase)
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
                                   ExpireDate = DbFunctions.DiffDays(DateTime.Now, project.ExpireDate),
                                   FundingGoal = project.FundingGoal,
                                   Category = project.Category.Name,
                                   Backers = project.Backings.Count,
                                   CreatedDate = project.CreatedDate,
                                   PopularPoint = project.PopularPoint
                               }).Distinct();




            return projectList.ToList();

        }

        public void RemindProject(string userName, string projectCode)
        {
            var userCurrent = db.DDL_Users.FirstOrDefault(x => x.Username.Equals(userName));
            var project = db.Projects.FirstOrDefault(x => x.ProjectCode == projectCode);
            var reminded = db.Reminds.FirstOrDefault(x => x.UserID.Equals(userCurrent.DDL_UserID) && x.ProjectID == project.ProjectID);
            if (reminded != null)
            {
                db.Reminds.Remove(reminded);
                db.SaveChanges();
            }
            else
            {
                reminded = new Remind
                {
                    Project = project,
                    User = userCurrent,
                    RemindID = 0,
                    ProjectID = 0,
                    UserID = 0
                };
                reminded.Project.ProjectID = project.ProjectID;
                reminded.User.DDL_UserID = userCurrent.DDL_UserID;
                db.Reminds.Add(reminded);
                db.SaveChanges();
            }
        }

        public void ReportProject(string userName, string projectCode, string content)
        {
            var userCurrent = db.DDL_Users.FirstOrDefault(x => x.Username.Equals(userName));
            var project = db.Projects.FirstOrDefault(x => x.ProjectCode == projectCode);
            var report = new ReportProject
            {
                Project = project,
                Reporter = userCurrent,
                ProjectID = project.ProjectID,
                ReportContent = content,
                ReportedDate = DateTime.Now,
                ReporterID = userCurrent.DDL_UserID,
                Status = "unread",
                Subject = "Report " + project.Title
            };
            db.ReportProjects.Add(report);
            db.SaveChanges();

        }

        public List<BackingDTO> GetListBacker(string projectCode)
        {
            var project = db.Projects.FirstOrDefault(x => x.ProjectCode == projectCode);
            var remindInfo = db.Reminds.FirstOrDefault(x => x.ProjectID == project.ProjectID);
            decimal a = db.BackingDetails.FirstOrDefault(x => x.BackingID == 4).PledgedAmount;

            var list = new List<BackingDTO>();
            var listBacker = from backer in db.Backings
                             where project.ProjectID == backer.ProjectID
                             select new BackingDTO
                             {
                                 Amount = backer.BackingDetail.PledgedAmount,
                                 Date = backer.BackedDate,
                                 FullName = backer.User.UserInfo.FullName,
                             };
            foreach (BackingDTO backer in listBacker)
            {
                backer.Date = CommonUtils.ConvertDateTimeFromUtc(backer.Date.GetValueOrDefault());
                list.Add(backer);
            }
            return list;
        }

        #region Comment

        private List<CommentDTO> GetComments(int projectID, bool showHide, DateTime? lastDateTime)
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

            if (!showHide)
            {
                commentsQuery = commentsQuery.Where(x => !x.IsHide);
            }

            var lastDateTimeUtc = CommonUtils.ConvertDateTimeToUtc(lastDateTime.GetValueOrDefault());

            if (lastDateTime != null)
            {
                commentsQuery = commentsQuery.Where(x => x.CreatedDate < lastDateTimeUtc);
            }

            //var commentsList = showHide ? commentsQuery.Take(10).ToList() : commentsQuery.Where(x => !x.IsHide).Take(10).ToList();
            var commentsList = commentsQuery.Take(10).ToList();

            commentsList.ForEach(x => x.CreatedDate = CommonUtils.ConvertDateTimeFromUtc(x.CreatedDate));

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
            newComment.CreatedDate = DateTime.UtcNow;
            newComment.IsHide = false;
            newComment.UserID = user.DDL_UserID;
            newComment.ProjectID = project.ProjectID;
            newComment.IsEdited = false;
            newComment.UpdatedDate = DateTime.UtcNow;


            // Add to DB.
            db.Comments.Add(newComment);
            db.SaveChanges();

            var lastDatimeTimeUtc = CommonUtils.ConvertDateTimeToUtc(lastCommentDateTime);

            // Get list new comment.
            var commentsQuery = from commentItem in db.Comments
                                where commentItem.Project.ProjectCode == projectCode && commentItem.CreatedDate > lastDatimeTimeUtc
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

            commentsList.ForEach(x => x.CreatedDate = CommonUtils.ConvertDateTimeFromUtc(x.CreatedDate));

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
                CreatedDate = CommonUtils.ConvertDateTimeFromUtc(comment.CreatedDate),
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
            comment.UpdatedDate = DateTime.UtcNow;
            comment.IsEdited = true;

            // Save in DB
            db.SaveChanges();

            // Map DTO
            var commentDto = new CommentDTO
            {
                IsHide = comment.IsHide,
                FullName = comment.User.UserInfo.FullName,
                CreatedDate = CommonUtils.ConvertDateTimeFromUtc(comment.CreatedDate),
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