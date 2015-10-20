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
            var currentUser = getCurrentUser();

            if (currentUser == null)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Chưa đăng nhập!", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }

            if (!ModelState.IsValid)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            // Check authen.
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }

            //try
            //{
            newProject.CreatorID = currentUser.DDL_UserID;
            var project = ProjectRepository.Instance.CreatProject(newProject);
            //}
            //catch (Exception)
            //{
            //    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            //}
            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "", Data = project.ProjectCode });
        }

        // PUT: api/ProjectApi/EditProject  
        [ResponseType(typeof(ProjectEditDTO))]
        [HttpPost]
        public IHttpActionResult EditProjectBasic()
        {
            // Check authen.
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }

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
                if (updateProject.ImageUrl != string.Empty)
                {
                    updateProject.ImageUrl = DDLConstants.FileType.PROJECT + updateProject.ImageUrl;
                }
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

            if (project == null)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            // Check authen.
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }

            try
            {
                updateProject = ProjectRepository.Instance.EditProjectStory(project);
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
                project = ProjectRepository.Instance.GetProjectBasic(code, currentUser.DDL_UserID, currentUser.UserType);
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
            // Check authen.
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }

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
            // Check authen.
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }

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

        // GET: api/ProjectApi/GetRewardPkgByCode/:code
        [HttpGet]
        [ResponseType(typeof(RewardPkgDTO))]
        public IHttpActionResult GetRewardPkgByCode(string code)
        {
            // Check authen.
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }

            List<RewardPkgDTO> rewardPkg;

            try
            {
                rewardPkg = RewardPkgRepository.Instance.GetRewardPkgByCode(code);
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

            if (!ModelState.IsValid)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            // Check authen.
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
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
            // Check authen.
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }

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
            // Check authen.
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
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
            // Check authen.
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
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
            var updateLog = new UpdateLog();

            // Check authen.
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
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
            // Check authen.
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }

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
            // Check authen.
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }

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

        // GET: api/ProjectApi/GetTimeline/:id
        [HttpGet]
        [ResponseType(typeof(TimeLineDTO))]
        public IHttpActionResult GetTimeLine(int id)
        {
            // Check authen.
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }

            List<TimeLineDTO> timeline;

            try
            {
                timeline = TimeLineRepository.Instance.GetTimeLine(id);
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
            // Check authen.
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }

            var httpRequest = HttpContext.Current.Request;

            TimeLineDTO newTimeLine = null;

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

            try
            {
                newTimeLine = TimeLineRepository.Instance.CreateTimeline(id, timeline, uploadImageName);
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
            // Check authen.
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }

            var httpRequest = HttpContext.Current.Request;

            bool updateTimeLine = false;

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

            try
            {
                updateTimeLine = TimeLineRepository.Instance.EditTimeline(timeline, uploadImageName);
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
            // Check authen.
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }

            try
            {
                var result = TimeLineRepository.Instance.DeleteTimeline(id);

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
            // Check authen.
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }

            List<QuestionDTO> questionList;

            try
            {
                questionList = QuestionRepository.Instance.GetQuestion(id);
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
            // Check authen.
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }

            var newQA = new QuestionDTO();

            if (!ModelState.IsValid)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            try
            {
                newQA = QuestionRepository.Instance.CreateQuestion(id, newQuestion);
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
            // Check authen.
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }

            bool result = false;
            if (question == null)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            try
            {
                result = QuestionRepository.Instance.EditQuestion(question);
            }
            catch (Exception)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "" });
        }

        // Delete: api/ProjectApi/DeleteQuestion/:id 
        [ResponseType(typeof(QuestionDTO))]
        [HttpDelete]
        public IHttpActionResult DeleteQuestion(int id)
        {
            // Check authen.
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }

            try
            {
                var result = QuestionRepository.Instance.DeleteQuestion(id);

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
            // Check authen.
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }

            List<string> errorList = null;

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

            try
            {
                errorList = ProjectRepository.Instance.SubmitProject(project);
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
            catch (Exception)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = DDLConstants.HttpMessageType.SUCCESS, Type = "", Data = project });
        }
        #endregion


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

        // GET: api/ProjectApi/GetCommentList?code=code
        [HttpGet]
        [ResponseType(typeof(CommentDTO))]
        public IHttpActionResult GetCommentList(string code, string lastDateTime = "")
        {
            List<CommentDTO> result = null;
            var datetime = !string.IsNullOrEmpty(lastDateTime) ? DateTime.Parse(lastDateTime) : DateTime.Now;
            try
            {
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
        //public IHttpActionResult GetBackedProject()
        //{
        //    var listBacked = ProjectRepository.Instance.GetBackedProject(User.Identity.Name);
        //    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Data = listBacked });
        //}

        [HttpGet]
        [ResponseType(typeof(ProjectBasicViewDTO))]
        public IHttpActionResult GetBackedProject()
        {
            List<ProjectBasicViewDTO> listBacked = new List<ProjectBasicViewDTO>();

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
                var ListBacked = ProjectRepository.Instance.GetBackedProject(User.Identity.Name);
                listBacked = ListBacked;
                
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
                var ListCreated = ProjectRepository.Instance.GetCreatedProject(User.Identity.Name);
                listCreated = ListCreated;

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

        [HttpGet]
        public IHttpActionResult RemindProject(string code)
        {
            var currentUser = getCurrentUser();

            if (currentUser == null)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Chưa đăng nhập!", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }
            var httpRequest = HttpContext.Current.Request;
            //RemindDTO remindInfo = null;

            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }
            try
            {
                ProjectRepository.Instance.RemindProject(User.Identity.Name, code);
            }
            catch (UserNotFoundException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Bạn chưa đăng nhập!", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            return Ok(new HttpMessageDTO { Status = "success", Message = "", Type = "" });
        }

        [HttpGet]
        public IHttpActionResult GetListBacker(string code)
        {
            var listBacker = new List<BackingDTO>();
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }
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