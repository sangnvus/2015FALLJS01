using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using DDL_CapstoneProject.Helper;
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

        // PUT: api/ProjectApi/Edit  
        [ResponseType(typeof(ProjectEditDTO))]
        [HttpPut]
        public IHttpActionResult EditProject(int ProjectID, ProjectEditDTO project)
        {
            if (project == null)
            {
                return Ok(new HttpMessageDTO { Status = "error", Message = "", Type = "Bad-Request" });
            }

            var updateProject = ProjectRepository.Instance.EditProject(ProjectID, project);

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "" });
        }

        // GET: api/ProjectApi/GetProject/:id
        [HttpGet]
        [ResponseType(typeof(ProjectEditDTO))]
        public IHttpActionResult GetProject(int id)
        {
            var project = ProjectRepository.Instance.GetProject(id);

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Data = project });
        }

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