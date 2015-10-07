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

namespace DDL_CapstoneProject.Controllers.ApiControllers
{
    public class UserApiController : BaseApiController
    {
        // POST: api/AccountsApi/CreateAccount
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
    }
}
