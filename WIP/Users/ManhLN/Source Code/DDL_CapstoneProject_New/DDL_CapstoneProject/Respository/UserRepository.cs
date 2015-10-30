using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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

        #region "Contructors"
        private UserRepository()
        {
        }
        #endregion

        #region "Methods"
        #region TrungVN

        public Dictionary<string, List<UserBackInforDTO>> GetUserTop(string categoryid)
        {
            using (var db = new DDLDataContext())
            {
                categoryid = "|" + categoryid + "|";
                bool allCategory = categoryid.ToLower().Contains("all");
                var userTop = from user in db.DDL_Users
                    select new UserBackInforDTO
                    {
                        Rank = "Rank A",
                        Name = user.UserInfo.FullName,
                        TotalFunded =
                            user.CreatedProjects.Where(x => categoryid.Contains(x.CategoryID.ToString()) || allCategory)
                                .Sum(x => (decimal?) x.CurrentFunded) ?? 0,
                        TotalBacked =
                            user.Backings.Where(x => categoryid.Contains(x.Project.CategoryID.ToString()) || allCategory)
                                .Sum(x => (decimal?) x.BackingDetail.PledgedAmount) ?? 0
                    };
                int count = userTop.Count();
                if (count >= 10) count = 10;
                Dictionary<string, List<UserBackInforDTO>> dic = new Dictionary<string, List<UserBackInforDTO>>();
                dic.Add("UserTopBack",
                    userTop.Where(x => x.TotalBacked > 0)
                        .Take(count)
                        .OrderByDescending(x => x.TotalBacked)
                        .ThenByDescending(x => x.TotalFunded)
                        .ToList());
                dic.Add("UserTopFund",
                    userTop.Where(x => x.TotalFunded > 0)
                        .Take(count)
                        .OrderByDescending(x => x.TotalFunded)
                        .ThenByDescending(x => x.TotalBacked)
                        .ToList());
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
                try
                {
                    db.DDL_Users.Add(newUser);
                    db.SaveChanges();
                }
                catch (Exception)
                {

                    return null;
                }

                return GetByUserNameOrEmail(newUser.Email);
            }
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
                    newDLLUser.UserInfo.ProfileImage = "avatar_default.png";
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
                db.SaveChanges();

                return true;
            }
        }

        public DDL_User UpdateUser(DDL_User user)
        {
            using (var db = new DDLDataContext())
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

                return GetByUserNameOrEmail(user.Email);
            }
        }

        public bool ResetPassword(string email)
        {
            using (var db = new DDLDataContext())
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
                if (!userPublic.Any())
                {
                    throw new UserNotFoundException();
                }

                var userPublicDTO = userPublic.FirstOrDefault();
                userPublicDTO.CreatedDate = CommonUtils.ConvertDateTimeFromUtc(userPublicDTO.CreatedDate);

                return userPublicDTO;
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

        public Boolean ChangePassword(string userName, EditPasswordDTO newPass)
        {
            using (var db = new DDLDataContext())
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
                var userList = db.DDL_Users.Where(x => x.UserType != "admin").ToList();
                AdminUserListDTO listReturn = new AdminUserListDTO();
                List<AdminUserDTO> listUser = new List<AdminUserDTO>();
                foreach (var user in userList)
                {
                    var userReturn = new AdminUserDTO
                    {
                        Email = user.Email,
                        FullName = user.UserInfo.FullName,
                        LoginType = user.LoginType,
                        PhoneNumber = user.UserInfo.PhoneNumber,
                        Status = user.IsVerify,
                        UserName = user.Username,
                        CreatedDate = user.CreatedDate
                    };
                    userReturn.CreatedDate = CommonUtils.ConvertDateTimeToUtc(userReturn.CreatedDate.GetValueOrDefault());
                    if (userReturn.LoginType == "normal")
                    {
                        userReturn.LoginType = "Bình thường";
                    }
                    else userReturn.LoginType = "Facebook";
                    listUser.Add(userReturn);
                }
                listReturn.ListUser = listUser;
                listReturn.TotalUser = listUser.Count();
                listReturn.ActiveUser = listUser.Where(x => x.Status == true).Count();
                listReturn.InActiveUser = listUser.Where(x => x.Status == false).Count();
                listReturn.NewUser = listUser.Where(x => x.CreatedDate.GetValueOrDefault().ToString("dd-MM-yyyy") == DateTime.UtcNow.ToString("dd-MM-yyyy")).Count();
                return listReturn;
            }
        }
        public void ChangeUserStatus(string UserName)
        {
            using (var db = new DDLDataContext())
            {
                var userList = db.DDL_Users.Where(x => x.Username == UserName).FirstOrDefault();
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
                AdminUserProfileDTO userReturn = new AdminUserProfileDTO();
                userReturn = userProfile.FirstOrDefault();
                if (userReturn.LoginType == "normal")
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
                var userCurrent = db.DDL_Users.Where(x => x.Username == UserName).FirstOrDefault();
                var listProjectBacked = userCurrent.Backings.ToList();
                List<AdminUserBackedListDTO> listReturn = new List<AdminUserBackedListDTO>();
                foreach (var backed in listProjectBacked)
                {
                    AdminUserBackedListDTO projectReturn = new AdminUserBackedListDTO();
                    projectReturn.Status = backed.Project.Status;
                    if (backed.Project.IsFunded == true)
                    {
                        projectReturn.Status = "Thành công";
                    }
                    else if (backed.Project.IsFunded == false && backed.Project.IsExprired == true)
                    {
                        projectReturn.Status = "Thất bại";
                    }
                    else if (backed.Project.IsFunded == false && backed.Project.IsExprired == false)
                    {
                        projectReturn.Status = "Đang chạy";
                    }
                    projectReturn.PledgedAmount = backed.BackingDetail.PledgedAmount;
                    projectReturn.FundingGoals = backed.Project.FundingGoal;
                    projectReturn.ProjectTitle = backed.Project.Title;
                    projectReturn.ProjectCode = backed.Project.ProjectCode;
                    if (!listReturn.Any(x => x.ProjectCode == projectReturn.ProjectCode))
                    {
                        listReturn.Add(projectReturn);
                    }
                    else
                    {
                        listReturn.Where(x => x.ProjectCode == projectReturn.ProjectCode).FirstOrDefault().PledgedAmount += projectReturn.PledgedAmount;
                    }


                }
                return listReturn;
            }
        }

        public List<AdminUserCreatedListDTO> GetUserCreatedProjectForAdmin(string UserName)
        {
            using (var db = new DDLDataContext())
            {
                var userCurrent = db.DDL_Users.Where(x => x.Username == UserName).FirstOrDefault();
                var listProjectCreated = db.Projects.Where(x => x.CreatorID == userCurrent.DDL_UserID).ToList();
                List<AdminUserCreatedListDTO> listReturn = new List<AdminUserCreatedListDTO>();
                foreach (var created in listProjectCreated)
                {
                    AdminUserCreatedListDTO projectReturn = new AdminUserCreatedListDTO();
                    projectReturn.FundingGoals = created.FundingGoal;
                    projectReturn.Status = created.Status;
                    projectReturn.ProjectTitle = created.Title;
                    projectReturn.ProjectCode = created.ProjectCode;
                    projectReturn.ExpireDate = created.ExpireDate;
                    projectReturn.Category = created.Category.Name;
                    if (created.IsFunded == true)
                    {
                        projectReturn.Isfunded = "Thành công";
                    }
                    else if (created.IsFunded == false && created.IsExprired == true)
                    {
                        projectReturn.Isfunded = "Thất bại";
                    }
                    else if (created.IsFunded == false && created.IsExprired == false)
                    {
                        projectReturn.Isfunded = "Đang chạy";
                    }
                    List<Backing> AllBacked = db.Backings.Where(x => x.Project.ProjectCode == created.ProjectCode).ToList();
                    decimal PledgedOn = new decimal();
                    foreach (Backing backing in AllBacked)
                    {
                        PledgedOn = PledgedOn + backing.BackingDetail.PledgedAmount;
                    };
                    projectReturn.PledgedOn = PledgedOn;
                    listReturn.Add(projectReturn);
                }
                return listReturn;
            }
        }

        public AdminUserBackingDetailDTO GetUseBackingDetailForAdmin(string UserName)
        {
            using (var db = new DDLDataContext())
            {
                var userCurrent1 = db.DDL_Users.Where(x => x.Username == UserName).FirstOrDefault();
                var backingList = userCurrent1.Backings.ToList();
                Backing backing = new Backing();
                backing = backingList.FirstOrDefault();

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

                var userCurrent = db.UserInfos.Where(x => x.DDL_User.Username == UserName).FirstOrDefault();
                AdminUserBackingDetailDTO backingReturn = new AdminUserBackingDetailDTO();
                backingReturn.Address = userCurrent.Address;
                backingReturn.BackedDate = CommonUtils.ConvertDateTimeToUtc(backing.BackedDate);
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
                var userList = db.DDL_Users.Where(x => x.UserType != "admin").ToList();
                AdminUserDashboardDTO listReturn = new AdminUserDashboardDTO();
                RecentUserDTO RecentUser = new RecentUserDTO();
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

                TopBackerDTO TopBacker = new TopBackerDTO();
                List<TopBackerDTO> listTopbackerUser = new List<TopBackerDTO>();
                foreach (var user in userList)
                {
                    if (user.Backings.Count() > 0)
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

                TopCreatorDTO TopCreator = new TopCreatorDTO();
                List<TopCreatorDTO> listTopCreator = new List<TopCreatorDTO>();
                foreach (var user in userList)
                {
                    if (user.CreatedProjects.Count() > 0 && user.CreatedProjects.Where(x => x.IsFunded == true).Count() > 0)
                    {
                        var userReturn = new TopCreatorDTO
                        {
                            AvartaURL = user.UserInfo.ProfileImage,
                            UserName = user.Username,
                            Status = user.IsActive,
                            FullName = user.UserInfo.FullName,
                            TotalSuccessProject = user.CreatedProjects.Where(x => x.IsFunded == true).Count(),
                        };
                        var createdProject = user.CreatedProjects.Where(x => x.IsFunded == true).ToList();
                        foreach (var project in createdProject)
                        {
                            userReturn.TotalPledgedAmount = userReturn.TotalPledgedAmount + project.CurrentFunded;
                        }
                        listTopCreator.Add(userReturn);
                        if (!listTopbackerUser.Any(x => x.UserName != userReturn.UserName))
                        {
                            listReturn.Creator = listReturn.Creator + 1;
                        }
                    }
                }

                NewUserDTO NewUser = new NewUserDTO();
                List<NewUserDTO> listNewUser = new List<NewUserDTO>();
                var NewuserList = userList.Where(x => x.CreatedDate.ToString("dd-MM-yyyy") == DateTime.UtcNow.ToString("dd-MM-yyyy")).ToList();
                foreach (var user in NewuserList)
                {
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
                listNewUser = listNewUser.OrderByDescending(x => x.CreatedDate).Take(5).ToList();


                listReturn.RecentUser = listRecentUser;
                listReturn.TopBacker = listTopbackerUser;
                listReturn.TopCreator = listTopCreator;
                listReturn.ListNewUser = listNewUser;

                listReturn.TotalUser = userList.Count();
                listReturn.VerifiedUser = userList.Where(x => x.IsVerify == true).Count();
                listReturn.NotVerifiedUser = userList.Where(x => x.IsVerify == false).Count();
                listReturn.NewUser = userList.Where(x => x.CreatedDate.ToString("dd-MM-yyyy") == DateTime.UtcNow.ToString("dd-MM-yyyy")).Count();
                listReturn.IdleUser = listReturn.TotalUser - listReturn.Creator - listReturn.Backer - listReturn.NotVerifiedUser;

                return listReturn;
            }
        }

        #endregion
    }
}