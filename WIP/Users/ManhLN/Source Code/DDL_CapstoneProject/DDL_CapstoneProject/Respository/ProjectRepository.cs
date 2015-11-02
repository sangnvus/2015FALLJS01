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
using System.Data.Entity.SqlServer;
using System.Diagnostics;

namespace DDL_CapstoneProject.Respository
{
    public class ProjectRepository : SingletonBase<ProjectRepository>
    {

        #region "Constructors"
        private ProjectRepository()
        {
        }
        #endregion

        #region "Methods"


        #region TrungVN

        public Dictionary<string, int> getStatisticsInfor()
        {
            using (var db = new DDLDataContext())
            {
                Dictionary<string, int> dic = new Dictionary<string, int>();
                //projectSuccesedCount
                int projectSuccesedCount = GetProject(0, 0, "All", "", "", "", true, "true").Count;
                //total funded
                var totalFund = from project in db.Projects
                                select project.CurrentFunded;
                //backingusercount and userbackmuchcount 
                var backingUser = from backing in db.Backings
                                  group backing by backing.UserID
                                      into b
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
        }


        public List<ProjectBasicViewDTO> GetProjectTop(String categoryid)
        {
            categoryid = "|" + categoryid + "|";
            return GetProject(12, 0, categoryid, "CurrentFunded", "", "", true, "");
        }

        public int SearchCount(string categoryidlist, string searchkey)
        {
            int searchcount = GetProject(0, 0, categoryidlist, "", searchkey, "", false, "").Count;
            return searchcount;
        }


        public List<ProjectBasicViewDTO> GetProject(int take, int from, String categoryidList, string order,
                                                    string pathofprojectname, string status,
                                                    bool isExprired, string isFunded)
        {
            using (var db = new DDLDataContext())
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


                var ProjectList = from project in db.Projects
                                  where
                                      (categoryidList.ToLower().Contains("all") ||
                                       categoryidList.Contains("|" + project.CategoryID + "|"))
                                      && project.IsExprired == isExprired && project.Title.Contains(pathofprojectname)
                                      && project.Status.Contains(status) && project.IsFunded.ToString().ToLower().Contains(isFunded)
                                      && !project.Status.Equals(DDLConstants.ProjectStatus.DRAFT) && !project.Status.Equals(DDLConstants.ProjectStatus.REJECTED)
                                      && !project.Status.Equals(DDLConstants.ProjectStatus.SUSPENDED) && !project.Status.Equals(DDLConstants.ProjectStatus.PENDING)
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
                                      ExpireDate = DbFunctions.DiffDays(DateTime.UtcNow, project.ExpireDate),
                                      FundingGoal = project.FundingGoal,
                                      Category = project.Category.Name,
                                      CategoryID = project.CategoryID,
                                      Backers = project.Backings.Count,
                                      CreatedDate = project.CreatedDate,
                                      PopularPoint = project.PopularPoint
                                  };

                if (from >= ProjectList.Count())
                {
                    return new List<ProjectBasicViewDTO>();
                }
                if (take == 0) take = ProjectList.Count();
                var listProject = orderBy(order, ProjectList).Take(take + from).ToList();
                listProject.RemoveRange(0, from);
                // Convert datetime to GMT+7
                listProject.ForEach(x => x.CreatedDate = CommonUtils.ConvertDateTimeFromUtc(x.CreatedDate));
                return listProject;
            }
        }



        private IQueryable<ProjectBasicViewDTO> orderBy(string order, IQueryable<ProjectBasicViewDTO> ProjectList)
        {
            using (var db = new DDLDataContext())
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
        }
        public List<ProjectBasicViewDTO> GetProjectByCategory()
        {
            using (var db = new DDLDataContext())
            {
                var ProjectList = new List<ProjectBasicViewDTO>();
                try
                {
                    List<Category> cat = db.Categories.ToList();
                    for (int i = 0; i < cat.Count(); i++)
                    {
                        List<ProjectBasicViewDTO> getProject = GetProject(1, 0, "|" + cat[i].CategoryID + "|",
                            "PopularPoint", "", "", false, "");
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
        }


        public List<List<ProjectBasicViewDTO>> GetProjectStatisticList()
        {
            var ProjectList = new List<List<ProjectBasicViewDTO>>();
            ProjectList.Add(GetProject(4, 0, "All", "PopularPoint", "", "", false, ""));
            ProjectList.Add(GetProject(4, 0, "All", "CreatedDate", "", "", false, ""));
            ProjectList.Add(GetProject(4, 0, "All", "CurrentFunded", "", "", false, ""));
            ProjectList.Add(GetProject(4, 0, "All", "ExpireDate", "", "", false, ""));

            return ProjectList;
        }
        public Dictionary<string, List<ProjectBasicViewDTO>> GetStatisticListForHome()
        {
            var ProjectList = new Dictionary<string, List<ProjectBasicViewDTO>>();
            ProjectList.Add("popularproject", GetProject(4, 0, "All", "PopularPoint", "", "", false, ""));
            ProjectList.Add("projectByCategory", GetProjectByCategory());
            ProjectList.Add("highestprojectpledge", GetProject(1, 0, "All", "CurrentFunded", "", "", false, ""));
            ProjectList.Add("highestprojectfund", GetProject(1, 0, "All", "CurrentFunded", "", "", true, "true"));
            ProjectList.Add("totalprojectfund", GetTotalFund());
            return ProjectList;
        }

        public List<ProjectBasicViewDTO> GetTotalFund()
        {
            using (var db = new DDLDataContext())
            {
                var list = new List<ProjectBasicViewDTO>();
                var TotalFund = from project in db.Projects
                                select project.CurrentFunded;
                var totalFund = new ProjectBasicViewDTO();
                totalFund.CurrentFundedNumber = (Convert.ToDecimal(TotalFund.Sum()));

                list.Add(totalFund);
                return list;
            }
        }
        #endregion

        #region HuyNM
        /// <summary>
        /// Initial a empty project
        /// </summary>
        /// <returns>emptyProject</returns>
        public Project CreateEmptyProject()
        {
            using (var db = new DDLDataContext())
            {
                // Create project 
                var project = db.Projects.Create();
                project.ProjectCode = string.Empty;
                project.CategoryID = 0;
                project.CreatorID = 0;
                project.Title = string.Empty;
                project.CreatedDate = DateTime.UtcNow;
                project.UpdatedDate = DateTime.UtcNow;
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
        }

        /// <summary>
        /// Create a new project
        /// </summary>
        /// <returns>project</returns>
        public string CreatProject(ProjectCreateDTO newProject, string username)
        {
            using (var db = new DDLDataContext())
            {
                var user = db.DDL_Users.SingleOrDefault(x => x.Username == username);
                if (user == null)
                {
                    throw new KeyNotFoundException();
                }

                var project = CreateEmptyProject();
                project.CreatorID = newProject.CategoryID;
                project.Title = newProject.Title;
                project.CategoryID = newProject.CategoryID;
                project.CreatorID = user.DDL_UserID;
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
        }

        /// <summary>
        /// Edit a project
        /// </summary>
        /// <param name="project">object</param>
        /// <returns>updateProject</returns>
        public ProjectEditDTO EditProjectBasic(ProjectEditDTO project, string uploadImageName)
        {
            using (var db = new DDLDataContext())
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
                updateProject.UpdatedDate = DateTime.UtcNow;

                if (updateProject.ExpireDate != null)
                {
                    updateProject.ExpireDate =
                        CommonUtils.ConvertDateTimeToUtc(updateProject.ExpireDate.GetValueOrDefault());
                }

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

                updateProjectDTO.ExpireDate =
                    CommonUtils.ConvertDateTimeFromUtc(updateProjectDTO.ExpireDate.GetValueOrDefault());

                // Set number exprire day.
                var timespan = updateProjectDTO.ExpireDate - CommonUtils.DateTodayGMT7();
                updateProjectDTO.NumberDays = timespan.GetValueOrDefault().Days;

                return updateProjectDTO;
            }
        }

        /// <summary>
        /// Edit project's story
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public ProjectStoryDTO EditProjectStory(ProjectStoryDTO project)
        {
            using (var db = new DDLDataContext())
            {
                var updateProject = db.Projects.SingleOrDefault(u => u.ProjectID == project.ProjectID);

                if (updateProject == null)
                {
                    throw new KeyNotFoundException();
                }

                updateProject.Risk = project.Risk;
                updateProject.VideoUrl = project.VideoUrl;
                updateProject.Description = project.Description;
                updateProject.UpdatedDate = DateTime.UtcNow;

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
        }

        /// <summary>
        /// Get a project
        /// </summary>
        /// <param name="ProjectID">int</param>
        /// <returns>project</returns>
        public ProjectEditDTO GetProjectBasic(string code, string UserName)
        {
            using (var db = new DDLDataContext())
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

                if (projectDTO.ExpireDate != null)
                {
                    projectDTO.ExpireDate = CommonUtils.ConvertDateTimeFromUtc(projectDTO.ExpireDate.GetValueOrDefault());
                }

                // Set number exprire day.
                var timespan = projectDTO.ExpireDate - CommonUtils.DateTodayGMT7();
                projectDTO.NumberDays = timespan.GetValueOrDefault().Days;

                return projectDTO;
            }
        }

        public ProjectStoryDTO GetProjectStory(int ProjectID)
        {
            using (var db = new DDLDataContext())
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
        }

        /// <summary>
        /// Submit a project
        /// </summary>
        /// <param name="submitProject"></param>
        /// <returns>errorList</returns>
        public List<string> SubmitProject(ProjectEditDTO submitProject)
        {
            using (var db = new DDLDataContext())
            {
                var project = db.Projects.SingleOrDefault(u => u.ProjectID == submitProject.ProjectID);

                if (project == null)
                {
                    throw new KeyNotFoundException();
                }

                if (project.ExpireDate != null)
                {
                    project.ExpireDate = CommonUtils.ConvertDateTimeFromUtc(project.ExpireDate.GetValueOrDefault());
                }

                List<string> mylist = new List<string>();

                var expireDateLaw = DateTime.UtcNow.AddDays(10);
                expireDateLaw = CommonUtils.ConvertDateTimeFromUtc(expireDateLaw);

                string messageBasic = string.Empty;
                if (string.IsNullOrEmpty(project.Title) || project.Title.Length < 10 || project.Title.Length > 60
                    || string.IsNullOrEmpty(project.ImageUrl)
                    || string.IsNullOrEmpty(project.SubDescription) || project.SubDescription.Length < 30 || project.SubDescription.Length > 135
                    || string.IsNullOrEmpty(project.Location)
                    || project.ExpireDate == null || project.ExpireDate < expireDateLaw
                    || project.FundingGoal < 1000000)
                {
                    messageBasic = "Xin hãy xem lại trang thông tin cơ bản, các trường (kể cả ảnh dự án) PHẢI được điền đầy đủ và hợp lệ";
                    mylist.Add(messageBasic);

                }

                var rewardList = RewardPkgRepository.Instance.GetRewardPkg(project.ProjectID);
                rewardList.ForEach(x => x.EstimatedDelivery = CommonUtils.ConvertDateTimeFromUtc(x.EstimatedDelivery.GetValueOrDefault()));
                string messageReward = string.Empty;

                if (rewardList.Count == 0)
                {
                    messageReward = "Xin hãy xem lại trang gói quà! Cần ít nhất 1 gói quà!";
                    mylist.Add(messageReward);
                }

                if (rewardList.Any(reward => reward.PledgeAmount < 1000
                    || string.IsNullOrEmpty(reward.Description) || reward.Description.Length < 10 || reward.Description.Length > 135
                    || (reward.EstimatedDelivery < project.ExpireDate && reward.Type != DDLConstants.RewardType.NO_REWARD)
                    || (reward.Type != DDLConstants.RewardType.NO_REWARD && reward.Quantity < 1)
                    ))
                {
                    messageReward = "Xin hãy xem lại trang gói quà! Tất cả các trường PHẢI được điền đầy đủ và hợp lệ(Ngày giao phải sau ngày đóng gây quỹ)";
                    mylist.Add(messageReward);
                }

                string messageStory = string.Empty;
                if (string.IsNullOrEmpty(project.Description) || project.Description.Length < 135
                    || string.IsNullOrEmpty(project.Risk) || project.Risk.Length < 135
                    )
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
        }

        /// <summary>
        /// Get bacsic information of project
        /// </summary>
        /// <param name="code"></param>
        /// <returns>projectInforDTO</returns>
        public ProjectInfoBackDTO GetBackProjectInfo(string code)
        {
            using (var db = new DDLDataContext())
            {
                var project = db.Projects.SingleOrDefault(x => x.ProjectCode == code);

                if (project == null)
                {
                    throw new KeyNotFoundException();
                }

                var user = db.DDL_Users.SingleOrDefault(x => x.DDL_UserID == project.CreatorID);

                if (user == null)
                {
                    throw new NotPermissionException();
                }

                var projectInforDTO = new ProjectInfoBackDTO
                {
                    ProjectCode = project.ProjectCode,
                    Title = project.Title,
                    Creator = user.UserInfo.FullName
                };

                return projectInforDTO;
            }
        }

        /// <summary>
        /// Back a project
        /// </summary>
        /// <param name="backingData"></param>
        /// <returns>projectCode</returns>
        public string BackProject(ProjectBackDTO backingData)
        {
            using (var db = new DDLDataContext())
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
                backing.BackedDate = DateTime.UtcNow;
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

                // Caculate reward quantity
                var rewardPkg = db.RewardPkgs.SingleOrDefault(u => u.RewardPkgID == backingDetail.RewardPkgID);
                if (rewardPkg == null)
                {
                    throw new KeyNotFoundException();
                }

                rewardPkg.CurrentQuantity += backingDetail.Quantity;

                // Set project is funded
                if ((project.CurrentFunded >= project.FundingGoal) && project.IsFunded == false)
                {
                    project.IsFunded = true;
                }

                db.SaveChanges();

                return project.ProjectCode;
            }
        }


        #region Admin

        /// <summary>
        /// Get basic statistic of project for admin
        /// </summary>
        /// <returns>AdminProjectInfoDTO</returns>
        public AdminProjectGeneralInfoDTO AdminProjectGeneralInfo()
        {
            using (var db = new DDLDataContext())
            {
                var pending = db.Projects.Count(x => x.Status == DDLConstants.ProjectStatus.PENDING);
                var approved = db.Projects.Count(x => x.Status == DDLConstants.ProjectStatus.APPROVED);
                var suspended = db.Projects.Count(x => x.Status == DDLConstants.ProjectStatus.SUSPENDED);
                var funded = db.Projects.Count(x => x.IsFunded == true);
                var total = db.Projects.Count(x => x.Status != DDLConstants.ProjectStatus.DRAFT);
                var expired = db.Projects.Count(x => x.Status == DDLConstants.ProjectStatus.EXPIRED);

                var AdminProjectInfoDTO = new AdminProjectGeneralInfoDTO
                {
                    ApprovedProject = approved,
                    ExpriredProject = expired,
                    SucceedProject = funded,
                    PendingProject = pending,
                    SuspendedProject = suspended,
                    TotalProject = total
                };

                return AdminProjectInfoDTO;
            }
        }

        /// <summary>
        /// Get pending project list
        /// </summary>
        /// <returns>pendingList</returns>
        public List<ProjectBasicListDTO> GetPendingProjectList()
        {
            using (var db = new DDLDataContext())
            {
                // Get rewardPkg list
                var pendingList = (from Project in db.Projects
                                   where Project.Status == DDLConstants.ProjectStatus.PENDING || Project.Status == DDLConstants.ProjectStatus.REJECTED
                                   orderby Project.CreatedDate descending
                                   select new ProjectBasicListDTO
                                   {
                                       ProjectCode = Project.ProjectCode,
                                       ProjectID = Project.ProjectID,
                                       Title = Project.Title,
                                       Category = Project.Category.Name,
                                       CreatorEmail = Project.Creator.Email,
                                       FundingGoal = Project.FundingGoal,
                                       ExpireDate = Project.ExpireDate,
                                       Status = Project.Status,
                                       CurrentFunded = Project.CurrentFunded
                                   }).ToList();

                pendingList.ForEach(x => x.ExpireDate = CommonUtils.ConvertDateTimeFromUtc(x.ExpireDate.GetValueOrDefault()));

                return pendingList;
            }
        }

        /// <summary>
        /// Get project list
        /// </summary>
        /// <returns></returns>
        public List<ProjectBasicListDTO> GetProjectList()
        {
            using (var db = new DDLDataContext())
            {
                // Get rewardPkg list
                var projectList = (from Project in db.Projects
                                   where Project.Status != DDLConstants.ProjectStatus.DRAFT
                                   orderby Project.CreatedDate ascending
                                   select new ProjectBasicListDTO
                                   {
                                       ProjectCode = Project.ProjectCode,
                                       ProjectID = Project.ProjectID,
                                       Title = Project.Title,
                                       Category = Project.Category.Name,
                                       CreatorEmail = Project.Creator.Email,
                                       FundingGoal = Project.FundingGoal,
                                       ExpireDate = Project.ExpireDate,
                                       Status = Project.Status,
                                       CurrentFunded = Project.CurrentFunded,
                                       TotalBacking = Project.Backings.Count
                                   }).ToList();

                projectList.ForEach(x => x.ExpireDate = CommonUtils.ConvertDateTimeFromUtc(x.ExpireDate.GetValueOrDefault()));

                return projectList;
            }
        }

        /// <summary>
        /// Change project status by admin
        /// </summary>
        /// <param name="project"></param>
        /// <returns>bool</returns>
        public bool AdminChangeProjectStatus(ProjectEditDTO project)
        {
            using (var db = new DDLDataContext())
            {
                var updateProject = db.Projects.SingleOrDefault(x => x.ProjectID == project.ProjectID);

                if (updateProject == null)
                {
                    throw new KeyNotFoundException();
                }

                updateProject.Status = project.Status;

                db.SaveChanges();

                return true;
            }
        }

        /// <summary>
        /// Get general information for admin dashboard
        /// </summary>
        /// <returns>AdminDashboardInfoDTO</returns>
        public AdminDashboardInfoDTO AdminDashboardInfo()
        {
            using (var db = new DDLDataContext())
            {
                // Count user
                var totalUser = db.DDL_Users.Count();
                // Count project
                var totalProject = db.Projects.Count(x => x.Status != DDLConstants.ProjectStatus.DRAFT);

                // Caculate total funded
                var pledge = db.BackingDetails.GroupBy(o => o.BackingID)
                   .Select(g => new { BackingID = g.Key, total = g.Sum(i => i.PledgedAmount) });
                decimal totalFunded = 0;

                foreach (var group in pledge)
                {
                    totalFunded += group.total;
                }

                // Caculate total profit
                var succeedProject = (from Project in db.Projects
                                      where Project.IsFunded && Project.IsExprired
                                      select Project).ToList();

                decimal totalProfit = 0;

                foreach (var project in succeedProject)
                {
                    if (project.FundingGoal <= 50000000)
                    {
                        totalProfit += 5 * project.CurrentFunded / 100;
                    }
                    else if (project.FundingGoal <= 100000000 && project.FundingGoal > 50000000)
                    {
                        totalProfit += 4 * project.CurrentFunded / 100;
                    }
                    else if (project.FundingGoal <= 500000000 && project.FundingGoal > 100000000)
                    {
                        totalProfit += 3 * project.CurrentFunded / 100;
                    }
                    else
                    {
                        totalProfit += 2 * project.CurrentFunded / 100;
                    }
                }

                // Count pending project
                var pending = db.Projects.Count(x => x.Status == DDLConstants.ProjectStatus.PENDING);
                // Count live project
                var approved = db.Projects.Count(x => x.Status == DDLConstants.ProjectStatus.APPROVED);
                // Count succeed project
                var funed = db.Projects.Count(x => x.IsFunded);
                // Count rank A project
                var rankA = db.Projects.Count(x => x.FundingGoal > 500000000);
                // Count rank B project
                var rankB = db.Projects.Count(x => x.FundingGoal <= 500000000 && x.FundingGoal > 100000000);
                // Count rank C project
                var rankC = db.Projects.Count(x => x.FundingGoal <= 100000000 && x.FundingGoal > 50000000);
                // Count rank D project
                var rankD = db.Projects.Count(x => x.FundingGoal <= 50000000);
                // Count fail project
                var failProject = db.Projects.Count(x => x.IsFunded == false && x.IsExprired);

                var AdminDashboardInfoDTO = new AdminDashboardInfoDTO
                {
                    TotalProject = totalProject,
                    LiveProject = approved,
                    NewProject = pending,
                    TotalFund = totalFunded,
                    TotalProfit = totalProfit,
                    TotalUser = totalUser,
                    TotalSucceed = funed,
                    RankA = rankA,
                    RankB = rankB,
                    RankC = rankC,
                    RankD = rankD,
                    TotalFail = failProject
                };

                return AdminDashboardInfoDTO;
            }
        }

        /// <summary>
        /// Get 5 top projects
        /// </summary>
        /// <returns></returns>
        public List<ProjectBasicListDTO> AdminGetTopProjectList()
        {
            using (var db = new DDLDataContext())
            {
                // Get rewardPkg list
                var projectList = (from Project in db.Projects
                                   where (Project.Status == DDLConstants.ProjectStatus.APPROVED || Project.Status == DDLConstants.ProjectStatus.EXPIRED) && Project.IsFunded
                                   select new ProjectBasicListDTO
                                   {
                                       ProjectCode = Project.ProjectCode,
                                       Title = Project.Title,
                                       Category = Project.Category.Name,
                                       CreatorEmail = Project.Creator.Email,
                                       FundingGoal = Project.FundingGoal,
                                       Status = Project.Status,
                                       CurrentFunded = Project.CurrentFunded,
                                   }).ToList();

                projectList = projectList.OrderByDescending(x => x.CurrentFunded).Take(5).ToList();

                return projectList;
            }
        }


        /// <summary>
        /// Get project statistic evert month in year
        /// </summary>
        /// <returns></returns>
        public AdminProjectStatisticDTO AdminProjectStatistic()
        {
            using (var db = new DDLDataContext())
            {
                var projects = db.Projects.ToList();

                foreach (var project in projects)
                {
                    project.CreatedDate = CommonUtils.ConvertDateTimeFromUtc(project.CreatedDate);
                    project.ExpireDate = CommonUtils.ConvertDateTimeFromUtc(project.CreatedDate);
                }

                // Caculate created project
                var createdProject = projects.Where(x => x.CreatedDate.Year == 2015)
                  .GroupBy(x => new { x.CreatedDate.Month })
                  .Select(grp => new
                  {
                      Month = grp.Key.Month,
                      Total = grp.Count()
                  });

                var listCreated = new List<Statistic>();

                foreach (var project in createdProject)
                {
                    var sum = new Statistic
                    {
                        Amount = project.Total,
                        Month = project.Month
                    };
                    listCreated.Add(sum);
                }

                // Caculate Success project
                var successProject = projects.Where(x => x.ExpireDate.Value.Year == 2015 && x.IsFunded && x.IsExprired)
                  .GroupBy(x => new { x.ExpireDate.Value.Month })
                  .Select(grp => new
                  {
                      Month = grp.Key.Month,
                      Total = grp.Count(),
                  });

                var listSuccess = new List<Statistic>();

                foreach (var project in successProject)
                {
                    var sum = new Statistic
                    {
                        Amount = project.Total,
                        Month = project.Month
                    };
                    listSuccess.Add(sum);
                }

                // Caculate fail project
                var failProject = projects.Where(x => x.ExpireDate.Value.Year == 2015 && x.IsFunded == false && x.IsExprired)
                  .GroupBy(x => new { x.ExpireDate.Value.Month })
                  .Select(grp => new
                  {
                      Month = grp.Key.Month,
                      Total = grp.Count()
                  });

                var listFail = new List<Statistic>();

                foreach (var project in failProject)
                {
                    var sum = new Statistic
                    {
                        Amount = project.Total,
                        Month = project.Month
                    };
                    listFail.Add(sum);
                }

                // Caculate total funded
                var pledge = db.Backings.Where(x => x.BackedDate.Year == 2015)
                    .GroupBy(x => new { x.BackedDate.Month })
                   .Select(g => new
                   {
                       Month = g.Key.Month,
                       Total = g.Sum(i => i.BackingDetail.PledgedAmount)
                   });
                var listFunded = new List<Statistic>();
                foreach (var amount in pledge)
                {
                    var sum = new Statistic
                    {
                        Amount = amount.Total,
                        Month = amount.Month
                    };
                    listFunded.Add(sum);
                }

                var list = new AdminProjectStatisticDTO
                {
                    Created = listCreated,
                    Succeed = listSuccess,
                    Fail = listFail,
                    Funded = listFunded
                };

                return list;
            }
        }
        #endregion


        #endregion

        public ProjectDetailDTO GetProjectByCode(string projectCode, string userName)
        {
            using (var db = new DDLDataContext())
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
                                         NumberComment =
                                             project.Creator.Username == userName
                                                 ? project.Comments.Count
                                                 : project.Comments.Count(x => !x.IsHide),
                                         NumberUpdate = project.UpdateLogs.Count,
                                         Creator = new CreatorDTO
                                         {
                                             FullName = project.Creator.UserInfo.FullName,
                                             UserName = project.Creator.Username,
                                             NumberBacked = project.Creator.Backings.Count,
                                             NumberCreated =
                                                 project.Creator.CreatedProjects.Count(x => x.Status != DDLConstants.ProjectStatus.DRAFT
                                                                                            &&
                                                                                            x.Status !=
                                                                                            DDLConstants.ProjectStatus.REJECTED
                                                                                            &&
                                                                                            x.Status !=
                                                                                            DDLConstants.ProjectStatus.PENDING),
                                             ProfileImage = project.Creator.UserInfo.ProfileImage
                                         },
                                     }).FirstOrDefault();


                // Check project exist?
                if (projectDetail == null)
                {
                    throw new KeyNotFoundException();
                }

                // Check current user remind.
                projectDetail.Reminded = userCurrent != null &&
                                         db.Reminds.Any(
                                             x =>
                                                 x.UserID == userCurrent.DDL_UserID &&
                                                 x.ProjectID == projectDetail.ProjectID);

                // Get rewards.
                var rewardDetail =
                    db.RewardPkgs.Where(x => x.ProjectID == projectDetail.ProjectID && !x.IsHide).ToList();
                var rewardDto = new List<RewardPkgDTO>();
                foreach (var reward in rewardDetail)
                {
                    var Reward = new RewardPkgDTO
                    {
                        Backers = reward.BackingDetails.Count(),
                        Description = reward.Description,
                        EstimatedDelivery =
                            CommonUtils.ConvertDateTimeFromUtc(reward.EstimatedDelivery.GetValueOrDefault()),
                        PledgeAmount = reward.PledgeAmount,
                        Quantity = reward.Quantity
                    };
                    rewardDto.Add(Reward);
                }
                projectDetail.RewardDetail = rewardDto;

                // Convert datetime to gmt+7
                projectDetail.CreatedDate = CommonUtils.ConvertDateTimeFromUtc(projectDetail.CreatedDate);
                projectDetail.ExpireDate =
                    CommonUtils.ConvertDateTimeFromUtc(projectDetail.ExpireDate.GetValueOrDefault());

                // Set number exprire day.
                var timespan = projectDetail.ExpireDate - CommonUtils.DateTodayGMT7();
                projectDetail.NumberDays = timespan.GetValueOrDefault().Days;

                return projectDetail;
            }
        }

        public List<CommentDTO> GetListComment(string projectCode, DateTime? lastDateTime, string userName)
        {
            using (var db = new DDLDataContext())
            {
                var project = db.Projects.FirstOrDefault(x => x.ProjectCode == projectCode);
                if (project == null)
                {
                    throw new KeyNotFoundException();
                }
                // Get comments.
                var commentsList = (userName != null &&
                                    project.Creator.Username.Equals(userName, StringComparison.OrdinalIgnoreCase))
                    ? GetComments(project.ProjectID, true, lastDateTime)
                    : GetComments(project.ProjectID, false, lastDateTime);

                return commentsList;
            }
        }

        public List<UpdateLogDTO> GetListUpdateLog(string projectCode, string userName)
        {
            using (var db = new DDLDataContext())
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
        }

        // 19/10/2015 - MaiCTP - Get BackedProjectHistory
        public List<ProjectBasicViewDTO> GetBackedProjectHistory(String userName)
        {

            using (var db = new DDLDataContext())
            {
                var Project = (from backing in db.Backings
                               from project in db.Projects
                               where backing.User.Username == userName && project.ProjectID == backing.ProjectID
                               select new ProjectBasicViewDTO
                {
                    ProjectID = project.ProjectID,
                    ProjectCode = project.ProjectCode,
                    CreatorName = project.Creator.UserInfo.FullName,
                    Title = project.Title,
                    CurrentFunded = project.CurrentFunded,
                    BackedDate = backing.BackedDate,
                    Status = project.Status
                }).Distinct().ToList();
                return Project;
            }

        }

        // 24/10/2015 - MaiCTP - Get BackingInfo
        public List<BackingInfoDTO> BackingInfo(string projectCode)
        {

            using (var db = new DDLDataContext())
            {
                var Backing = (from backing in db.Backings
                               from rewad in db.RewardPkgs
                               from project in db.Projects
                               where project.ProjectCode == projectCode && backing.ProjectID == project.ProjectID && rewad.RewardPkgID == backing.BackingDetail.RewardPkgID
                               select new BackingInfoDTO
                               {

                                   RewadDiscription = rewad.Description,
                                   PledgeAmount = rewad.PledgeAmount,
                                   Quantity = backing.BackingDetail.Quantity,
                                   BackedDate = backing.BackedDate,
                                   TotalAmount = backing.BackingDetail.Quantity * rewad.PledgeAmount,
                                   BackingDiscription = backing.BackingDetail.Description,
                                   ProfileImage = backing.User.UserInfo.ProfileImage,
                                   FullName = backing.User.UserInfo.FullName,
                                   Email = backing.BackingDetail.Email,
                                   Add = backing.User.UserInfo.Address,
                                   Phone = backing.User.UserInfo.PhoneNumber
                               }).Distinct().ToList();
                return Backing;
            }

        }

        // 17/10/2015 - MaiCTP - Get BackedProject
        public List<ProjectBasicViewDTO> GetBackedProject(String userName)
        {

            using (var db = new DDLDataContext())
            {
                var Project = (from backing in db.Backings
                               from project in db.Projects
                               where backing.User.Username == userName && project.ProjectID == backing.ProjectID
                               select new ProjectBasicViewDTO
                               {
                                   ProjectID = project.ProjectID,
                                   ProjectCode = project.ProjectCode,
                                   Title = project.Title,
                                   CreatorName = project.Creator.UserInfo.FullName,
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

        }

        //18/10/2015 - MaiCTP - Get StarredProject
        public List<ProjectBasicViewDTO> GetStarredProject(String userName)
        {
            using (var db = new DDLDataContext())
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
        }


        // 18/10/2015 - MaiCTP - Get CreatedProject
        public List<ProjectBasicViewDTO> GetCreatedProject(String userName)
        {
            using (var db = new DDLDataContext())
            {
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
                                       PopularPoint = project.PopularPoint,
                                       Status = project.Status
                                   }).Distinct();




                return projectList.ToList();
            }

        }

        //21/10/2015- MaiCTP - DeleteProjectReminded
        public bool DeleteProjectReminded(int projectID)
        {

            using (var db = new DDLDataContext())
            {
                var deleteProjectReminded = db.Reminds.FirstOrDefault(x => x.ProjectID == projectID);

                if (deleteProjectReminded == null)
                {
                    throw new KeyNotFoundException();
                }

                db.Reminds.Remove(deleteProjectReminded);
                db.SaveChanges();

                return true;
            }
        }

        //24/10/2015- MaiCTP - DeleteProjectDraft
        public bool DeleteProjectDraft(int projectID)
        {

            using (var db = new DDLDataContext())
            {
                var deleteProjectDraft = db.Projects.FirstOrDefault(x => x.ProjectID == projectID);

                if (deleteProjectDraft == null)
                {
                    throw new KeyNotFoundException();
                }

                db.Projects.Remove(deleteProjectDraft);
                db.SaveChanges();

                return true;
            }
        }


        public bool RemindProject(string userName, string projectCode)
        {
            using (var db = new DDLDataContext())
            {
                var userCurrent = db.DDL_Users.FirstOrDefault(x => x.Username.Equals(userName));
                var project = db.Projects.FirstOrDefault(x => x.ProjectCode == projectCode);
                var reminded =
                    db.Reminds.FirstOrDefault(
                        x => x.UserID.Equals(userCurrent.DDL_UserID) && x.ProjectID == project.ProjectID);
                if (reminded != null)
                {
                    db.Reminds.Remove(reminded);
                    db.SaveChanges();
                    return false;
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
                return true;
            }
        }


        public void ReportProject(string userName, string projectCode, string content)
        {
            using (var db = new DDLDataContext())
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

        }


        public BackingDTO GetListBacker(string projectCode)
        {
            using (var db = new DDLDataContext())
            {
                var project = db.Projects.FirstOrDefault(x => x.ProjectCode == projectCode);
                // Check project exist?
                if (project == null)
                {
                    throw new KeyNotFoundException();
                }
                project.CreatedDate = CommonUtils.ConvertDateTimeFromUtc(project.CreatedDate);
                var list = new BackingDTO();
                // Get linq.
                var listBackerLinQ = from backer in db.Backings
                                     where project.ProjectID == backer.ProjectID
                                     orderby backer.BackedDate descending
                                     select new BackerDTO
                                     {
                                         Amount = backer.BackingDetail.PledgedAmount,
                                         Date = SqlFunctions.DateAdd("hh", 7, backer.BackedDate),
                                         FullName = backer.User.UserInfo.FullName,
                                     };
                // Generate all date from created date 1 days to now
                var dateMonths = Enumerable.Range(0, 1 + CommonUtils.DateTimeNowGMT7().Subtract(project.CreatedDate.AddDays(-1)).Days)
                    .Select(offset =>
                    {
                        var date = project.CreatedDate.AddDays(-1).AddDays(offset); return new { Year = date.Year, Month = date.Month, Day = date.Day };
                    });
                // Create chart.
                //var query = listBackerLinQ.Where(x => x.Date.HasValue)
                //  .GroupBy(x => new { x.Date.Value.Day, x.Date.Value.Month })
                //  .Select(grp => new
                //  {
                //      Day = grp.Key.Day,
                //      Month = grp.Key.Month,
                //      Total = grp.Sum(x => x.Amount)
                //  }).ToList();

                // Join with data for each day.
                var datetimeNow = CommonUtils.DateTimeNowGMT7();
                var query = (from dateMonth in dateMonths
                             join backing in
                                 (from backing in listBackerLinQ
                                  where backing.Date >= project.CreatedDate && backing.Date <= datetimeNow && backing.Date.HasValue
                                  group backing by new { Year = backing.Date.Value.Year, Month = backing.Date.Value.Month, Day = backing.Date.Value.Day } into g
                                  select new { Key = g.Key, Total = g.Sum(x => x.Amount) }) on dateMonth equals backing.Key into j
                             from k in j.DefaultIfEmpty()
                             select new { Year = dateMonth.Year, Day = dateMonth.Day, Month = dateMonth.Month, Total = k != null ? k.Total : 0 }).ToList();


                var listAmount = new List<SumAmount>();


                // Sum data for each day
                decimal total = 0;
                for (var i = 0; i < query.Count; i++)
                {
                    total += query[i].Total;
                    var sum = new SumAmount
                    {
                        Amount = total,
                        Day = query[i].Day,
                        Month = query[i].Month,
                        Year = query[i].Year,
                    };
                    listAmount.Add(sum);
                }

                // Create table.
                var listBacker = listBackerLinQ.ToList();

                //foreach (var backer in listBackerLinQ)
                //{
                //    backer.Date = CommonUtils.ConvertDateTimeFromUtc(backer.Date.GetValueOrDefault());
                //    listBacker.Add(backer);
                //}
                list.sumAmount = listAmount;
                list.listBacker = listBacker;
                list.Amount = new List<decimal>();
                list.Date = new List<string>();
                foreach (var item in listAmount)
                {
                    var date = item.Day.ToString() + '/' + item.Month + "/" + item.Year;
                    list.Date.Add(date);
                    list.Amount.Add(item.Amount);
                }
                return list;
            }
        }

        #region Comment

        private List<CommentDTO> GetComments(int projectID, bool showHide, DateTime? lastDateTime)
        {
            using (var db = new DDLDataContext())
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
        }

        public List<CommentDTO> AddComment(string projectCode, CommentDTO comment, DateTime lastCommentDateTime)
        {
            using (var db = new DDLDataContext())
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
                    project =
                        db.Projects.FirstOrDefault(
                            c => c.ProjectCode.Equals(projectCode, StringComparison.OrdinalIgnoreCase));
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
        }

        /// <summary>
        /// ShowHideComment function
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns>CommentDTO</returns>
        public CommentDTO ShowHideComment(int id, string userName)
        {
            using (var db = new DDLDataContext())
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
            using (var db = new DDLDataContext())
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
        }

        /// <summary>
        /// DeleteComment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userName"></param>
        /// <returns>boolean</returns>
        public bool DeleteComment(int id, string userName)
        {
            using (var db = new DDLDataContext())
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
        }
        #endregion

        #endregion
    }
}