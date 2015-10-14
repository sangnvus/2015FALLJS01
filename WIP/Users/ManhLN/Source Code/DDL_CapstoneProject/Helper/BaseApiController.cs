using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using DDL_CapstoneProject.Models;
using DDL_CapstoneProject.Models.DTOs;
using DDL_CapstoneProject.Respository;

namespace DDL_CapstoneProject.Helper
{
    public class BaseApiController : ApiController
    {
        protected DDL_User getCurrentUser()
        {
            DDL_User currentUser = null;
            if (User.Identity.IsAuthenticated)
            {
                currentUser = UserRepository.Instance.GetByUserNameOrEmail(User.Identity.Name);
            }

            return currentUser;
        }
    }
}