using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using DDL_CapstoneProject.Helper;
using DDL_CapstoneProject.Models.DTOs;
using DDL_CapstoneProject.Respository;
using DDL_CapstoneProject.Ultilities;

namespace DDL_CapstoneProject.Controllers.ApiControllers
{
    public class UserApiController : BaseApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>
        [ResponseType(typeof(UserRegisterDTO))]
        [HttpPost]
        public IHttpActionResult Register(UserRegisterDTO newUser)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new HttpMessageDTO{Status = "error", Message = "", Type = "Bad-Request"});
            }
            try
            {
                var user = UserRepository.Instance.Register(newUser);
            }
            catch (DuplicateUserNameException)
            {
                return Ok(new HttpMessageDTO { Status = "error", Message = "Tên tài khoản đã được sử dụng!", Type = "DuplicateUserName" });
            }
            catch (DuplicateEmailException)
            {
                return Ok(new HttpMessageDTO { Status = "error", Message = "Email này đã được sử dụng!", Type = "DuplicateEmail" });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = "error", Message = "", Type = "Bad-Request" });
            }
            return Ok(new HttpMessageDTO { Status = "success", Message = "", Type = "" });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ResetPassword(string email)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new HttpMessageDTO { Status = "error", Message = "", Type = "Bad-Request" });
            }
            try
            {
                var result = UserRepository.Instance.ResetPassword(email);
            }
            catch (UserNotFoundException)
            {
                return Ok(new HttpMessageDTO { Status = "error", Message = "Tài khoản không tồn tại!", Type = "UserNotFound" });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = "error", Message = "", Type = "Bad-Request" });
            }

            return Ok(new HttpMessageDTO { Status = "success", Message = "", Type = "" });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetListUserName(string username)
        {
            var listUserName = new List<UserNameDTO>();
            if (string.IsNullOrEmpty(username))
            {
                Ok(new HttpMessageDTO { Status = "success", Message = "", Type = "", Data = listUserName});
            }
            try
            {
                listUserName = UserRepository.Instance.GetListUserName(username);
            }
            catch (UserNotFoundException)
            {
                return Ok(new HttpMessageDTO { Status = "error", Message = "Tài khoản không tồn tại!", Type = "UserNotFound" });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = "error", Message = "", Type = "Bad-Request" });
            }

            return Ok(new HttpMessageDTO { Status = "success", Message = "", Type = "", Data = listUserName});
        }

        /// <summary>
        /// Function check login status
        /// </summary>
        /// <returns>user info</returns>
        [HttpGet]
        public IHttpActionResult CheckLoginStatus()
        {
            UserBasicInfoDTO currentUser;
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return
                    Ok(new HttpMessageDTO
                    {
                        Status = DDLConstants.HttpMessageType.ERROR,
                        Message = "",
                        Type = DDLConstants.HttpMessageType.NOT_AUTHEN
                    });
            }

            try
            {
                currentUser = UserRepository.Instance.GetBasicInfo(User.Identity.Name);
            }
            catch (Exception)
            {

                throw;
            }

            return Ok(new HttpMessageDTO
            {
                Status = DDLConstants.HttpMessageType.SUCCESS,
                Message = "",
                Type = "",
                Data = currentUser
            });
        }
    }
}
