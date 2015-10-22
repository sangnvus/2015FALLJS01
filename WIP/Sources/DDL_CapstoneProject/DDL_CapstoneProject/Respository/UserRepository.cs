using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using DDL_CapstoneProject.Helper;
using DDL_CapstoneProject.Helpers;
using DDL_CapstoneProject.Models;
using DDL_CapstoneProject.Models.DTOs;
using DDL_CapstoneProject.Ultilities;
using Microsoft.Ajax.Utilities;

namespace DDL_CapstoneProject.Respository
{
    public class UserRepository : SingletonBase<UserRepository>
    {
        // data context.
        private DDLDataContext db;

        #region "Contructors"
        private UserRepository()
        {
            db = new DDLDataContext();
        }
        #endregion

        #region "Methods"
        #region TrungVN

        public Dictionary<string, List<UserBackInforDTO>> GetUserTop(string categoryid)
        {
            categoryid = "|" + categoryid + "|";
            bool allCategory = false;
            if (categoryid.ToLower().Contains("all")) allCategory = true;
            var UserTop = from user in db.DDL_Users
                          select new UserBackInforDTO
                          {
                              Rank = "Rank A",
                              Name = user.UserInfo.FullName,
                              TotalFunded = user.CreatedProjects.Where(x => categoryid.Contains(x.CategoryID.ToString()) || allCategory).Sum(x => (decimal?)x.CurrentFunded) ?? 0,
                              TotalBacked = user.Backings.Where(x => categoryid.Contains(x.Project.CategoryID.ToString()) || allCategory).Sum(x => (decimal?)x.BackingDetail.PledgedAmount) ?? 0
                          };
            int count = UserTop.Count();
            if (count >= 10) count = 10;
            Dictionary<string, List<UserBackInforDTO>> dic = new Dictionary<string, List<UserBackInforDTO>>();
            dic.Add("UserTopBack", UserTop.Where(x => x.TotalBacked > 0).Take(count).OrderByDescending(x => x.TotalBacked).ThenByDescending(x => x.TotalFunded).ToList());
            dic.Add("UserTopFund", UserTop.Where(x => x.TotalFunded > 0).Take(count).OrderByDescending(x => x.TotalFunded).ThenByDescending(x => x.TotalBacked).ToList());
            return dic;
        }

        #endregion
        public DDL_User CreateEmptyUser()
        {
            var user = new DDL_User
            {
                Username = string.Empty,
                Email = string.Empty,
                CreatedDate = DateTime.UtcNow,
                IsActive = false,
                IsVerify = false,
                VerifyCode = string.Empty,
                UserType = DDLConstants.UserType.USER,
                LoginType = DDLConstants.LoginType.NORMAL,
                Password = string.Empty,
                LastLogin = null,
                UserInfo = new UserInfo
                {
                    FullName = string.Empty,
                    Address = string.Empty,
                    Biography = string.Empty,
                    Country = string.Empty,
                    FacebookUrl = string.Empty,
                    Gender = DDLConstants.Gender.MALE,
                    ProfileImage = string.Empty,
                    Website = string.Empty,
                }
            };

            return user;
        }

        public string GenerateNewPassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var newPassword = new string(
                Enumerable.Repeat(chars, 8)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());

            return newPassword;
        }

        public DDL_User GetByUserNameOrEmail(string userNameOrEmail, string password)
        {
            var user =
                db.DDL_Users.FirstOrDefault(x => ((x.Username == userNameOrEmail || x.Email == userNameOrEmail) && x.Password == password));

            return user;
        }

        public DDL_User GetByUserNameOrEmail(string userNameOrEmail)
        {
            var user =
                db.DDL_Users.FirstOrDefault(x => x.Username == userNameOrEmail || x.Email == userNameOrEmail);

            return user;
        }

        public UserBasicInfoDTO GetBasicInfo(string userNameOrEmail)
        {
            var currentUser = (from user in db.DDL_Users
                               where user.Username.Equals(userNameOrEmail) || user.Email.Equals(userNameOrEmail)
                               select new UserBasicInfoDTO
                               {
                                   FullName = user.UserInfo.FullName,
                                   IsActive = user.IsActive,
                                   LoginType = user.LoginType,
                                   ProfileImage = user.UserInfo.ProfileImage,
                                   UserName = user.Username,
                                   Role = user.UserType
                               }).FirstOrDefault();

            return currentUser;
        }

        //public DDL_User GetByUserNameOrEmailByLoginType(string userNameOrEmail, string loginType)
        //{
        //    var user =
        //        db.DDL_Users.FirstOrDefault(x => (x.Username == userNameOrEmail || x.Email == userNameOrEmail) && x.LoginType == loginType);

        //    return user;
        //}

        public DDL_User InsertUser(DDL_User newUser)
        {
            try
            {
                db.DDL_Users.Add(newUser);
                db.SaveChanges();
            }
            catch (Exception)
            {

                return null;
            }

            return GetByUserNameOrEmail(newUser.Email); ;
        }

        public DDL_User RegisterFacebook(dynamic me)
        {
            // Create new User
            var newUser = new DDL_User
            {
                LoginType = DDLConstants.LoginType.FACEBOOK,
                Email = me.email,
                CreatedDate = DateTime.UtcNow,
                IsActive = true,
                Password = string.Empty,
                IsVerify = true,
                LastLogin = DateTime.UtcNow,
                UserType = DDLConstants.UserType.USER,
                Username = me.id,
                VerifyCode = string.Empty,
                UserInfo = new UserInfo
                {
                    Address = string.Empty,
                    FullName = me.name,
                    Biography = me.bio,
                    Gender = me.gender,
                    DateOfBirth = me.birthday,
                    FacebookUrl = me.link,
                    ProfileImage = "https://graph.facebook.com/" + me.id + "/picture?type=large",
                    Country = string.Empty,
                    Website = string.Empty,
                }
            };
            // Facebook account

            // insert user to Database
            newUser = InsertUser(newUser);

            return newUser;
        }

        public DDL_User Register(UserRegisterDTO newUser)
        {
            DDL_User newDLLUser;

            if (db.DDL_Users.Any(x => x.Username.Equals(newUser.Username, StringComparison.OrdinalIgnoreCase)))
            {
                throw new DuplicateUserNameException();
            }
            else if (db.DDL_Users.Any(x => x.Email.Equals(newUser.Email, StringComparison.OrdinalIgnoreCase)))
            {
                throw new DuplicateEmailException();
            }
            else
            {
                // Generate verify code.
                var verifyCode = CommonUtils.GenerateVerifyCode();

                // Insert data.
                newDLLUser = CreateEmptyUser();
                newDLLUser.Username = newUser.Username;
                newDLLUser.Password = newUser.Password;
                newDLLUser.Email = newUser.Email;
                newDLLUser.VerifyCode = verifyCode;
                newDLLUser.UserInfo.FullName = newUser.FullName;
                newDLLUser.UserInfo.ProfileImage = "avatar_default.png";
                db.DDL_Users.Add(newDLLUser);
                db.SaveChanges();

                // Send active link to email of user.
                MailHelper.Instance.SendMailActive(newUser.Email, newUser.Username, verifyCode, newUser.FullName);
            }

            return newDLLUser;
        }

        public bool VerifyAccount(string userName, string code)
        {
            var user = GetByUserNameOrEmail(userName);

            // Check code.
            if (user == null || !user.VerifyCode.Equals(code)) return false;

            // Update account status.
            user.IsActive = true;
            user.IsVerify = true;
            db.SaveChanges();

            return true;
        }

        public DDL_User UpdateUser(DDL_User user)
        {
            try
            {
                db.DDL_Users.AddOrUpdate(user);
                db.SaveChanges();
            }
            catch (Exception)
            {

                return null;
            }

            return GetByUserNameOrEmail(user.Email); ;
        }

        public bool ResetPassword(string email)
        {
            var user = GetByUserNameOrEmail(email);
            if (user == null || user.LoginType == DDLConstants.LoginType.FACEBOOK)
            {
                throw new UserNotFoundException();
            }

            string newPassword = GenerateNewPassword();
            user.Password = CommonUtils.Md5(newPassword);
            db.SaveChanges();

            MailHelper.Instance.SendMailResetPassword(email, newPassword, user.UserInfo.FullName);

            return true;
        }

        public List<UserNameDTO> GetListUserName(string username)
        {
            var listUserName = from user in db.DDL_Users
                               where user.Username.Contains(username) || user.UserInfo.FullName.Contains(username)
                               orderby user.Username
                               select new UserNameDTO
                               {
                                   UserName = user.Username,
                                   FullName = user.UserInfo.FullName
                               };

            return listUserName.ToList();
        }

        public UserPublicInfoDTO GetUserPublicInfo(string userName)
        {
            var userPublic = from user in db.DDL_Users
                             where user.Username == userName
                             select new UserPublicInfoDTO
                             {
                                 IsActive = user.IsActive,
                                 PhoneNumber = user.UserInfo.PhoneNumber,
                                 FullName = user.UserInfo.FullName,
                                 Biography = user.UserInfo.Biography,
                                 CreatedDate = user.CreatedDate,
                                 FacebookUrl = user.UserInfo.FacebookUrl,
                                 LastLogin = user.LastLogin,
                                 ProfileImage = user.UserInfo.ProfileImage,
                                 CountBackedProject = user.Backings.Count,
                                 CountCreatedProject = user.CreatedProjects.Count(x => x.Status != DDLConstants.ProjectStatus.DRAFT
                                             && x.Status != DDLConstants.ProjectStatus.REJECTED
                                             && x.Status != DDLConstants.ProjectStatus.PENDING),
                                 UserName = user.Username,
                                 Website = user.UserInfo.Website
                             };
            if (!userPublic.Any())
            {
                throw new UserNotFoundException();
            }

            var userPublicDTO = userPublic.FirstOrDefault();
            userPublicDTO.CreatedDate = CommonUtils.ConvertDateTimeFromUtc(userPublicDTO.CreatedDate);

            return userPublicDTO;
        }

        public UserEditInfoDTO GetUserEditInfo(string userName)
        {
            var userEdit = from user in db.DDL_Users
                           where user.Username == userName
                           select new UserEditInfoDTO
                           {
                               FullName = user.UserInfo.FullName,
                               Biography = user.UserInfo.Biography,
                               CreatedDate = user.CreatedDate,
                               FacebookUrl = user.UserInfo.FacebookUrl,
                               ProfileImage = user.UserInfo.ProfileImage,
                               UserName = user.Username,
                               DateOfBirth = user.UserInfo.DateOfBirth,
                               Addres = user.UserInfo.Address,
                               Email = user.Email,
                               Website = user.UserInfo.Website,
                               Gender = user.UserInfo.Gender,
                               ContactNumber = user.UserInfo.PhoneNumber,
                           };
            return userEdit.First();
        }


        //UserEditInfoDTO
        public void EditUserInfo(UserEditInfoDTO userCurrent, string uploadImageName)
        {
            var userEdit = db.DDL_Users.FirstOrDefault(x => x.Username.Equals(userCurrent.UserName)).UserInfo;
            if (uploadImageName != string.Empty)
            {
                userEdit.ProfileImage = uploadImageName;
            }
            userEdit.FullName = userCurrent.FullName;
            userEdit.FacebookUrl = userCurrent.FacebookUrl;
            userEdit.Website = userCurrent.Website;
            userEdit.DateOfBirth = userCurrent.DateOfBirth;
            userEdit.Biography = userCurrent.Biography;
            userEdit.Address = userCurrent.Addres;
            userEdit.Gender = userCurrent.Gender;
            userEdit.PhoneNumber = userCurrent.ContactNumber;

            db.SaveChanges();
        }

        public EditPasswordDTO GetUserPassword(string userName)
        {
            var userPublic = from user in db.DDL_Users
                             where user.Username == userName
                             select new EditPasswordDTO
                             {
                                 //CurrentPassword = user.Password,
                                 Email = user.Email,
                                 LoginType = user.LoginType
                             };
            if (!userPublic.Any())
            {
                throw new UserNotFoundException();
            }

            return userPublic.FirstOrDefault();
        }

        public Boolean ChangePassword(string userName, EditPasswordDTO newPass)
        {
            var userCurrent = db.DDL_Users.FirstOrDefault(x => x.Username.Equals(userName));
            if (userCurrent.Password == newPass.CurrentPassword)
            {
                userCurrent.Password = newPass.NewPassword;
                db.SaveChanges();
            }
            else
            {
                return false;
            }
            return true;
        }

        #endregion
    }
}