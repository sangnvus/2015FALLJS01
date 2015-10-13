using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Script.Serialization;
using DDL_CapstoneProject.Helper;
using DDL_CapstoneProject.Models;
using DDL_CapstoneProject.Models.DTOs;
using DDL_CapstoneProject.Respository;
using DDL_CapstoneProject.Ultilities;

namespace DDL_CapstoneProject.Controllers.ApiControllers
{
    public class ProjectApiController : BaseApiController
    {
        // POST: api/ProjectApi/CreateProject
        [ResponseType(typeof(ProjectCreateDTO))]
        [HttpPost]
        public IHttpActionResult CreateProject(ProjectCreateDTO newProject)
        {
            int id;
            var currentUser = getCurrentUser();

            if (currentUser == null)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Chưa đăng nhập!", Type = "UserNotFound" });
            }

            if (!ModelState.IsValid)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = "Bad-Request" });
            }

            try
            {
                newProject.CreatorID = currentUser.DDL_UserID;
                var project = ProjectRepository.Instance.CreatProject(newProject);
                id = project.ProjectID;
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = "Bad-Request" });
            }
            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "", Data = id });
        }

        // PUT: api/ProjectApi/EditProject  
        [ResponseType(typeof(ProjectDetailDTO))]
        [HttpPost]
        public IHttpActionResult EditProjectBasic()
        {
            var httpRequest = HttpContext.Current.Request;

            if (httpRequest.Form.Count <= 0)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            var projectJson = httpRequest.Form["project"];
            var project = new JavaScriptSerializer().Deserialize<ProjectDetailDTO>(projectJson);

            string imageName = "img_" + project.ProjectCode;
            var file = httpRequest.Files["file"];
            var uploadImageName = CommonUtils.UploadImage(file, imageName, DDLConstants.FileType.PROJECT);

            var updateProject = ProjectRepository.Instance.EditProjectBasic(project, uploadImageName);

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "" });
        }

        // PUT: api/ProjectApi/EditProjectStory  
        [ResponseType(typeof(ProjectStoryDTO))]
        [HttpPut]
        public IHttpActionResult EditProjectStory(ProjectStoryDTO project)
        {
            if (project == null)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            var updateProject = ProjectRepository.Instance.EditProjectStory(project);

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "" });
        }

        // GET: api/ProjectApi/GetProject/:id
        [HttpGet]
        [ResponseType(typeof(ProjectDetailDTO))]
        public IHttpActionResult GetProject(int id)
        {
            var project = ProjectRepository.Instance.GetProjectBasic(id);

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Data = project });
        }

        // GET: api/ProjectApi/GetProjectStory/:id
        [HttpGet]
        [ResponseType(typeof(ProjectStoryDTO))]
        public IHttpActionResult GetProjectStory(int id)
        {
            var project = ProjectRepository.Instance.GetProjectStory(id);

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Data = project });
        }

        // GET: api/ProjectApi/GetRewardPkg/:id
        [HttpGet]
        [ResponseType(typeof(RewardPkgDTO))]
        public IHttpActionResult GetRewardPkg(int id)
        {
            var rewardPkg = RewardPkgRepository.Instance.GetRewardPkg(id);

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Data = rewardPkg });
        }

        // POST: api/ProjectApi/CreateRewardPkg
        [ResponseType(typeof(RewardPkgDTO))]
        [HttpPost]
        public IHttpActionResult CreateRewardPkg(int id, RewardPkgDTO newRewardPkgs)
        {
            var currentUser = getCurrentUser();
            var rewardPkg = new RewardPkg();

            if (currentUser == null)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Chưa đăng nhập!", Type = "UserNotFound" });
            }

            if (!ModelState.IsValid)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = "Bad-Request" });
            }

            try
            {
                rewardPkg = RewardPkgRepository.Instance.CreateRewardPkg(id, newRewardPkgs);
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = "Bad-Request" });
            }
            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "", Data = rewardPkg });
        }

        // PUT: api/ProjectApi/EditReward  
        [ResponseType(typeof(RewardPkgDTO))]
        [HttpPut]
        public IHttpActionResult EditRewardPkg(List<RewardPkgDTO> rewardPkg)
        {
            if (rewardPkg == null)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = "Bad-Request" });
            }

            var result = RewardPkgRepository.Instance.EditRewardPkg(rewardPkg);

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "" });
        }

        // Delete: api/ProjectApi/DeleteRewardPkg/:id 
        [ResponseType(typeof(RewardPkgDTO))]
        [HttpDelete]
        public IHttpActionResult DeleteRewardPkg(int id)
        {
            try
            {
                var result = RewardPkgRepository.Instance.DeleteRewardPkg(id);

            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = "Bad-Request" });
            }

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "" });
        }

        // GET: api/ProjectApi/GetUpdateLog/:id
        [HttpGet]
        [ResponseType(typeof(UpdateLogDTO))]
        public IHttpActionResult GetUpdateLog(int id)
        {
            var updateLog = UpdateLogRepository.Instance.GetUpdateLog(id);

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Data = updateLog });
        }

        // POST: api/ProjectApi/CreateUpdateLog
        [ResponseType(typeof(UpdateLogDTO))]
        [HttpPost]
        public IHttpActionResult CreateUpdateLog(int id, UpdateLogDTO newUpdateLog)
        {
            var currentUser = getCurrentUser();

            if (currentUser == null)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Chưa đăng nhập!", Type = "UserNotFound" });
            }

            if (!ModelState.IsValid)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = "Bad-Request" });
            }

            //try
            //{
            var updateLog = UpdateLogRepository.Instance.CreateUpdateLog(id, newUpdateLog);
            //}
            //catch (Exception)
            //{
            //    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = "Bad-Request" });
            //}
            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "", Data = updateLog });
        }

        // PUT: api/ProjectApi/EditUpdateLog  
        [ResponseType(typeof(UpdateLogDTO))]
        [HttpPut]
        public IHttpActionResult EditUpdateLog(List<UpdateLogDTO> updateLog)
        {
            if (updateLog == null)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = "Bad-Request" });
            }

            var result = UpdateLogRepository.Instance.EditUpdateLog(updateLog);

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "" });
        }

        /// <summary>
        /// Delete a updateLog
        /// </summary>
        /// <param name="id">updateLogID</param>
        /// <returns>HttpMessageDTO</returns>
        [ResponseType(typeof(UpdateLogDTO))]
        [HttpDelete]
        public IHttpActionResult DeleteUpdateLog(int id)
        {
            try
            {
                var result = UpdateLogRepository.Instance.DeleteUpdateLog(id);

            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = "Bad-Request" });
            }

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "" });
        }

        // Post: Upload Image
        [HttpPost]
        public IHttpActionResult fileUpload(string ProjectCode)
        {
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count <= 0)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }
            string imageName = "img_" + ProjectCode;
            var file = httpRequest.Files[0];
            var uploadImageName = CommonUtils.UploadImage(file, imageName, DDLConstants.FileType.PROJECT);

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "" });
        }


        //TrungVN

        public IHttpActionResult GetProject(int take, int categoryid, string orderby)
        {
            var listGetProject = ProjectRepository.Instance.GetProject(take, categoryid, orderby);
            return Ok(new HttpMessageDTO { Status = "success", Data = listGetProject });
        }
        public IHttpActionResult GetProjectByCategory()
        {
            var listGetProjectByCategory = ProjectRepository.Instance.GetProjectByCategory();
            return Ok(new HttpMessageDTO { Status = "success", Data = listGetProjectByCategory });
        }
        public IHttpActionResult GetProjectStatisticList()
        {
            var listGetProjectStatistic = ProjectRepository.Instance.GetProjectStatisticList();
            return Ok(new HttpMessageDTO { Status = "success", Data = listGetProjectStatistic });
        }
        public IHttpActionResult GetStatisticListForHome()
        {
            var listStatisticForHome = ProjectRepository.Instance.GetStatisticListForHome();
            return Ok(new HttpMessageDTO { Status = "success", Data = listStatisticForHome });
        }

        //Trungvn
    }
}