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
        #region HuyNM project function

        // POST: api/ProjectApi/CreateProject
        [ResponseType(typeof(ProjectCreateDTO))]
        [HttpPost]
        public IHttpActionResult CreateProject(ProjectCreateDTO newProject)
        {
            int id;
            string projectCode = "";
            try
            {
                // Check authen.
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }

                if (!ModelState.IsValid)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
                }

                projectCode = ProjectRepository.Instance.CreatProject(newProject, User.Identity.Name);
            }
            catch (UserNotFoundException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }
            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "", Data = projectCode });
        }

        // PUT: api/ProjectApi/EditProject  
        [ResponseType(typeof(ProjectEditDTO))]
        [HttpPost]
        public IHttpActionResult EditProjectBasic()
        {
            ProjectEditDTO updateProject = null;

            try
            {
                // Check authen.
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }

                var httpRequest = HttpContext.Current.Request;


                if (httpRequest.Form.Count <= 0)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
                }

                var projectJson = httpRequest.Form["project"];
                var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
                var project = serializer.Deserialize<ProjectEditDTO>(projectJson);

                if (project == null)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
                }

                string imageName = "img_" + project.ProjectCode;
                var file = httpRequest.Files["file"];
                var uploadImageName = CommonUtils.UploadImage(file, imageName, DDLConstants.FileType.PROJECT);


                updateProject = ProjectRepository.Instance.EditProjectBasic(project, uploadImageName, User.Identity.Name);
                if (updateProject.ImageUrl != string.Empty)
                {
                    updateProject.ImageUrl = DDLConstants.FileType.PROJECT + updateProject.ImageUrl;
                }
            }
            catch (KeyNotFoundException)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (NotPermissionException)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }
            catch (Exception)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Data = updateProject });
        }

        // PUT: api/ProjectApi/EditProjectStory  
        [ResponseType(typeof(ProjectStoryDTO))]
        [HttpPut]
        public IHttpActionResult EditProjectStory(ProjectStoryDTO project)
        {
            ProjectStoryDTO updateProject = null;
            try
            {
                if (project == null)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
                }

                // Check authen.
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }

                updateProject = ProjectRepository.Instance.EditProjectStory(project, User.Identity.Name);
            }
            catch (KeyNotFoundException)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (NotPermissionException)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }
            catch (Exception)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "" });
        }

        // GET: api/ProjectApi/GetProject/:code
        [HttpGet]
        [ResponseType(typeof(ProjectEditDTO))]
        public IHttpActionResult GetProjectBasic(string code)
        {
            ProjectEditDTO project = null;

            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
                }

                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Chưa đăng nhập", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }

                project = ProjectRepository.Instance.GetProjectBasic(code, User.Identity.Name);
                if (project.ImageUrl != string.Empty)
                {
                    project.ImageUrl = DDLConstants.FileType.PROJECT + project.ImageUrl;
                }
            }
            catch (ProjectNotFoundException)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Không tìm thấy dự án!", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (NotPermissionException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
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
                // Check authen.
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }

                project = ProjectRepository.Instance.GetProjectStory(id, User.Identity.Name);
            }
            catch (KeyNotFoundException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (NotPermissionException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
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
                // Check authen.
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }

                rewardPkg = ProjectRepository.Instance.GetRewardPkg(id);
            }
            catch (Exception)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Data = rewardPkg });
        }

        // GET: api/ProjectApi/GetRewardPkgByCode/:code
        [HttpGet]
        [ResponseType(typeof(RewardPkgDTO))]
        public IHttpActionResult GetRewardPkgByCode(string code)
        {
            List<RewardPkgDTO> rewardPkg;
            try
            {
                // Check authen.
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }

                rewardPkg = ProjectRepository.Instance.GetRewardPkgByCode(code);
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
            var rewardPkg = new RewardPkgDTO();

            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
                }

                // Check authen.
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }

                rewardPkg = ProjectRepository.Instance.CreateRewardPkg(id, newRewardPkgs, User.Identity.Name);
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
        public IHttpActionResult EditRewardPkg(RewardPkgDTO rewardPkg)
        {
            bool result = false;

            try
            {
                // Check authen.
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }

                if (rewardPkg == null)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
                }

                result = ProjectRepository.Instance.EditRewardPkg(rewardPkg, User.Identity.Name);
            }
            catch (KeyNotFoundException)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (NotPermissionException)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
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
            try
            {
                // Check authen.
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }

                var result = ProjectRepository.Instance.DeleteRewardPkg(id, User.Identity.Name);
            }
            catch (KeyNotFoundException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (NotPermissionException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
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
            try
            {
                // Check authen.
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }

                updateLog = ProjectRepository.Instance.GetUpdateLog(id);
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
            var updateLog = new UpdateLogDTO();

            try
            {
                // Check authen.
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }

                if (!ModelState.IsValid)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
                }

                updateLog = ProjectRepository.Instance.CreateUpdateLog(id, newUpdateLog, User.Identity.Name);
            }
            catch (KeyNotFoundException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (NotPermissionException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
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
            var result = false;
            try
            {
                // Check authen.
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }

                if (updateLog == null)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
                }

                result = ProjectRepository.Instance.EditUpdateLog(updateLog, User.Identity.Name);
            }
            catch (KeyNotFoundException)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (NotPermissionException)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }
            catch (Exception)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "" });
        }

        // PUT: api/ProjectApi/EditSingleUpdateLog  
        [ResponseType(typeof(UpdateLogDTO))]
        [HttpPut]
        public IHttpActionResult EditSingleUpdateLog(UpdateLogDTO updateLog)
        {
            var result = new UpdateLogDTO();
            try
            {
                // Check authen.
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }

                if (updateLog == null)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
                }

                result = ProjectRepository.Instance.EditSingleUpdateLog(updateLog, User.Identity.Name);
            }
            catch (KeyNotFoundException)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (NotPermissionException)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }
            catch (Exception)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "", Data = result });
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
            var result = false;

            try
            {
                // Check authen.
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }

                result = ProjectRepository.Instance.DeleteUpdateLog(id, User.Identity.Name);
            }
            catch (KeyNotFoundException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (NotPermissionException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "" });
        }

        // GET: api/ProjectApi/GetTimeline/:id
        [HttpGet]
        [ResponseType(typeof(TimeLineDTO))]
        public IHttpActionResult GetTimeLine(int id)
        {
            List<TimeLineDTO> timeline;

            try
            {
                // Check authen.
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }

                timeline = ProjectRepository.Instance.GetTimeLine(id);
                foreach (var point in timeline)
                {
                    if (point.ImageUrl != string.Empty)
                    {
                        point.ImageUrl = DDLConstants.FileType.PROJECT + point.ImageUrl;
                    }
                }

            }
            catch (Exception)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Data = timeline });
        }

        // PUT: api/ProjectApi/CreateTimeline/:id
        [ResponseType(typeof(TimeLineDTO))]
        [HttpPost]
        public IHttpActionResult CreateTimeline(int id)
        {
            var newTimeLine = new TimeLineDTO();

            try
            {
                // Check authen.
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }

                var httpRequest = HttpContext.Current.Request;


                if (httpRequest.Form.Count <= 0)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
                }

                var timelineJson = httpRequest.Form["timeline"];
                var timeline = new JavaScriptSerializer().Deserialize<TimeLineDTO>(timelineJson);

                if (timeline == null)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
                }

                string imageName = "imgTimeline";
                var file = httpRequest.Files["file"];
                //var uploadImageName = CommonUtils.UploadImage(file, imageName, DDLConstants.FileType.PROJECT);

                newTimeLine = ProjectRepository.Instance.CreateTimeline(id, timeline, imageName);
                var uploadImageName = CommonUtils.UploadImage(file, newTimeLine.ImageUrl, DDLConstants.FileType.PROJECT);
                bool editTimeline = ProjectRepository.Instance.EditTimeline(newTimeLine, uploadImageName);
                newTimeLine.ImageUrl = uploadImageName;

                if (newTimeLine.ImageUrl != string.Empty)
                {
                    newTimeLine.ImageUrl = DDLConstants.FileType.PROJECT + newTimeLine.ImageUrl;
                }
            }
            catch (Exception)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Data = newTimeLine });
        }

        // PUT: api/ProjectApi/UpdateTimeline
        [ResponseType(typeof(TimeLineDTO))]
        [HttpPost]
        public IHttpActionResult UpdateTimeline()
        {
            bool updateTimeLine = false;

            try
            {
                // Check authen.
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }

                var httpRequest = HttpContext.Current.Request;

                if (httpRequest.Form.Count <= 0)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
                }

                var timelineJson = httpRequest.Form["timeline"];
                var timeline = new JavaScriptSerializer().Deserialize<TimeLineDTO>(timelineJson);

                if (timeline == null)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
                }

                string imageName = "imgTimeline_" + timeline.TimelineID;
                var file = httpRequest.Files["file"];
                var uploadImageName = CommonUtils.UploadImage(file, imageName, DDLConstants.FileType.PROJECT);

                updateTimeLine = ProjectRepository.Instance.EditTimeline(timeline, uploadImageName);
            }
            catch (Exception)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Data = updateTimeLine });
        }

        // Delete: api/ProjectApi/DeleteTimeline/:id 
        [ResponseType(typeof(TimeLineDTO))]
        [HttpDelete]
        public IHttpActionResult DeleteTimeline(int id)
        {
            var result = false;

            try
            {
                // Check authen.
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }

                result = ProjectRepository.Instance.DeleteTimeline(id);

            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "" });
        }

        // GET: api/ProjectApi/GetQuestion/:id
        [HttpGet]
        [ResponseType(typeof(QuestionDTO))]
        public IHttpActionResult GetQuestion(int id)
        {
            List<QuestionDTO> questionList;

            try
            {
                // Check authen.
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }

                questionList = ProjectRepository.Instance.GetQuestion(id);
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Data = questionList });
        }

        // POST: api/ProjectApi/CreateQuestion
        [ResponseType(typeof(QuestionDTO))]
        [HttpPost]
        public IHttpActionResult CreateQuestion(int id, QuestionDTO newQuestion)
        {
            var newQA = new QuestionDTO();

            try
            {
                // Check authen.
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }


                if (!ModelState.IsValid)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
                }

                newQA = ProjectRepository.Instance.CreateQuestion(id, newQuestion, User.Identity.Name);
            }
            catch (KeyNotFoundException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (NotPermissionException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }
            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "", Data = newQA });
        }

        // PUT: api/ProjectApi/EditQuestion
        [ResponseType(typeof(QuestionDTO))]
        [HttpPut]
        public IHttpActionResult EditQuestion(List<QuestionDTO> question)
        {
            bool result = false;

            try
            {
                // Check authen.
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }

                if (question == null)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
                }

                result = ProjectRepository.Instance.EditQuestion(question, User.Identity.Name);
            }
            catch (KeyNotFoundException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (NotPermissionException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "" });
        }

        // PUT: api/ProjectApi/EditSingleQuestion  
        [ResponseType(typeof(UpdateLogDTO))]
        [HttpPut]
        public IHttpActionResult EditSingleQuestion(QuestionDTO question)
        {
            var result = new QuestionDTO();
            try
            {
                // Check authen.
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }

                if (question == null)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
                }

                result = ProjectRepository.Instance.EditSingleQuestion(question, User.Identity.Name);
            }
            catch (NotPermissionException)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (KeyNotFoundException)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }
            catch (Exception)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "", Data = result });
        }

        // Delete: api/ProjectApi/DeleteQuestion/:id 
        [ResponseType(typeof(QuestionDTO))]
        [HttpDelete]
        public IHttpActionResult DeleteQuestion(int id)
        {
            try
            {
                // Check authen.
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }

                var result = ProjectRepository.Instance.DeleteQuestion(id, User.Identity.Name);
            }
            catch (KeyNotFoundException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (NotPermissionException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "" });
        }

        // PUT: api/ProjectApi/EditProjectStory  
        [ResponseType(typeof(ProjectEditDTO))]
        [HttpPut]
        public IHttpActionResult SubmitProject(ProjectEditDTO project)
        {
            List<string> errorList = null;

            try
            {
                // Check authen.
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }


                int flag = 0;

                if (project == null)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
                }

                if (project.Status != DDLConstants.ProjectStatus.DRAFT &&
                    project.Status != DDLConstants.ProjectStatus.REJECTED)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
                }

                errorList = ProjectRepository.Instance.SubmitProject(project, User.Identity.Name);
                foreach (var error in errorList)
                {
                    if (!string.IsNullOrEmpty(error))
                    {
                        flag = 1;
                    }
                }

                if (flag == 0)
                {
                    project.Status = DDLConstants.ProjectStatus.PENDING;
                }
                else
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = "", Data = errorList });
                }
            }
            catch (KeyNotFoundException)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (NotPermissionException)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }
            catch (Exception)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = DDLConstants.HttpMessageType.SUCCESS, Type = "", Data = project });
        }

        // POST: api/ProjectApi/BackProject
        [ResponseType(typeof(ProjectBackDTO))]
        [HttpPost]
        public IHttpActionResult BackProject(ProjectBackDTO backingData)
        {
            int backingId;

            try
            {
                // Check authen.
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }

                if (!ModelState.IsValid)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
                }

                backingData.OrderId = backingData.ProjectCode + CommonUtils.GenerateVerifyCode() + DateTime.Now.ToString("hmmsstt");
                backingData.TransactionId = CommonUtils.GenerateVerifyCode().Substring(0, 13);
                backingData.BackedDate = DateTime.Now;

                backingId = ProjectRepository.Instance.BackProject(backingData);
                ProjectRepository.Instance.CaculateProjectPoint(backingData.ProjectCode, DDLConstants.PopularPointType.BackingPoint);
            }
            catch (KeyNotFoundException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (UserNotFoundException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }
            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "", Data = backingId });
        }

        // GET: api/ProjectApi/GetBackProjectInfo/:code
        [HttpGet]
        [ResponseType(typeof(ProjectInfoBackDTO))]
        public IHttpActionResult GetBackProjectInfo(string code)
        {
            var projectInfo = new ProjectInfoBackDTO();

            try
            {
                // Check authen.
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }

                projectInfo = ProjectRepository.Instance.GetBackProjectInfo(code);
            }
            catch (KeyNotFoundException)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (Exception)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Data = projectInfo });
        }

        // GET: api/ProjectApi/GetBackingDetail/:id
        [HttpGet]
        [ResponseType(typeof(ProjectBackDTO))]
        public IHttpActionResult GetBackingDetail(int backingId)
        {
            var backingDetail = new ProjectBackDTO();

            try
            {
                // Check authen.
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }

                backingDetail = ProjectRepository.Instance.GetBackingDetail(backingId, User.Identity.Name);
            }
            catch (KeyNotFoundException)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (NotPermissionException)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (Exception)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Data = backingDetail });
        }

        // GET: api/ProjectApi/AdminGetBackProjectInfo/:id
        [HttpGet]
        [ResponseType(typeof(ProjectBackDTO))]
        public IHttpActionResult AdminGetBackingDetail(int backingId)
        {
            var backingDetail = new ProjectBackDTO();

            try
            {
                // Check authen.
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }

                backingDetail = ProjectRepository.Instance.AdminGetBackingDetail(backingId);
            }
            catch (KeyNotFoundException)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (Exception)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Data = backingDetail });
        }

        #region Admin
        // GET: api/ProjectApi/GetPendingProjectList/
        [HttpGet]
        [ResponseType(typeof(ProjectBasicListDTO))]
        public IHttpActionResult GetPendingProjectList()
        {
            var pendingList = new List<ProjectBasicListDTO>();

            try
            {
                // Check authen.
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }

                // Check role user.
                var currentUser = UserRepository.Instance.GetBasicInfo(User.Identity.Name);
                if (currentUser == null || currentUser.Role != DDLConstants.UserType.ADMIN)
                {
                    throw new NotPermissionException();
                }

                pendingList = ProjectRepository.Instance.GetPendingProjectList();
            }
            catch (Exception)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Data = pendingList });
        }

        // GET: api/ProjectApi/AdminGetProjectGeneralInfo/
        [HttpGet]
        [ResponseType(typeof(AdminProjectGeneralInfoDTO))]
        public IHttpActionResult AdminGetProjectGeneralInfo()
        {
            var generalInfo = new AdminProjectGeneralInfoDTO();

            try
            {
                // Check authen.
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }

                // Check role user.
                var currentUser = UserRepository.Instance.GetBasicInfo(User.Identity.Name);
                if (currentUser == null || currentUser.Role != DDLConstants.UserType.ADMIN)
                {
                    throw new NotPermissionException();
                }

                generalInfo = ProjectRepository.Instance.AdminProjectGeneralInfo();
            }
            catch (Exception)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Data = generalInfo });
        }

        // GET: api/ProjectApi/AdminGetProjectList/
        [HttpGet]
        [ResponseType(typeof(ProjectBasicListDTO))]
        public IHttpActionResult AdminGetProjectList()
        {
            var projectList = new List<ProjectBasicListDTO>();

            try
            {
                // Check authen.
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }

                // Check role user.
                var currentUser = UserRepository.Instance.GetBasicInfo(User.Identity.Name);
                if (currentUser == null || currentUser.Role != DDLConstants.UserType.ADMIN)
                {
                    throw new NotPermissionException();
                }

                projectList = ProjectRepository.Instance.GetProjectList();
            }
            catch (NotPermissionException)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }
            catch (Exception)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Data = projectList });
        }

        // GET: api/ProjectApi/AdminGetProjectDetail/:code
        [HttpGet]
        [ResponseType(typeof(ProjectDetailDTO))]
        public IHttpActionResult AdminGetProjectDetail(string code)
        {
            var project = new ProjectDetailDTO();

            try
            {
                // Check authen.
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }

                // Check role user.
                var currentUser = UserRepository.Instance.GetBasicInfo(User.Identity.Name);
                if (currentUser == null || currentUser.Role != DDLConstants.UserType.ADMIN)
                {
                    throw new NotPermissionException();
                }

                project = ProjectRepository.Instance.AdminGetProjectDetail(code);
                project.Question = ProjectRepository.Instance.GetQuestion(project.ProjectID);
                if (project.ImageUrl != string.Empty)
                {
                    project.ImageUrl = DDLConstants.FileType.PROJECT + project.ImageUrl;
                }
            }
            catch (Exception)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Data = project });
        }

        // GET: api/ProjectApi/AdminGetProjectComment/:code
        [HttpGet]
        [ResponseType(typeof(CommentDTO))]
        public IHttpActionResult AdminGetProjectComment(string code, string lastDateTime = "")
        {
            List<CommentDTO> result = null;

            try
            {
                // Check authen.
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }

                var datetime = !string.IsNullOrEmpty(lastDateTime) ? DateTime.Parse(lastDateTime) : CommonUtils.DateTimeNowGMT7();

                // Check role user.
                var currentUser = UserRepository.Instance.GetBasicInfo(User.Identity.Name);
                if (currentUser == null || currentUser.Role != DDLConstants.UserType.ADMIN)
                {
                    throw new NotPermissionException();
                }
                result = ProjectRepository.Instance.GetListComment(code, datetime, currentUser.UserName);
            }
            catch (KeyNotFoundException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Dự án không tồn tại!", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (Exception)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Data = result });
        }

        // GET: api/ProjectApi/AdminGetUpdateLogList?code=code
        [HttpGet]
        [ResponseType(typeof(UpdateLogDTO))]
        public IHttpActionResult AdminGetUpdateLogList(string code)
        {
            List<UpdateLogDTO> result = null;

            try
            {
                // Check authen.
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }

                // Check role user.
                var currentUser = UserRepository.Instance.GetBasicInfo(User.Identity.Name);
                if (currentUser == null || currentUser.Role != DDLConstants.UserType.ADMIN)
                {
                    throw new NotPermissionException();
                }
                result = ProjectRepository.Instance.GetListUpdateLog(code, currentUser.UserName);
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
                Data = result
            });
        }

        // PUT: api/ProjectApi/AdminChangeProjectStatus/:id  
        [ResponseType(typeof(ProjectEditDTO))]
        [HttpPut]
        public IHttpActionResult AdminChangeProjectStatus(ProjectEditDTO project)
        {
            bool result = false;

            try
            {
                // Check authen.
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }


                if (project == null)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
                }

                // Check role user.
                var currentUser = UserRepository.Instance.GetBasicInfo(User.Identity.Name);
                if (currentUser == null || currentUser.Role != DDLConstants.UserType.ADMIN)
                {
                    throw new NotPermissionException();
                }
                result = ProjectRepository.Instance.AdminChangeProjectStatus(project, User.Identity.Name);
            }
            catch (Exception)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "", Data = result });
        }

        // PUT: api/ProjectApi/AdminGetListBacker/:code
        [ResponseType(typeof(BackingDTO))]
        [HttpGet]
        public IHttpActionResult AdminGetListBacker(string code)
        {
            var listBacker = new BackingDTO();

            try
            {
                // Check authen.
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }

                // Check role user.
                var currentUser = UserRepository.Instance.GetBasicInfo(User.Identity.Name);
                if (currentUser == null || currentUser.Role != DDLConstants.UserType.ADMIN)
                {
                    throw new NotPermissionException();
                }
                listBacker = ProjectRepository.Instance.GetListBacker(code);
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            return Ok(new HttpMessageDTO { Status = "success", Message = "", Type = "", Data = listBacker });
        }

        // GET: api/ProjectApi/AdminGetDashboardInfo/
        [HttpGet]
        [ResponseType(typeof(AdminDashboardInfoDTO))]
        public IHttpActionResult AdminGetDashboardInfo()
        {
            var generalInfo = new AdminDashboardInfoDTO();

            try
            {
                // Check authen.
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }

                // Check role user.
                var currentUser = UserRepository.Instance.GetBasicInfo(User.Identity.Name);
                if (currentUser == null || currentUser.Role != DDLConstants.UserType.ADMIN)
                {
                    throw new NotPermissionException();
                }

                generalInfo = ProjectRepository.Instance.AdminDashboardInfo();
            }
            catch (Exception)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Data = generalInfo });
        }

        // GET: api/ProjectApi/AdminGetTopProjectList/
        [HttpGet]
        [ResponseType(typeof(ProjectBasicListDTO))]
        public IHttpActionResult AdminGetTopProjectList()
        {
            var projectList = new List<ProjectBasicListDTO>();

            try
            {
                // Check authen.
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }

                // Check role user.
                var currentUser = UserRepository.Instance.GetBasicInfo(User.Identity.Name);
                if (currentUser == null || currentUser.Role != DDLConstants.UserType.ADMIN)
                {
                    throw new NotPermissionException();
                }

                projectList = ProjectRepository.Instance.AdminGetTopProjectList();
            }
            catch (Exception)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Data = projectList });
        }

        // GET: api/ProjectApi/AdminGetProjectStatistic/:year
        [HttpGet]
        [ResponseType(typeof(AdminProjectStatisticDTO))]
        public IHttpActionResult AdminGetProjectStatistic(int year)
        {
            var statistic = new AdminProjectStatisticDTO();

            try
            {
                // Check authen.
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }

                // Check role user.
                var currentUser = UserRepository.Instance.GetBasicInfo(User.Identity.Name);
                if (currentUser == null || currentUser.Role != DDLConstants.UserType.ADMIN)
                {
                    throw new NotPermissionException();
                }

                statistic = ProjectRepository.Instance.AdminProjectStatistic(year);
            }
            catch (Exception)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Data = statistic });
        }

        // GET: api/ProjectApi/AdminGetStatisticTable/
        [HttpGet]
        [ResponseType(typeof(AdminDashboardInfoDTO))]
        public IHttpActionResult AdminGetStatisticTable(string option)
        {
            var statistic = new List<AdminDashboardInfoDTO>();

            try
            {
                // Check authen.
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }

                // Check role user.
                var currentUser = UserRepository.Instance.GetBasicInfo(User.Identity.Name);
                if (currentUser == null || currentUser.Role != DDLConstants.UserType.ADMIN)
                {
                    throw new NotPermissionException();
                }

                statistic = ProjectRepository.Instance.AdminStatisticTable(option);
            }
            catch (Exception)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Data = statistic });
        }
        #endregion

        #endregion


        #region TrungVN
        [HttpGet]
        public IHttpActionResult SearchCount(string categoryidlist, string searchkey, string statusString)
        {


            int result;
            try
            {
                // Get current user name.
                result = ProjectRepository.Instance.SearchCount(categoryidlist, searchkey, statusString);
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
                Data = result
            });
        }
        public IHttpActionResult GetProjectTop(string categoryid)
        {
            List<ProjectBasicViewDTO> result = null;
            try
            {
                result = ProjectRepository.Instance.GetProjectTop(categoryid);
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
                Data = result
            });
        }


        [HttpGet]
        public IHttpActionResult projectTitleList(string searchkey)
        {
            List<ProjectTitleDTO> result = new List<ProjectTitleDTO>();
            try
            {
                result = ProjectRepository.Instance.projectTitleList(searchkey);
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
                Data = result
            });
        }
        public IHttpActionResult GetProject(int take, int from, string categoryid, string orderby, string searchkey, string status, string isExprired, string isFunded)
        {

            List<ProjectBasicViewDTO> result = null;
            try
            {
                result = ProjectRepository.Instance.GetProject(take, from, categoryid, orderby, searchkey, status, isExprired, isFunded);
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
                Data = result
            });
        }
        public IHttpActionResult GetProjectByCategory()
        {

            List<ProjectBasicViewDTO> result = null;
            try
            {
                result = ProjectRepository.Instance.GetProjectByCategory();
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
                Data = result
            });
        }

        public IHttpActionResult GetProjectStatisticList()
        {

            List<List<ProjectBasicViewDTO>> result = null;
            try
            {
                result = ProjectRepository.Instance.GetProjectStatisticList();
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
                Data = result
            });
        }
        public IHttpActionResult GetStatisticListForHome()
        {
            Dictionary<string, List<ProjectBasicViewDTO>> result = null;
            try
            {
                result = ProjectRepository.Instance.GetStatisticListForHome();
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
                Data = result
            });
        }
        public IHttpActionResult getStatisticsInfor()
        {
            Dictionary<string, int> result = null;
            try
            {
                result = ProjectRepository.Instance.getStatisticsInfor();
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
                Data = result
            });
        }


        #endregion

        // GET: api/ProjectApi/GetProjectDetail?code=code
        [HttpGet]
        [ResponseType(typeof(ProjectDetailDTO))]
        public IHttpActionResult GetProjectDetail(string code)
        {
            ProjectDetailDTO projectDetail = null;
            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
                }


                // Get current user name.
                var currentUser = User.Identity != null ? User.Identity.Name : null;
                projectDetail = ProjectRepository.Instance.GetProjectByCode(code, currentUser);
                projectDetail.Question = ProjectRepository.Instance.GetQuestion(projectDetail.ProjectID);
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    projectDetail.Creator.IsOwner = false;
                    ProjectRepository.Instance.CaculateProjectPoint(code, DDLConstants.PopularPointType.ViewPoint);
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

        // GET: api/ProjectApi/GetCommentList?code=code
        [HttpGet]
        [ResponseType(typeof(CommentDTO))]
        public IHttpActionResult GetCommentList(string code, string lastDateTime = "")
        {
            List<CommentDTO> result = null;
            try
            {
                var datetime = !string.IsNullOrEmpty(lastDateTime) ? DateTime.Parse(lastDateTime) : CommonUtils.DateTimeNowGMT7();

                // Get current user name.
                var currentUser = User.Identity != null ? User.Identity.Name : null;
                result = ProjectRepository.Instance.GetListComment(code, datetime, currentUser);
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
                Data = result
            });
        }

        // GET: api/ProjectApi/GetUpdateLogList?code=code
        [HttpGet]
        [ResponseType(typeof(CommentDTO))]
        public IHttpActionResult GetUpdateLogList(string code)
        {
            List<UpdateLogDTO> result = null;
            try
            {
                // Get current user name.
                var currentUser = User.Identity != null ? User.Identity.Name : null;
                result = ProjectRepository.Instance.GetListUpdateLog(code, currentUser);
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
                Data = result
            });
        }


        //  17/10/2015 - MaiCTP - Get BAckedProject

        [HttpGet]
        [ResponseType(typeof(ProjectBasicViewDTO))]
        public IHttpActionResult GetBackedProject()
        {
            List<ProjectBasicViewDTO> listBacked = new List<ProjectBasicViewDTO>();

            try
            {
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

                listBacked = ProjectRepository.Instance.GetBackedProject(User.Identity.Name);


            }
            catch (ProjectNotFoundException)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Chưa ủng hộ dự án nào!", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Data = listBacked });
        }

        //  19/10/2015 - MaiCTP - Get BAckedProjectHistory

        [HttpGet]
        [ResponseType(typeof(ProjectBasicViewDTO))]
        public IHttpActionResult GetBackedProjectHistory()
        {
            List<ProjectBasicViewDTO> listBacked = new List<ProjectBasicViewDTO>();

            try
            {

                if (!ModelState.IsValid)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
                }

                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Chưa đăng nhập", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }

                listBacked = ProjectRepository.Instance.GetBackedProjectHistory(User.Identity.Name);
            }
            catch (ProjectNotFoundException)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Chưa ủng hộ dự án nào!", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Data = listBacked });
        }

        //  18/10/2015 - MaiCTP - Get StarredProject
        [HttpGet]
        [ResponseType(typeof(ProjectBasicViewDTO))]
        public IHttpActionResult GetStarredProject()
        {
            List<ProjectBasicViewDTO> listStarred = new List<ProjectBasicViewDTO>();

            try
            {

                if (!ModelState.IsValid)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
                }

                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Chưa đăng nhập", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }

                var ListStarred = ProjectRepository.Instance.GetStarredProject(User.Identity.Name);
                listStarred = ListStarred;

            }
            catch (ProjectNotFoundException)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Chưa theo dõi dự án nào!", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Data = listStarred });
        }

        //  18/10/2015 - MaiCTP - Get CreatedProject
        [HttpGet]
        [ResponseType(typeof(ProjectBasicViewDTO))]
        public IHttpActionResult GetCreatedProject()
        {
            List<ProjectBasicViewDTO> listCreated = new List<ProjectBasicViewDTO>();

            try
            {

                if (!ModelState.IsValid)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
                }

                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Chưa đăng nhập", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }

                listCreated = ProjectRepository.Instance.GetCreatedProject(User.Identity.Name);


            }
            catch (ProjectNotFoundException)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Chưa tạo dự án nào!", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Data = listCreated });
        }

        //  24/10/2015 - MaiCTP - DeleteProjectDraft

        [ResponseType(typeof(ProjectBasicViewDTO))]
        [HttpDelete]
        public IHttpActionResult DeleteProjectDraft(int id)
        {
            try
            {
                // Check authen.
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }

                var result = ProjectRepository.Instance.DeleteProjectDraft(id);

            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "" });
        }

        //  21/10/2015 - MaiCTP - DeleteProjectReminded

        [ResponseType(typeof(ProjectBasicViewDTO))]
        [HttpDelete]
        public IHttpActionResult DeleteProjectReminded(int id)
        {
            try
            {
                // Check authen.
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }

                var result = ProjectRepository.Instance.DeleteProjectReminded(id);

            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "" });
        }


        //  24/10/2015 - MaiCTP - Get BackingInfo
        [HttpGet]
        [ResponseType(typeof(BackingInfoDTO))]
        public IHttpActionResult GetBackingInfo(string projectCode)
        {
            List<BackingInfoDTO> listBacking = new List<BackingInfoDTO>();

            try
            {

                if (!ModelState.IsValid)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
                }

                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Chưa đăng nhập", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }

                listBacking = ProjectRepository.Instance.BackingInfo(projectCode);
            }
            catch (ProjectNotFoundException)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Missing Backing information", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Data = listBacking });
        }

        [HttpGet]
        public IHttpActionResult RemindProject(string code)
        {
            bool Reminded;
            try
            {

                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                } 
                Reminded = ProjectRepository.Instance.RemindProject(User.Identity.Name, code);
            }
            catch (UserNotFoundException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Bạn chưa đăng nhập!", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }
            if (Reminded == true)
            {
                return Ok(new HttpMessageDTO { Status = "success", Message = "reminded", Type = "" });
            }
            return Ok(new HttpMessageDTO { Status = "success", Message = "notremind", Type = "" });
        }

        [HttpGet]
        public IHttpActionResult GetListBacker(string code)
        {
            var listBacker = new BackingDTO();
            try
            {
                listBacker = ProjectRepository.Instance.GetListBacker(code);
            }
            catch (UserNotFoundException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Bạn chưa đăng nhập!", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            return Ok(new HttpMessageDTO { Status = "success", Message = "", Type = "", Data = listBacker });
        }


        #region Comment Functions

        /// <summary>
        /// Post: api/ProjectApi/Comment?projectCode=xxx
        /// </summary>
        /// <param name="projectCode">projectCode</param>
        /// <param name="comment">CommentDTO</param>
        /// <param name="lastDateTime"></param>
        /// <returns>CommentDTO</returns>
        [HttpPost]
        [ResponseType(typeof(CommentDTO))]
        public IHttpActionResult Comment(string projectCode, CommentDTO comment, string lastDateTime = "")
        {
            List<CommentDTO> result = null;
            try
            {
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }

                var datetime = !string.IsNullOrEmpty(lastDateTime) ? DateTime.Parse(lastDateTime) : CommonUtils.DateTimeNowGMT7();
                result = ProjectRepository.Instance.AddComment(projectCode, comment, datetime);
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

            try
            {
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }

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

            try
            {
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }

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

            try
            {
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }

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