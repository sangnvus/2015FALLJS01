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
               return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Chưa đăng nhập!", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }

            if (!ModelState.IsValid)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            try
            {
                newProject.CreatorID = currentUser.DDL_UserID;
                var project = ProjectRepository.Instance.CreatProject(newProject);
                id = project.ProjectID;
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }
            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "", Data = id });
        }

        // PUT: api/ProjectApi/EditProject  
        [ResponseType(typeof(ProjectEditDTO))]
        [HttpPost]
        public IHttpActionResult EditProjectBasic()
        {
            var httpRequest = HttpContext.Current.Request;

           ProjectEditDTO updateProject = null;

            if (httpRequest.Form.Count <= 0)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            var projectJson = httpRequest.Form["project"];
            var project = new JavaScriptSerializer().Deserialize<ProjectEditDTO>(projectJson);

          if (project == null)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }
			
            string imageName = "img_" + project.ProjectCode;
            var file = httpRequest.Files["file"];
            var uploadImageName = CommonUtils.UploadImage(file, imageName, DDLConstants.FileType.PROJECT);

  try
            {
                updateProject = ProjectRepository.Instance.EditProjectBasic(project, uploadImageName);
            }
            catch (Exception)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            //var updateProject = ProjectRepository.Instance.EditProjectBasic(project, uploadImageName);

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "" });
        }

        // PUT: api/ProjectApi/EditProjectStory  
        [ResponseType(typeof(ProjectStoryDTO))]
        [HttpPut]
        public IHttpActionResult EditProjectStory(ProjectStoryDTO project)
        {
			    ProjectStoryDTO updateProject = null;

            if (project == null)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }
 try
            {
                updateProject = ProjectRepository.Instance.EditProjectStory(project);
            }
            catch (Exception)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

         //   var updateProject = ProjectRepository.Instance.EditProjectStory(project);

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "" });
        }

        // GET: api/ProjectApi/GetProject/:id
        [HttpGet]
         [ResponseType(typeof(ProjectEditDTO))]
        public IHttpActionResult GetProject(int id)
        {
  ProjectEditDTO project = null;

            var currentUser = getCurrentUser();

            if (currentUser == null)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Chưa đăng nhập!", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }

            if (!ModelState.IsValid)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Chưa đăng nhập", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }

            try
            {
                project = ProjectRepository.Instance.GetProjectBasic(id, currentUser.DDL_UserID);
                if (project.ImageUrl != string.Empty)
                {
                    project.ImageUrl = DDLConstants.FileType.PROJECT + project.ImageUrl;
                }
            }
            catch (ProjectNotFoundException)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Không tìm thấy dự án!", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Data = project });
        }

        // GET: api/ProjectApi/GetProjectStory/:id
        [HttpGet]
        [ResponseType(typeof(ProjectStoryDTO))]
        public IHttpActionResult GetProjectStory(int id)
        {
          ProjectStoryDTO project = null;
            try
            {
                project = ProjectRepository.Instance.GetProjectStory(id);
            }
            catch (Exception)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }
            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Data = project });
        }

        // GET: api/ProjectApi/GetRewardPkg/:id
        [HttpGet]
        [ResponseType(typeof(RewardPkgDTO))]
        public IHttpActionResult GetRewardPkg(int id)
        {
           List<RewardPkgDTO> rewardPkg;

            try
            {
                rewardPkg = RewardPkgRepository.Instance.GetRewardPkg(id);
            }
            catch (Exception)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

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
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Chưa đăng nhập!", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }

            if (!ModelState.IsValid)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            try
            {
                rewardPkg = RewardPkgRepository.Instance.CreateRewardPkg(id, newRewardPkgs);
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }
            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "", Data = rewardPkg });
        }

        // PUT: api/ProjectApi/EditReward  
        [ResponseType(typeof(RewardPkgDTO))]
        [HttpPut]
        public IHttpActionResult EditRewardPkg(List<RewardPkgDTO> rewardPkg)
        {
            bool result = false;
            if (rewardPkg == null)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

           try
            {
                result = RewardPkgRepository.Instance.EditRewardPkg(rewardPkg);
            }
            catch (Exception)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "" });
        }

        // Delete: api/ProjectApi/DeleteRewardPkg/:id 
        [ResponseType(typeof(RewardPkgDTO))]
        [HttpDelete]
        public IHttpActionResult DeleteRewardPkg(int id)
        {
          var currentUser = getCurrentUser();

            if (currentUser == null)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Chưa đăng nhập!", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }

            try
            {
                var result = RewardPkgRepository.Instance.DeleteRewardPkg(id);

            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "" });
        }

        // GET: api/ProjectApi/GetUpdateLog/:id
        [HttpGet]
        [ResponseType(typeof(UpdateLogDTO))]
        public IHttpActionResult GetUpdateLog(int id)
        {
           List<UpdateLogDTO> updateLog = null;
            var currentUser = getCurrentUser();

            if (currentUser == null)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Chưa đăng nhập!", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }

            try
            {
                updateLog = UpdateLogRepository.Instance.GetUpdateLog(id);
            }
            catch (Exception)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Data = updateLog });
        }

        // POST: api/ProjectApi/CreateUpdateLog
        [ResponseType(typeof(UpdateLogDTO))]
        [HttpPost]
        public IHttpActionResult CreateUpdateLog(int id, UpdateLogDTO newUpdateLog)
        {
            var currentUser = getCurrentUser();
			var updateLog = new UpdateLog();

            if (currentUser == null)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Chưa đăng nhập!", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }

            if (!ModelState.IsValid)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

           try
            {


                updateLog = UpdateLogRepository.Instance.CreateUpdateLog(id, newUpdateLog);
            }

            catch (Exception)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }
         
            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "", Data = updateLog });
        }

        // PUT: api/ProjectApi/EditUpdateLog  
        [ResponseType(typeof(UpdateLogDTO))]
        [HttpPut]
        public IHttpActionResult EditUpdateLog(List<UpdateLogDTO> updateLog)
        {
            if (updateLog == null)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            try
            {
                var result = UpdateLogRepository.Instance.EditUpdateLog(updateLog);
            }
            catch (Exception)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

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
 
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }
 
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

        // PUT: api/ProjectApi/EditProjectStory  
        [ResponseType(typeof(ProjectEditDTO))]
        [HttpPut]
        public IHttpActionResult SubmitProject(ProjectEditDTO project)
        {
            ProjectEditDTO updateProject = null;

            if (project == null)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            try
            {
                updateProject = ProjectRepository.Instance.SubmitProject(project);
            }
            catch (Exception)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "" });
        }

        //Trungvn


        // GET: api/ProjectApi/GetProjectDetail?code=code
        [HttpGet]
        [ResponseType(typeof(ProjectDetailDTO))]
        public IHttpActionResult GetProjectDetail(string code)
        {
            ProjectDetailDTO projectDetail = null;
            if (!ModelState.IsValid)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            try
            {
                // Get current user name.
                var currentUser = User.Identity != null ? User.Identity.Name : null;
                projectDetail = ProjectRepository.Instance.GetProjectByCode(code, currentUser);
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    projectDetail.Creator.IsOwner = false;
                }
                else if (projectDetail.Creator.UserName.Equals(User.Identity.Name))
                {
                    projectDetail.Creator.IsOwner = true;
                }
            }
            catch (KeyNotFoundException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Dự án không tồn tại!", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }
            return Ok(new HttpMessageDTO
            {
                Status = DDLConstants.HttpMessageType.SUCCESS,
                Message = "",
                Type = "",
                Data = projectDetail
            });
        }

        //  17/10/2015 - MaiCTP - Get BAckedProject
        public IHttpActionResult GetBackedProject()
        {
            var listBacked = ProjectRepository.Instance.GetBackedProject(User.Identity.Name);
            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Data = listBacked });
        }


        //  18/10/2015 - MaiCTP - Get StarredProject
        public IHttpActionResult GetStarredProject()
        {
            var listStarred = ProjectRepository.Instance.GetStarredProject(User.Identity.Name);
            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Data = listStarred });
        }

        //  18/10/2015 - MaiCTP - Get CreatedProject
        public IHttpActionResult GetCreatedProject()
        {
            var listCreated = ProjectRepository.Instance.GetCreatedProject(User.Identity.Name);
            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Data = listCreated });
        }

        #region Comment Functions

        /// <summary>
        /// Post: api/ProjectApi/Comment?projectCode=xxx
        /// </summary>
        /// <param name="projectCode">projectCode</param>
        /// <param name="lastComment">CommentDTO</param>
        /// <param name="comment">CommentDTO</param>
        /// <returns>CommentDTO</returns>
        [HttpPost]
        [ResponseType(typeof(CommentDTO))]
        public IHttpActionResult Comment(string projectCode, DateTime lastComment, CommentDTO comment)
        {
            List<CommentDTO> result = null;

            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }

            try
            {
                result = ProjectRepository.Instance.AddComment(projectCode, comment, lastComment);
            }
            catch (UserNotFoundException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Người dùng không tồn tại!", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (KeyNotFoundException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Dự án không tồn tại!", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }
            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "", Data = result });
        }

        /// <summary>
        /// Put: api/ProjectApi/ShowHideComment?id=
        /// </summary>
        /// <param name="id"></param>
        /// <returns>CommentDTO</returns>
        [HttpPut]
        [ResponseType(typeof(CommentDTO))]
        public IHttpActionResult ShowHideComment(int id)
        {
            CommentDTO result = null;

            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }

            try
            {
                result = ProjectRepository.Instance.ShowHideComment(id, User.Identity.Name);
            }
            catch (UserNotFoundException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Người dùng không tồn tại!", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (KeyNotFoundException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Bình luận không tồn tại!", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }
            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "", Data = result });
        }

        /// <summary>
        /// Put: api/ProjectApi/EditComment?id=&content=
        /// </summary>
        /// <param name="id"></param>
        /// <param name="content"></param>
        /// <returns>CommentDTO</returns>
        [HttpPut]
        [ResponseType(typeof(CommentDTO))]
        public IHttpActionResult EditComment(int id, string content)
        {
            CommentDTO result = null;

            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }

            try
            {
                result = ProjectRepository.Instance.EditComment(id, User.Identity.Name, content);
            }
            catch (UserNotFoundException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Người dùng không tồn tại!", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (KeyNotFoundException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Bình luận không tồn tại!", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }
            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "", Data = result });
        }

        /// <summary>
        /// Put: api/ProjectApi/DeleteComment?id=
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [ResponseType(typeof(CommentDTO))]
        public IHttpActionResult DeleteComment(int id)
        {
            var result = false;

            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }

            try
            {
                result = ProjectRepository.Instance.DeleteComment(id, User.Identity.Name);
            }
            catch (UserNotFoundException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Người dùng không tồn tại!", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (KeyNotFoundException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Bình luận không tồn tại!", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }
            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "", Data = result });
        }
        #endregion
    }
}