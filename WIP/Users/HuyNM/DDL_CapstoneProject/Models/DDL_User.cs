using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;

namespace DDL_CapstoneProject.Models
{
    public class DDL_User
    {
        #region "Attributes"
        /// <summary>
        /// DDL_User's Identify number.
        /// </summary>
        public int DDL_UserID { get; set; }

        /// <summary>
        /// User name of a account which used to login
        /// username is unique.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Password of a account which used to login
        /// password is encryted by md5.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// User can register account by normal way or facebook account
        /// There are 3 type:
        ///     Normal (register)
        ///     Facebook (register by facebook)
        ///     Both (relation normal account with a facebook account.
        /// </summary>
        public string LoginType { get; set; }
        
        /// <summary>
        /// Type or role of account
        /// There are 2 role:
        ///     User
        ///     Admin
        /// </summary>
        public string UserType { get; set; }

        /// <summary>
        /// Registered datetime of a account
        /// this value is set when a account registered
        /// </summary>
        public DateTime CreatedDate { get; set; }
        
        /// <summary>
        /// Last login datetime of a account
        /// This value is set when account logged in system.
        /// </summary>
        public DateTime? LastLogin { get; set; }
        
        /// <summary>
        /// Active status of a account.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Email of user
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Account's email is verified?
        /// </summary>
        public bool IsVerify { get; set; }

        /// <summary>
        /// Verify code of a account
        /// System sends a vefiry link with the code to user'emal to verify email.
        /// </summary>
        public string VerifyCode { get; set; }

        #endregion "End Attribute"

        #region "RelationShip"
        
        public virtual UserInfo UserInfo{ get; set; }
        public virtual ICollection<Conversation> CreatedConversations { get; set; }
        public virtual ICollection<Conversation> ReceivedConversations { get; set; }
        public virtual ICollection<Message> SentMessages { get; set; }
        public virtual ICollection<ReportUser> CreatedReportUsers { get; set; }
        public virtual ICollection<ReportUser> ReportedReportUsers { get; set; }
        public virtual ICollection<Project>  CreatedProjects{ get; set; }
        public virtual ICollection<ReportProject> CreatedReportProjects { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Remind> Reminds  { get; set; }
        public virtual ICollection<Backing> Backings { get; set; }

        #endregion
    }

    #region "Enum"

    //public enum LoginType
    //{
    //    Normal = 1,
    //    Facebook = 2,
    //    Both = 3
    //}

    //public enum UserType
    //{
    //    User = 1,
    //    Admin = 2
    //}
    #endregion "Enum"
}