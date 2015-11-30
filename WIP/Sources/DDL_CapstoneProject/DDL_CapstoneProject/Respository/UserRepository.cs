using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
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

        #region "Contructors"
        private UserRepository()
        {
        }
        #endregion

        #region "Methods"
        #region TrungVN




        public List<AdminBakingFullInforDTO> GetBackingFullInforListForExport()
        {
            using (var db = new DDLDataContext())
            {
                //var listBacking = db.Backings.ToList();
                var listReturn = (from backing in db.Backings
                                  select new AdminBakingFullInforDTO
                                  {
                                      ProjectCode = backing.Project.ProjectCode,
                                      ProjectTitle = backing.Project.Title,

                                      RewardID = backing.BackingDetail.RewardPkgID,
                                      RewardDes = backing.BackingDetail.RewardPkg.Description,
                                      RewardEstimatedDelivery = backing.BackingDetail.RewardPkg.EstimatedDelivery.Value + ".",

                                      BackingID = backing.BackingID,
                                      BackingPledgeAmount = backing.BackingDetail.PledgedAmount,
                                      BackingQuantity = backing.BackingDetail.Quantity,
                                      BackingDes = backing.BackingDetail.Description,
                                      BackedDate = backing.BackedDate + ".",

                                      BackerName = backing.User.UserInfo.FullName,
                                      BackerUserName = backing.User.Username,
                                      BackerEmail = backing.BackingDetail.Email,
                                      BackerAddress = backing.BackingDetail.Address,
                                      BackerPhoneNumber = "'" + backing.BackingDetail.PhoneNumber,

                                  }).ToList();
                //foreach (var backing in listBacking)
                //{
                //    AdminBakingFullInforDTO backingReturn = new AdminBakingFullInforDTO
                //    {

                //    };
                //    listReturn.Add(backingReturn);
                //}
                return listReturn;
            }
        }


        public Dictionary<string, List<UserBackInforDTO>> GetUserTop(string categoryid)
        {
            using (var db = new DDLDataContext())
            {
                categoryid = "|" + categoryid + "|";
                bool allCategory = categoryid.ToLower().Contains("all");
                var userTopFunded = from user in db.DDL_Users
                                    select new UserBackInforDTO
                                    {
                                        Username = user.Username,
                                        Name = user.UserInfo.FullName,
                                        TotalFunded =
                                            user.CreatedProjects.Where(x => (categoryid.Contains(x.CategoryID.ToString()) || allCategory) && !x.Status.Equals(DDLConstants.ProjectStatus.DRAFT))
                                                .Sum(x => (decimal?)x.CurrentFunded) ?? 0,
                                        TotalBacked = 0,
                                        projectCount = user.CreatedProjects.Where(x => (categoryid.Contains(x.CategoryID.ToString()) || allCategory) && !x.Status.Equals(DDLConstants.ProjectStatus.DRAFT)).Count()
                                    };

                var userTopBacked = from user in db.DDL_Users
                                    select new UserBackInforDTO
                                    {
                                        Username = user.Username,
                                        Name = user.UserInfo.FullName,
                                        TotalFunded = 0,
                                        TotalBacked =
                                            user.Backings.Where(x => (categoryid.Contains(x.Project.CategoryID.ToString()) || allCategory) && !x.Project.Status.Equals(DDLConstants.ProjectStatus.DRAFT))
                                                .Sum(x => (decimal?)x.BackingDetail.PledgedAmount) ?? 0,
                                        projectCount =
                                            user.Backings.Where(x => (categoryid.Contains(x.Project.CategoryID.ToString()) || allCategory) && !x.Project.Status.Equals(DDLConstants.ProjectStatus.DRAFT)).Count()
                                    };
                Dictionary<string, List<UserBackInforDTO>> dic = new Dictionary<string, List<UserBackInforDTO>>
                {
                    {
                        "UserTopBack", userTopBacked.Where(x => x.TotalBacked > 0)
                            .Take(10)
                            .OrderByDescending(x => x.TotalBacked)
                            .ThenByDescending(x => x.projectCount)
                            .ToList()
                    },
                    {
                        "UserTopFund", userTopFunded.Where(x => x.TotalFunded > 0)
                            .Take(10)
                            .OrderByDescending(x => x.TotalFunded)
                            .ThenByDescending(x => x.projectCount)
                            .ToList()
                    }
                };
                return dic;
            }
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
            return GenerateRandomString(8);
        }
        public string GenerateResetCode()
        {
            return GenerateRandomString(6);
        }

        public string GenerateRandomString(int numberCharacter)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var randomString = new string(
                Enumerable.Repeat(chars, numberCharacter)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());

            return randomString;
        }

        public DDL_User GetByUserNameOrEmail(string userNameOrEmail, string password)
        {
            using (var db = new DDLDataContext())
            {
                var user =
                    db.DDL_Users.Include(x => x.UserInfo).FirstOrDefault(
                        x => ((x.Username == userNameOrEmail || x.Email == userNameOrEmail) && x.Password == password));

                return user;
            }
        }

        public DDL_User GetByUserNameOrEmail(string userNameOrEmail)
        {
            using (var db = new DDLDataContext())
            {
                var user =
                    db.DDL_Users.Include(x => x.UserInfo).FirstOrDefault(x => x.Username == userNameOrEmail || x.Email == userNameOrEmail);

                return user;
            }
        }

        public UserBasicInfoDTO GetBasicInfo(string userNameOrEmail)
        {
            using (var db = new DDLDataContext())
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
        }

        //public DDL_User GetByUserNameOrEmailByLoginType(string userNameOrEmail, string loginType)
        //{
        //    var user =
        //        db.DDL_Users.FirstOrDefault(x => (x.Username == userNameOrEmail || x.Email == userNameOrEmail) && x.LoginType == loginType);

        //    return user;
        //}

        public DDL_User InsertUser(DDL_User newUser)
        {
            using (var db = new DDLDataContext())
            {
                db.DDL_Users.Add(newUser);
                db.SaveChanges();
                return GetByUserNameOrEmail(newUser.Email);
            }
        }

        public DDL_User RegisterFacebook(dynamic me)
        {
            string email = me.email;
            // Create new User
            var newUser = new DDL_User
            {
                LoginType = DDLConstants.LoginType.FACEBOOK,
                Email = email,
                CreatedDate = DateTime.UtcNow,
                IsActive = true,
                Password = string.Empty,
                IsVerify = true,
                LastLogin = DateTime.UtcNow,
                UserType = DDLConstants.UserType.USER,
                Username = "fb." + email.Split(new string[] { "@" }, StringSplitOptions.None)[0],
                VerifyCode = string.Empty,
                UserInfo = new UserInfo
                {
                    Address = me.location,
                    FullName = me.name,
                    Biography = me.bio,
                    Gender = me.gender,
                    DateOfBirth = !string.IsNullOrEmpty(me.birthday) ? DateTime.ParseExact(me.birthday.ToString(), "MM/DD/YYYY", System.Globalization.CultureInfo.InvariantCulture) : null,
                    FacebookUrl = me.link,
                    ProfileImage = "https://graph.facebook.com/" + me.id + "/picture?type=large",
                    Country = string.Empty,
                    Website = me.website,
                    PhoneNumber = string.Empty
                }
            };
            // Facebook account

            // insert user to Database
            newUser = InsertUser(newUser);

            return newUser;
        }

        public DDL_User Register(UserRegisterDTO newUser)
        {
            using (var db = new DDLDataContext())
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
                    newDLLUser.UserInfo.ProfileImage = "avatar_default.jpg";
                    db.DDL_Users.Add(newDLLUser);
                    db.SaveChanges();

                    // Send active link to email of user.
                    MailHelper.Instance.SendMailActive(newUser.Email, newUser.Username, verifyCode, newUser.FullName);
                }

                return GetByUserNameOrEmail(newDLLUser.Email); ;
            }
        }

        public bool VerifyAccount(string userName, string code)
        {
            using (var db = new DDLDataContext())
            {
                var user = GetByUserNameOrEmail(userName);

                // Check code.
                if (user == null || !user.VerifyCode.Equals(code)) return false;

                // Update account status.
                user.IsActive = true;
                user.IsVerify = true;
                user.VerifyCode = string.Empty;
                db.DDL_Users.AddOrUpdate(user);
                db.SaveChanges();

                return true;
            }
        }

        public DDL_User UpdateUser(DDL_User user)
        {
            using (var db = new DDLDataContext())
            {
                db.DDL_Users.AddOrUpdate(user);
                db.UserInfos.AddOrUpdate(user.UserInfo);
                db.SaveChanges();

                return GetByUserNameOrEmail(user.Email);
            }
        }

        public bool SendCodeResetPassword(string email)
        {
            using (var db = new DDLDataContext())
            {
                var user = GetByUserNameOrEmail(email);
                if (user == null || user.LoginType == DDLConstants.LoginType.FACEBOOK)
                {
                    throw new UserNotFoundException();
                }

                string resetCode = GenerateResetCode();
                user.VerifyCode = resetCode;
                db.DDL_Users.AddOrUpdate(user);
                db.SaveChanges();

                MailHelper.Instance.SendMailResetPasswordCode(email, resetCode, user.UserInfo.FullName);

                return true;
            }
        }

        public bool ResetPassword(string email, string code)
        {
            using (var db = new DDLDataContext())
            {
                var user = GetByUserNameOrEmail(email);
                if (user == null || user.LoginType == DDLConstants.LoginType.FACEBOOK)
                {
                    throw new UserNotFoundException();
                }

                if (string.IsNullOrEmpty(code) || !code.Equals(user.VerifyCode))
                {
                    throw new InvalidDataException();
                }

                string newPassword = GenerateNewPassword();
                user.Password = CommonUtils.Md5(newPassword);
                user.VerifyCode = string.Empty;
                db.DDL_Users.AddOrUpdate(user);
                db.SaveChanges();

                MailHelper.Instance.SendMailResetPassword(email, newPassword, user.UserInfo.FullName);

                return true;
            }
        }

        public List<UserNameDTO> GetListUserName(string username)
        {
            using (var db = new DDLDataContext())
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
        }

        public UserPublicInfoDTO GetUserPublicInfo(string userName)
        {
            using (var db = new DDLDataContext())
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
                                     CountCreatedProject =
                                         user.CreatedProjects.Count(x => x.Status != DDLConstants.ProjectStatus.DRAFT
                                                                         && x.Status != DDLConstants.ProjectStatus.REJECTED
                                                                         && x.Status != DDLConstants.ProjectStatus.PENDING),
                                     UserName = user.Username,
                                     Website = user.UserInfo.Website
                                 };

                var userPublicDto = userPublic.FirstOrDefault();
                if (userPublicDto == null)
                {
                    throw new UserNotFoundException();
                }
                userPublicDto.CreatedDate = CommonUtils.ConvertDateTimeFromUtc(userPublicDto.CreatedDate);

                return userPublicDto;
            }
        }

        public UserEditInfoDTO GetUserEditInfo(string userName)
        {
            using (var db = new DDLDataContext())
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
        }


        //UserEditInfoDTO
        public void EditUserInfo(UserEditInfoDTO userCurrent, string uploadImageName)
        {
            using (var db = new DDLDataContext())
            {
                DDL_User firstOrDefault = db.DDL_Users.FirstOrDefault(x => x.Username.Equals(userCurrent.UserName));
                if (firstOrDefault != null)
                {
                    var userEdit = firstOrDefault.UserInfo;
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
                }

                db.SaveChanges();
            }
        }

        public EditPasswordDTO GetUserPassword(string userName)
        {
            using (var db = new DDLDataContext())
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
        }

        public bool ChangePassword(string userName, EditPasswordDTO newPass)
        {
            using (var db = new DDLDataContext())
            {
                var userCurrent = db.DDL_Users.FirstOrDefault(x => x.Username.Equals(userName));
                if (userCurrent != null && userCurrent.Password == newPass.CurrentPassword)
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
        }

        // 22/10/2015 - MaiCTP - get User info for projectBacked page
        public UserBackedInfoDTO GetBackedUserInfo(string userName)
        {

            using (var db = new DDLDataContext())
            {
                var currentUser = (from user in db.DDL_Users
                                   where user.Username.Equals(userName)
                                   select new UserBackedInfoDTO
                                   {
                                       FullName = user.UserInfo.FullName,
                                       ProfileImage = user.UserInfo.ProfileImage,
                                       Email = user.Email,
                                       Add = user.UserInfo.Address,
                                       Phone = user.UserInfo.PhoneNumber
                                   }).FirstOrDefault();

                return currentUser;
            }
        }

        public AdminUserListDTO GetUserListForAdmin()
        {
            using (var db = new DDLDataContext())
            {
                //var userList = db.DDL_Users.Where(x => x.UserType != DDLConstants.UserType.ADMIN).ToList();
                AdminUserListDTO listReturn = new AdminUserListDTO();
                var listUser = (from user in db.DDL_Users
                                where user.UserType != DDLConstants.UserType.ADMIN
                                select new AdminUserDTO
                                {
                                    Email = user.Email,
                                    FullName = user.UserInfo.FullName,
                                    LoginType = user.LoginType,
                                    PhoneNumber = user.UserInfo.PhoneNumber,
                                    Status = user.IsVerify,
                                    UserName = user.Username,
                                    CreatedDate = user.CreatedDate,
                                    StatusActive = user.IsActive
                                }).ToList();
                listUser.ForEach(x =>
                {
                    x.CreatedDate = CommonUtils.ConvertDateTimeFromUtc(x.CreatedDate.GetValueOrDefault());
                    x.LoginType = x.LoginType == DDLConstants.LoginType.NORMAL ? "Bình thường" : "Facebook";
                });
                //foreach (var user in userList)
                //{
                //    var userReturn = new AdminUserDTO
                //    {
                //        Email = user.Email,
                //        FullName = user.UserInfo.FullName,
                //        LoginType = user.LoginType,
                //        PhoneNumber = user.UserInfo.PhoneNumber,
                //        Status = user.IsVerify,
                //        UserName = user.Username,
                //        CreatedDate = user.CreatedDate,
                //        StatusActive = user.IsActive
                //    };
                //    userReturn.CreatedDate = CommonUtils.ConvertDateTimeFromUtc(userReturn.CreatedDate.GetValueOrDefault());
                //    userReturn.LoginType = userReturn.LoginType == DDLConstants.LoginType.NORMAL ? "Bình thường" : "Facebook";
                //    listUser.Add(userReturn);
                //}
                listReturn.ListUser = listUser;
                listReturn.TotalUser = listUser.Count();
                listReturn.ActiveUser = listUser.Count(x => x.Status == true);
                listReturn.InActiveUser = listUser.Count(x => x.Status == false);
                listReturn.NewUser = listUser.Count(x => x.CreatedDate.GetValueOrDefault().ToString("dd-MM-yyyy") == DateTime.UtcNow.ToString("dd-MM-yyyy"));
                return listReturn;
            }
        }
        public void ChangeUserStatus(string UserName)
        {
            using (var db = new DDLDataContext())
            {
                var userList = db.DDL_Users.FirstOrDefault(x => x.Username == UserName);
                if (userList == null)
                {
                    throw new KeyNotFoundException();
                }
                if (userList.IsActive == true)
                {
                    userList.IsActive = false;
                }
                else userList.IsActive = true;
                db.SaveChanges();
            }
        }

        public AdminUserProfileDTO GetUserProfileForAdmin(string UserName)
        {
            using (var db = new DDLDataContext())
            {
                var userProfile = from user in db.DDL_Users
                                  where user.Username == UserName
                                  select new AdminUserProfileDTO
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
                                      LoginType = user.LoginType,
                                      CountBackedProject = user.Backings.Count,
                                      CountCreatedProject =
                                      user.CreatedProjects.Count(x => x.Status != DDLConstants.ProjectStatus.DRAFT
                                                               && x.Status != DDLConstants.ProjectStatus.REJECTED
                                                               && x.Status != DDLConstants.ProjectStatus.PENDING),
                                      IsActive = user.IsActive,
                                  };
                AdminUserProfileDTO userReturn = userProfile.FirstOrDefault();
                if (userReturn == null)
                {
                    throw new KeyNotFoundException();
                }
                if (userReturn.LoginType == DDLConstants.LoginType.NORMAL)
                {
                    userReturn.LoginType = "Bình thường";
                }
                else userReturn.LoginType = "Facebook";

                return userReturn;
            }
        }

        public List<AdminUserBackedListDTO> GetUserBackedProjectForAdmin(string UserName)
        {
            using (var db = new DDLDataContext())
            {
                var userCurrent = db.DDL_Users.FirstOrDefault(x => x.Username == UserName);
                if (userCurrent == null)
                {
                    throw new KeyNotFoundException();
                }
                //var listProjectBacked = userCurrent.Backings.ToList();
                var listProjectBacked = (from backing in db.Backings
                                         select new AdminUserBackedListDTO
                                         {
                                             Status = backing.Project.Status,
                                             PledgedAmount = backing.BackingDetail.PledgedAmount,
                                             FundingGoals = backing.Project.FundingGoal,
                                             ProjectTitle = backing.Project.Title,
                                             ProjectCode = backing.Project.ProjectCode,
                                             Isfunded = backing.Project.Status == DDLConstants.ProjectStatus.APPROVED
                                                    ? (backing.Project.IsFunded ? "suscced" :
                                                    (!backing.Project.IsFunded && backing.Project.IsExprired ? "fail" :
                                                    (!backing.Project.IsFunded && !backing.Project.IsExprired ? "going" : null))) : null
                                         }).ToList();

                List<AdminUserBackedListDTO> listReturn = new List<AdminUserBackedListDTO>();
                foreach (var backed in listProjectBacked)
                {
                    //AdminUserBackedListDTO projectReturn = new AdminUserBackedListDTO { Status = backed.Project.Status };

                    //if (backed.Project.Status == DDLConstants.ProjectStatus.APPROVED)
                    //{
                    //    if (backed.Project.IsFunded == true)
                    //    {
                    //        projectReturn.Isfunded = "suscced";
                    //    }
                    //    else if (backed.Project.IsFunded == false && backed.Project.IsExprired == true)
                    //    {
                    //        projectReturn.Isfunded = "fail";
                    //    }
                    //    else if (backed.Project.IsFunded == false && backed.Project.IsExprired == false)
                    //    {
                    //        projectReturn.Isfunded = "going";
                    //    }
                    //}
                    //else if (backed.Project.Status == DDLConstants.ProjectStatus.REJECTED)
                    //{
                    //    projectReturn.Status = DDLConstants.ProjectStatus.REJECTED;
                    //}
                    //else if (backed.Project.Status == DDLConstants.ProjectStatus.PENDING)
                    //{
                    //    projectReturn.Status = DDLConstants.ProjectStatus.PENDING;
                    //}
                    //else if (backed.Project.Status == DDLConstants.ProjectStatus.SUSPENDED)
                    //{
                    //    projectReturn.Status = DDLConstants.ProjectStatus.SUSPENDED;
                    //}
                    //projectReturn.PledgedAmount = backed.BackingDetail.PledgedAmount;
                    //projectReturn.FundingGoals = backed.Project.FundingGoal;
                    //projectReturn.ProjectTitle = backed.Project.Title;
                    //projectReturn.ProjectCode = backed.Project.ProjectCode;
                    if (listReturn.All(x => x.ProjectCode != backed.ProjectCode))
                    {
                        listReturn.Add(backed);
                    }
                    else
                    {
                        AdminUserBackedListDTO adminUserBackedListDto = listReturn.FirstOrDefault(x => x.ProjectCode == backed.ProjectCode);
                        if (adminUserBackedListDto != null)
                            adminUserBackedListDto.PledgedAmount += backed.PledgedAmount;
                    }
                }
                return listReturn;
            }
        }

        public List<AdminUserCreatedListDTO> GetUserCreatedProjectForAdmin(string UserName)
        {
            using (var db = new DDLDataContext())
            {
                //var userCurrent = db.DDL_Users.FirstOrDefault(x => x.Username == UserName);
                //var listProjectCreated = db.Projects.Where(x => x.Creator.Username.Equals(UserName, StringComparison.OrdinalIgnoreCase) && x.Status != DDLConstants.ProjectStatus.DRAFT).ToList();


                var listReturn = (from project in db.Projects
                                  where
                                      project.Creator.Username.Equals(UserName, StringComparison.OrdinalIgnoreCase) &&
                                      project.Status != DDLConstants.ProjectStatus.DRAFT
                                  select new AdminUserCreatedListDTO
                                  {
                                      FundingGoals = project.FundingGoal,
                                      Status = project.Status,
                                      ProjectTitle = project.Title,
                                      ProjectCode = project.ProjectCode,
                                      ExpireDate = project.ExpireDate,
                                      Category = project.Category.Name,
                                      Isexpired = project.IsExprired ? -1 : 0,
                                      Isfunded = project.IsFunded.ToString()
                                  }).ToList();

                listReturn.ForEach(x => x.ExpireDate = CommonUtils.ConvertDateTimeFromUtc(x.ExpireDate.GetValueOrDefault()));

                foreach (var created in listReturn)
                {
                    //AdminUserCreatedListDTO projectReturn = new AdminUserCreatedListDTO
                    //{
                    //    FundingGoals = created.FundingGoal,
                    //    Status = created.Status,
                    //    ProjectTitle = created.Title,
                    //    ProjectCode = created.ProjectCode,
                    //    ExpireDate = CommonUtils.ConvertDateTimeFromUtc(created.ExpireDate.GetValueOrDefault()),
                    //    Category = created.Category.Name
                    //};

                    if (created.Isexpired == 0)
                    {
                        TimeSpan t = created.ExpireDate.GetValueOrDefault().Date - DateTime.UtcNow.Date;
                        created.Isexpired = t.TotalDays + 1;
                    }
                    else
                    {

                    }

                    if (created.Status == DDLConstants.ProjectStatus.APPROVED)
                    {
                        created.Status = DDLConstants.ProjectStatus.APPROVED;
                        if (created.Isfunded == "True")
                        {
                            created.Isfunded = "suscced";
                        }
                        else if (created.Isfunded == "False" && created.Isexpired == -1)
                        {
                            created.Isfunded = "fail";
                        }
                        //else if (created.Status == "draft")
                        //{
                        //    projectReturn.Isfunded = "Nháp";
                        //}
                        else if (created.Isfunded == "False" && created.Isexpired == 0)
                        {
                            created.Isfunded = "going";
                        }
                    }

                    //List<Backing> allBacked = db.Backings.Where(x => x.Project.ProjectCode == created.ProjectCode).ToList();
                    //decimal pledgedOn = new decimal();
                    //foreach (Backing backing in allBacked)
                    //{
                    //    pledgedOn = pledgedOn + backing.BackingDetail.PledgedAmount;
                    //};
                    var pledgedOn = (from backing in db.Backings
                                     where backing.Project.ProjectCode == created.ProjectCode
                                     group backing by backing.ProjectID into g
                                     select g.Sum(x => x.BackingDetail.PledgedAmount)).ToList();
                    created.PledgedOn = pledgedOn[0];
                }
                return listReturn;
            }
        }

        public AdminUserBackingDetailDTO GetUseBackingDetailForAdmin(string UserName)
        {
            using (var db = new DDLDataContext())
            {
                var userCurrent1 = db.DDL_Users.FirstOrDefault(x => x.Username == UserName);
                if (userCurrent1 == null)
                {
                    throw new KeyNotFoundException();
                }
                var backingList = userCurrent1.Backings.ToList();
                Backing backing = new Backing();
                backing = backingList.FirstOrDefault();
                if (backing == null)
                {
                    throw new KeyNotFoundException();
                }
                if (backingList.Count() > 1)
                {
                    foreach (var back in backingList)
                    {
                        if (backing.BackingID != back.BackingID)
                        {
                            backing.BackingDetail.PledgedAmount += back.BackingDetail.PledgedAmount;
                            backing.BackingDetail.Quantity += back.BackingDetail.Quantity;
                        }
                    }
                }

                var userCurrent = db.UserInfos.FirstOrDefault(x => x.DDL_User.Username == UserName);
                AdminUserBackingDetailDTO backingReturn = new AdminUserBackingDetailDTO();
                backingReturn.Address = userCurrent.Address;
                backingReturn.BackedDate = CommonUtils.ConvertDateTimeFromUtc(backing.BackedDate);
                backingReturn.Description = backing.BackingDetail.Description;
                backingReturn.Email = userCurrent.DDL_User.Email;
                backingReturn.FullName = userCurrent.FullName;
                backingReturn.PhoneNumber = userCurrent.PhoneNumber;
                backingReturn.PledgedAmount = backing.BackingDetail.PledgedAmount;
                backingReturn.Quantity = backing.BackingDetail.Quantity;
                backingReturn.Reward = backing.BackingDetail.RewardPkg.PledgeAmount;
                backingReturn.ProjectTitle = backing.Project.Title;
                backingReturn.Total = backingReturn.PledgedAmount;
                return backingReturn;
            }
        }

        public AdminUserDashboardDTO GetUserDashboardForAdmin()
        {
            using (var db = new DDLDataContext())
            {
                var userList = db.DDL_Users.Where(x => x.UserType != DDLConstants.UserType.ADMIN).ToList();
                AdminUserDashboardDTO listReturn = new AdminUserDashboardDTO();
                //RecentUserDTO RecentUser = new RecentUserDTO();
                List<RecentUserDTO> listRecentUser = new List<RecentUserDTO>();
                foreach (var user in userList)
                {
                    if (user.IsVerify == true)
                    {
                        var userReturn = new RecentUserDTO
                        {
                            AvartaURL = user.UserInfo.ProfileImage,
                            UserName = user.Username,
                            LastLogin = user.LastLogin,
                            Status = user.IsActive,
                            FullName = user.UserInfo.FullName
                        };
                        userReturn.LastLogin = CommonUtils.ConvertDateTimeFromUtc(userReturn.LastLogin.GetValueOrDefault());
                        listRecentUser.Add(userReturn);
                    }

                }

                List<TopBackerDTO> listTopbackerUser = new List<TopBackerDTO>();
                foreach (var user in userList)
                {
                    if (user.IsVerify == true)
                    {
                        if (user.Backings.Any())
                        {
                            var userReturn = new TopBackerDTO
                            {
                                AvartaURL = user.UserInfo.ProfileImage,
                                UserName = user.Username,
                                Status = user.IsActive,
                                FullName = user.UserInfo.FullName,
                            };
                            var backingList = user.Backings.ToList();
                            userReturn.TotalProject = backingList.GroupBy(x => x.ProjectID).Count();
                            foreach (var backing in backingList)
                            {
                                userReturn.TotalPledgedAmount = userReturn.TotalPledgedAmount + backing.BackingDetail.PledgedAmount;
                            }
                            listTopbackerUser.Add(userReturn);
                            listReturn.Backer = listTopbackerUser.Count();
                        }
                    }
                }

                // TopCreatorDTO TopCreator = new TopCreatorDTO();
                List<TopCreatorDTO> listTopCreator = new List<TopCreatorDTO>();
                foreach (var user in userList)
                {
                    if (user.IsVerify != true) continue;
                    if (!user.CreatedProjects.Any() || user.CreatedProjects.All(x => x.IsFunded != true)) continue;
                    var userReturn = new TopCreatorDTO
                    {
                        AvartaURL = user.UserInfo.ProfileImage,
                        UserName = user.Username,
                        Status = user.IsActive,
                        FullName = user.UserInfo.FullName,
                        TotalSuccessProject = user.CreatedProjects.Count(x => x.IsFunded == true),
                    };
                    var createdProject = user.CreatedProjects.Where(x => x.IsFunded == true).ToList();
                    foreach (var project in createdProject)
                    {
                        userReturn.TotalPledgedAmount = userReturn.TotalPledgedAmount + project.CurrentFunded;
                    }
                    listTopCreator.Add(userReturn);
                    if (listTopbackerUser.All(x => x.UserName == userReturn.UserName))
                    {
                        listReturn.Creator = listReturn.Creator + 1;
                    }
                }

                List<NewUserDTO> listNewUser = new List<NewUserDTO>();
                var NewuserList = userList.Where(x => x.CreatedDate.ToString("MM-yyyy") == DateTime.UtcNow.ToString("MM-yyyy")).ToList();
                foreach (var user in NewuserList)
                {
                    if (user.IsVerify != true) continue;
                    var userReturn = new NewUserDTO
                    {
                        AvartaURL = user.UserInfo.ProfileImage,
                        UserName = user.Username,
                        Status = user.IsActive,
                        FullName = user.UserInfo.FullName,
                        CreatedDate = user.CreatedDate
                    };

                    listNewUser.Add(userReturn);
                }


                listTopCreator = listTopCreator.OrderByDescending(x => x.TotalPledgedAmount).Take(5).ToList();
                listRecentUser = listRecentUser.OrderByDescending(x => x.LastLogin).Take(5).ToList(); ;
                listTopbackerUser = listTopbackerUser.OrderByDescending(x => x.TotalPledgedAmount).Take(5).ToList();
                listNewUser = listNewUser.OrderByDescending(x => x.CreatedDate).ToList();


                listReturn.RecentUser = listRecentUser;
                listReturn.TopBacker = listTopbackerUser;
                listReturn.TopCreator = listTopCreator;
                listReturn.ListNewUser = listNewUser;

                listReturn.TotalUser = userList.Count();
                listReturn.VerifiedUser = userList.Count(x => x.IsVerify);
                listReturn.NotVerifiedUser = userList.Count(x => x.IsVerify == false);
                listReturn.NewUser = userList.Count(x => x.CreatedDate.ToString("dd-MM-yyyy") == DateTime.UtcNow.ToString("dd-MM-yyyy"));
                listReturn.IdleUser = listReturn.TotalUser - listReturn.Creator - listReturn.Backer - listReturn.NotVerifiedUser;

                return listReturn;
            }
        }

        public List<AdminBackingListDTO> GetBackingListForAdmin()
        {
            using (var db = new DDLDataContext())
            {
                //var listBacking = db.Backings.ToList();
                var listReturn = (from backing in db.Backings
                                  select new AdminBackingListDTO
                                  {
                                      ProjectTitle = backing.Project.Title,
                                      PhoneNumber = backing.User.UserInfo.PhoneNumber,
                                      PledgeAmount = backing.BackingDetail.PledgedAmount,
                                      BackerName = backing.User.UserInfo.FullName,
                                      Address = backing.User.UserInfo.Address,
                                      BackedDate = backing.BackedDate,
                                      Content = backing.BackingDetail.Description,
                                      Email = backing.User.Email,
                                      RewardContent = backing.BackingDetail.RewardPkg.Description,
                                      RewardPledgeAmount = backing.BackingDetail.RewardPkg.PledgeAmount,
                                      UserName = backing.User.Username,
                                      BackingID = backing.BackingID
                                  }).ToList();

                listReturn.ForEach(x => x.BackedDate = CommonUtils.ConvertDateTimeFromUtc(x.BackedDate.GetValueOrDefault()));

                //foreach (var backing in listBacking)
                //{
                //    AdminBackingListDTO backingReturn = new AdminBackingListDTO
                //    {
                //        ProjectTitle = backing.Project.Title,
                //        PhoneNumber = backing.User.UserInfo.PhoneNumber,
                //        PledgeAmount = backing.BackingDetail.PledgedAmount,
                //        BackerName = backing.User.UserInfo.FullName,
                //        Address = backing.User.UserInfo.Address,
                //        BackedDate = CommonUtils.ConvertDateTimeFromUtc(backing.BackedDate),
                //        Content = backing.BackingDetail.Description,
                //        Email = backing.User.Email,
                //        RewardContent = backing.BackingDetail.RewardPkg.Description,
                //        RewardPledgeAmount = backing.BackingDetail.RewardPkg.PledgeAmount,
                //        UserName = backing.User.Username,
                //        BackingID = backing.BackingID
                //    };
                //    listReturn.Add(backingReturn);
                //}
                return listReturn;
            }
        }

        public AdminBackingListDTO GetBackerForAdmin(string userName, int backingID)
        {
            using (var db = new DDLDataContext())
            {
                var backing = db.Backings.FirstOrDefault(x => x.BackingID == backingID);
                if (backing == null)
                {
                    throw new KeyNotFoundException();
                }

                AdminBackingListDTO backingReturn = new AdminBackingListDTO
                {
                    ProjectTitle = backing.Project.Title,
                    PhoneNumber = backing.User.UserInfo.PhoneNumber,
                    PledgeAmount = backing.BackingDetail.PledgedAmount,
                    BackerName = backing.User.UserInfo.FullName,
                    Address = backing.User.UserInfo.Address,
                    BackedDate = CommonUtils.ConvertDateTimeFromUtc(backing.BackedDate),
                    Content = backing.BackingDetail.Description,
                    Email = backing.User.Email,
                    RewardContent = backing.BackingDetail.RewardPkg.Description,
                    RewardPledgeAmount = backing.BackingDetail.RewardPkg.PledgeAmount,
                    ImageURL = backing.User.UserInfo.ProfileImage,
                    Biography = backing.User.UserInfo.Biography
                };

                return backingReturn;
            }
        }

        #region HuyNM
        /// <summary>
        /// Get top 5 backers for admin
        /// </summary>
        /// <returns>listTopbackerUser</returns>
        public List<TopBackerDTO> AdminGetTopBacker()
        {
            using (var db = new DDLDataContext())
            {
                var userList = db.DDL_Users.Where(x => x.UserType != "admin").ToList();

                List<TopBackerDTO> listTopbackerUser = new List<TopBackerDTO>();
                foreach (var user in userList)
                {
                    if (user.Backings.Any())
                    {
                        var userReturn = new TopBackerDTO
                        {
                            AvartaURL = user.UserInfo.ProfileImage,
                            UserName = user.Username,
                            Status = user.IsActive,
                            FullName = user.UserInfo.FullName,
                        };
                        var backingList = user.Backings.ToList();
                        userReturn.TotalProject = backingList.GroupBy(x => x.ProjectID).Count();
                        foreach (var backing in backingList)
                        {
                            userReturn.TotalPledgedAmount = userReturn.TotalPledgedAmount + backing.BackingDetail.PledgedAmount;
                        }
                        listTopbackerUser.Add(userReturn);
                    }
                }

                listTopbackerUser = listTopbackerUser.OrderByDescending(x => x.TotalPledgedAmount).Take(5).ToList();

                return listTopbackerUser;
            }
        }

        /// <summary>
        /// Get 5 recent users for admin
        /// </summary>
        /// <returns>listRecentUser</returns>
        public List<RecentUserDTO> AdminGetRecentUser()
        {
            using (var db = new DDLDataContext())
            {
                var userList = db.DDL_Users.Where(x => x.UserType != "admin").ToList();

                List<RecentUserDTO> listRecentUser = new List<RecentUserDTO>();
                foreach (var user in userList)
                {
                    var userReturn = new RecentUserDTO
                    {
                        AvartaURL = user.UserInfo.ProfileImage,
                        UserName = user.Username,
                        LastLogin = user.LastLogin,
                        Status = user.IsActive,
                        FullName = user.UserInfo.FullName
                    };
                    userReturn.LastLogin = CommonUtils.ConvertDateTimeFromUtc(userReturn.LastLogin.GetValueOrDefault());
                    listRecentUser.Add(userReturn);
                }

                listRecentUser = listRecentUser.OrderByDescending(x => x.LastLogin).Take(5).ToList();

                return listRecentUser;
            }
        }
        #endregion
        #endregion
    }
}