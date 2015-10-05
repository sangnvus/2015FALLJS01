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

        public DDL_User CreateEmptyUser()
        {
            var user = new DDL_User
            {
                Username = string.Empty,
                Email = string.Empty,
                CreatedDate = DateTime.Now,
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
                CreatedDate = DateTime.Now,
                IsActive = true,
                Password = string.Empty,
                IsVerify = true,
                LastLogin = DateTime.Now,
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
        #endregion     
    }
}