using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using DDL_CapstoneProject.Helper;
using DDL_CapstoneProject.Models.DTOs;
using DDL_CapstoneProject.Respository;
using DDL_CapstoneProject.Ultilities;
using System.Web;
using System.Web.Script.Serialization;

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
            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
                }
                var user = UserRepository.Instance.Register(newUser);
            }
            catch (DuplicateUserNameException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Tên tài khoản đã được sử dụng!", Type = "DuplicateUserName" });
            }
            catch (DuplicateEmailException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Email này đã được sử dụng!", Type = "DuplicateEmail" });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }
            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "" });
        }

        /// <summary>
        /// Api reset password
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult ResetPassword(string email, string code)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
                }
                var result = UserRepository.Instance.ResetPassword(email, code);
            }
            catch (UserNotFoundException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Tài khoản không tồn tại!", Type = "UserNotFound" });
            }
            catch (InvalidDataException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Mã code không đúng", Type = "wrong-code" });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "" });
        }

        /// <summary>
        /// SendCodeResetPassword
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SendCodeResetPassword(string email)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
                }
                var result = UserRepository.Instance.SendCodeResetPassword(email);
            }
            catch (UserNotFoundException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Tài khoản không tồn tại!", Type = "UserNotFound" });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "" });
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
            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
                }
                if (string.IsNullOrEmpty(username))
                {
                    Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = "", Data = listUserName });
                }
                // Check authen.
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }
                listUserName = UserRepository.Instance.GetListUserName(username);
                if (listUserName.Any(x => x.UserName == User.Identity.Name))
                {
                    listUserName.Remove(listUserName.FirstOrDefault(x => x.UserName == User.Identity.Name));
                }
            }
            catch (UserNotFoundException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Tài khoản không tồn tại!", Type = "UserNotFound" });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "", Data = listUserName });
        }

        /// <summary>
        /// Function check login status
        /// </summary>
        /// <returns>user info</returns>
        [HttpGet]
        public IHttpActionResult CheckLoginStatus()
        {
            UserBasicInfoDTO currentUser;

            try
            {
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
                currentUser = UserRepository.Instance.GetBasicInfo(User.Identity.Name);
            }
            catch (Exception)
            {

                return Ok(new HttpMessageDTO
                {
                    Status = DDLConstants.HttpMessageType.ERROR,
                    Message = "",
                    Type = DDLConstants.HttpMessageType.BAD_REQUEST
                });
            }

            return Ok(new HttpMessageDTO
            {
                Status = DDLConstants.HttpMessageType.SUCCESS,
                Message = "",
                Type = "",
                Data = currentUser
            });
        }

        [HttpGet]
        public IHttpActionResult GetPublicInfo(string username)
        {
            UserPublicInfoDTO userExist;
            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
                }
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }
                userExist = UserRepository.Instance.GetUserPublicInfo(username);
            }
            catch (UserNotFoundException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Không tìm thấy!", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            //userExist.ProfileImage = DDLConstants.FileType.AVATAR + userExist.ProfileImage;
            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "", Data = userExist });
        }

        public IHttpActionResult GetUserInfoEdit()
        {
            UserEditInfoDTO userCurrent = null;
            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
                }

                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }
                userCurrent = UserRepository.Instance.GetUserEditInfo(User.Identity.Name);
                userCurrent.ProfileImage = DDLConstants.FileType.AVATAR + userCurrent.ProfileImage;
            }
            catch (UserNotFoundException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Bạn chưa đăng nhập!", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "", Data = userCurrent });
        }

        public IHttpActionResult EditUserInfo()
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;
                var userCurrentJson = httpRequest.Form["profile"];
                var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
                var userCurrent = serializer.Deserialize<UserEditInfoDTO>(userCurrentJson);
                if (!ModelState.IsValid)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
                }
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }
                string imageName = "img_" + userCurrent.UserName;
                var file = httpRequest.Files["file"];
                var uploadImageName = CommonUtils.UploadImage(file, imageName, DDLConstants.FileType.AVATAR);
                UserRepository.Instance.EditUserInfo(userCurrent, uploadImageName);
            }
            catch (UserNotFoundException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Bạn chưa đăng nhập!", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }
            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "" });
        }

        public IHttpActionResult GetUserPasswordEdit()
        {

            EditPasswordDTO userPass = null;
            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
                }
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }
                userPass = UserRepository.Instance.GetUserPassword(User.Identity.Name);
            }
            catch (UserNotFoundException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Bạn chưa đăng nhập!", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }
            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "", Data = userPass });
        }


        public IHttpActionResult ChangePassword(EditPasswordDTO newPass)
        {
            EditPasswordDTO userPass;
            try
            {
                userPass = UserRepository.Instance.GetUserPassword(User.Identity.Name);
                if (!ModelState.IsValid)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
                }
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }
                var checkpass = UserRepository.Instance.ChangePassword(User.Identity.Name, newPass);
                if (checkpass == false)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
                }
            }
            catch (UserNotFoundException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Bạn chưa đăng nhập!", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }
            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "", Data = userPass });
        }

        public IHttpActionResult GetUserBackedProjectForAdmin(string Username)
        {
            List<AdminUserBackedListDTO> projectBackedList;
            // Check authen.
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }
            try
            {
                // Check role user.
                var currentUser = UserRepository.Instance.GetBasicInfo(User.Identity.Name);
                if (currentUser == null || currentUser.Role != DDLConstants.UserType.ADMIN)
                {
                    throw new NotPermissionException();
                }
                projectBackedList = UserRepository.Instance.GetUserBackedProjectForAdmin(Username);
            }
            catch (KeyNotFoundException)
            {
                return
                    Ok(new HttpMessageDTO
                    {
                        Status = DDLConstants.HttpMessageType.ERROR,
                        Message = "Không tìm thấy!",
                        Type = DDLConstants.HttpMessageType.NOT_FOUND
                    });
            }
            catch (NotPermissionException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Không có quyền truy cập!", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }
            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "", Data = projectBackedList });
        }

        public IHttpActionResult GetUserCreatedProjectForAdmin(string Username)
        {
            List<AdminUserCreatedListDTO> projectCreatedList;
            // Check authen.
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }
            try
            {
                // Check role user.
                var currentUser = UserRepository.Instance.GetBasicInfo(User.Identity.Name);
                if (currentUser == null || currentUser.Role != DDLConstants.UserType.ADMIN)
                {
                    throw new NotPermissionException();
                }
                projectCreatedList = UserRepository.Instance.GetUserCreatedProjectForAdmin(Username);
            }
            catch (KeyNotFoundException)
            {
                return
                    Ok(new HttpMessageDTO
                    {
                        Status = DDLConstants.HttpMessageType.ERROR,
                        Message = "Không tìm thấy!",
                        Type = DDLConstants.HttpMessageType.NOT_FOUND
                    });
            }
            catch (NotPermissionException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Không có quyền truy cập!", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }
            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "", Data = projectCreatedList });
        }


        public IHttpActionResult GetUserBackingDetailForAdmin(string Username)
        {
            AdminUserBackingDetailDTO BackingDetail;
            // Check authen.
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }
            try
            {
                // Check role user.
                var currentUser = UserRepository.Instance.GetBasicInfo(User.Identity.Name);
                if (currentUser == null || currentUser.Role != DDLConstants.UserType.ADMIN)
                {
                    throw new NotPermissionException();
                }
                BackingDetail = UserRepository.Instance.GetUseBackingDetailForAdmin(Username);
            }
            catch (KeyNotFoundException)
            {
                return
                    Ok(new HttpMessageDTO
                    {
                        Status = DDLConstants.HttpMessageType.ERROR,
                        Message = "Không tìm thấy!",
                        Type = DDLConstants.HttpMessageType.NOT_FOUND
                    });
            }
            catch (NotPermissionException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Không có quyền truy cập!", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }
            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "", Data = BackingDetail });
        }

        public IHttpActionResult GetUserDashboardForAdmin()
        {
            AdminUserDashboardDTO DashboardReturn;
            // Check authen.
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }
            try
            {
                // Check role user.
                var currentUser = UserRepository.Instance.GetBasicInfo(User.Identity.Name);
                if (currentUser == null || currentUser.Role != DDLConstants.UserType.ADMIN)
                {
                    throw new NotPermissionException();
                }
                DashboardReturn = UserRepository.Instance.GetUserDashboardForAdmin();
            }
            catch (KeyNotFoundException)
            {
                return
                    Ok(new HttpMessageDTO
                    {
                        Status = DDLConstants.HttpMessageType.ERROR,
                        Message = "Không tìm thấy!",
                        Type = DDLConstants.HttpMessageType.NOT_FOUND
                    });
            }
            catch (NotPermissionException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Không có quyền truy cập!", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }
            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "", Data = DashboardReturn });
        }

        public IHttpActionResult GetUserListForAdmin()
        {
            AdminUserListDTO userList;
            // Check authen.
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }
            try
            {
                // Check role user.
                var currentUser = UserRepository.Instance.GetBasicInfo(User.Identity.Name);
                if (currentUser == null || currentUser.Role != DDLConstants.UserType.ADMIN)
                {
                    throw new NotPermissionException();
                }
                userList = UserRepository.Instance.GetUserListForAdmin();
            }
            catch (KeyNotFoundException)
            {
                return
                    Ok(new HttpMessageDTO
                    {
                        Status = DDLConstants.HttpMessageType.ERROR,
                        Message = "Không tìm thấy!",
                        Type = DDLConstants.HttpMessageType.NOT_FOUND
                    });
            }
            catch (NotPermissionException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Không có quyền truy cập!", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }
            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "", Data = userList });
        }

        public IHttpActionResult GetUserProfileForAdmin(string Username)
        {
            AdminUserProfileDTO userList;
            // Check authen.
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }
            try
            {
                // Check role user.
                var currentUser = UserRepository.Instance.GetBasicInfo(User.Identity.Name);
                if (currentUser == null || currentUser.Role != DDLConstants.UserType.ADMIN)
                {
                    throw new NotPermissionException();
                }
                userList = UserRepository.Instance.GetUserProfileForAdmin(Username);
            }
            catch (KeyNotFoundException)
            {
                return
                    Ok(new HttpMessageDTO
                    {
                        Status = DDLConstants.HttpMessageType.ERROR,
                        Message = "Không tìm thấy!",
                        Type = DDLConstants.HttpMessageType.NOT_FOUND
                    });
            }
            catch (NotPermissionException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Không có quyền truy cập!", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }
            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "", Data = userList });
        }

        [HttpPut]
        public IHttpActionResult ChangeUserStatus(string Username)
        {

            // Check authen.
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }
            try
            {
                // Check role user.
                var currentUser = UserRepository.Instance.GetBasicInfo(User.Identity.Name);
                if (currentUser == null || currentUser.Role != DDLConstants.UserType.ADMIN)
                {
                    throw new NotPermissionException();
                }
                UserRepository.Instance.ChangeUserStatus(Username);
            }
            catch (KeyNotFoundException)
            {
                return
                    Ok(new HttpMessageDTO
                    {
                        Status = DDLConstants.HttpMessageType.ERROR,
                        Message = "Không tìm thấy!",
                        Type = DDLConstants.HttpMessageType.NOT_FOUND
                    });
            }
            catch (NotPermissionException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Không có quyền truy cập!", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }
            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "" });
        }

        [HttpGet]
        public IHttpActionResult GetListBackingForAdmin()
        {
            List<AdminBackingListDTO> backingList = new List<AdminBackingListDTO>();
            // Check authen.
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }
            try
            {
                // Check role user.
                var currentUser = UserRepository.Instance.GetBasicInfo(User.Identity.Name);
                if (currentUser == null || currentUser.Role != DDLConstants.UserType.ADMIN)
                {
                    throw new NotPermissionException();
                }
                backingList = UserRepository.Instance.GetBackingListForAdmin();
            }
            catch (KeyNotFoundException)
            {
                return
                    Ok(new HttpMessageDTO
                    {
                        Status = DDLConstants.HttpMessageType.ERROR,
                        Message = "Không tìm thấy!",
                        Type = DDLConstants.HttpMessageType.NOT_FOUND
                    });
            }
            catch (NotPermissionException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Không có quyền truy cập!", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }
            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "", Data = backingList });
        }

        public IHttpActionResult GetBackerForAdmin(string userName, int backingID)
        {
            AdminBackingListDTO backingList = new AdminBackingListDTO();
            // Check authen.
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }
            try
            {
                // Check role user.
                var currentUser = UserRepository.Instance.GetBasicInfo(User.Identity.Name);
                if (currentUser == null || currentUser.Role != DDLConstants.UserType.ADMIN)
                {
                    throw new NotPermissionException();
                }
                backingList = UserRepository.Instance.GetBackerForAdmin(userName, backingID);
            }
            catch (KeyNotFoundException)
            {
                return
                    Ok(new HttpMessageDTO
                    {
                        Status = DDLConstants.HttpMessageType.ERROR,
                        Message = "Không tìm thấy!",
                        Type = DDLConstants.HttpMessageType.NOT_FOUND
                    });
            }
            catch (NotPermissionException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Không có quyền truy cập!", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }
            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "", Data = backingList });
        }


        #region TrungVn

        /// <summary>
        /// /api/UserApi/GetUserTop/?categoryID=xxx
        /// </summary>
        /// <param name="categoryid">categoryid</param>
        /// <returns>Dictionary</returns>
        [HttpGet]
        public IHttpActionResult GetUserTop(string categoryid)
        {
            Dictionary<string, List<UserBackInforDTO>> listGetUserTop;
            // Check authen.
            try
            {
                listGetUserTop = UserRepository.Instance.GetUserTop(categoryid);
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }
            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "", Data = listGetUserTop });

        }
        #endregion

        public IHttpActionResult GetBackedUserInfo()
        {
            UserBackedInfoDTO userCurrent = null;
            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
                }

                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }
                userCurrent = UserRepository.Instance.GetBackedUserInfo(User.Identity.Name);
               // userCurrent.ProfileImage = DDLConstants.FileType.AVATAR + userCurrent.ProfileImage;
            }
            catch (UserNotFoundException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Bạn chưa đăng nhập!", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "", Data = userCurrent });
        }

        #region HuyNM
        // GET: api/UserApi/AdminGetTopBacker/
        [HttpGet]
        [ResponseType(typeof(TopBackerDTO))]
        public IHttpActionResult AdminGetTopBacker()
        {
            var topBacker = new List<TopBackerDTO>();

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

                topBacker = UserRepository.Instance.AdminGetTopBacker();
            }
            catch (Exception)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Data = topBacker });
        }

        // GET: api/UserApi/AdminGetRecentUser/
        [HttpGet]
        [ResponseType(typeof(RecentUserDTO))]
        public IHttpActionResult AdminGetRecentUser()
        {
            var recentUser = new List<RecentUserDTO>();

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

                recentUser = UserRepository.Instance.AdminGetRecentUser();
            }
            catch (Exception)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Data = recentUser });
        }

        // GET: api/UserApi/AdminGetRecentBacking/
        [HttpGet]
        [ResponseType(typeof(AdminBackingListDTO))]
        public IHttpActionResult AdminGetRecentBacking()
        {
            var recentBacking = new List<AdminBackingListDTO>();

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

                recentBacking = UserRepository.Instance.GetBackingListForAdmin();
                recentBacking = recentBacking.OrderByDescending(x => x.BackedDate).Take(5).ToList();
            }
            catch (Exception)
            {

                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Data = recentBacking });
        }
        #endregion

    }
}
