using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DDL_CapstoneProject.Models;
using DDL_CapstoneProject.Models.DTOs;
using DDL_CapstoneProject.Respository;

namespace DDL_CapstoneProject.Helper
{
    public class BaseController: Controller
    {
        protected DDL_User getCurrentUser()
        {
            DDL_User currentUser = null;
            if (User.Identity.IsAuthenticated)
            {
                currentUser = UserRepository.Instance.GetByUserNameOrEmail(User.Identity.Name);
                ViewBag.CurrentUser = new UserBasicInfoDTO
                {
                    FullName = currentUser.UserInfo.FullName,
                    ProfileImage = currentUser.UserInfo.ProfileImage,
                    LoginType = currentUser.LoginType,
                    Role = currentUser.UserType,
                    IsActive = currentUser.IsActive,
                    UserName = currentUser.Username
                };
            }

            return currentUser;
        }

        public string GetBaseUrl()
        {
            string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority +
            Request.ApplicationPath.TrimEnd('/') + "/";
            return baseUrl;
        }
    }
}