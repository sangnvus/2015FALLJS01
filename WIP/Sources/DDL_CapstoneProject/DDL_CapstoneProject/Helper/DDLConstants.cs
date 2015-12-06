using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Ultilities
{
    public static class DDLConstants
    {
        // This class includes constants string of DDL system.
        public static readonly string ConnectionString = "DDLDataContext";
        public static string DatetimeFormat = "yyyy-MM-dd HH:mm:ss";
        public static string DateFormat = "yyyy-MM-dd"; 
        public static string TimeZoneId = "SE Asia Standard Time"; // GMT+7

        /// <summary>
        /// Login type constants
        /// </summary>
        public static class LoginType
        {
            public static readonly string NORMAL = "normal";
            public static readonly string FACEBOOK = "facebook";
            public static readonly string BOTH = "both";
        }

        public static class UserType
        {
            public static readonly string USER = "user";
            public static readonly string ADMIN = "admin";
        }

        public static class Gender
        {
            public static readonly string MALE = "male";
            public static readonly string FEMALE = "female";
        }

        public static class ProjectStatus
        {
            public static readonly string DRAFT = "draft";
            public static readonly string PENDING = "pending";
            public static readonly string REJECTED = "rejected";
            public static readonly string APPROVED = "approved";
            public static readonly string SUSPENDED = "suspended";
            public static readonly string EXPIRED = "expired";
        }

        public static class ReportStatus
        {
            public static readonly string NEW = "new";
            public static readonly string VIEWED = "viewed";
            public static readonly string REJECTED = "rejected";
            public static readonly string DONE = "done";
        }

        public static class RewardType
        {
            public static readonly string NO_REWARD = "no reward";
            public static readonly string UNLIMITED = "unlimited";
            public static readonly string LIMITED = "limited";
        }

        public static class ConversationStatus
        {
            public static readonly string CREATOR = "creator";
            public static readonly string RECEIVER = "receiver";
            public static readonly string BOTH = "both";
            public static readonly string NO = "no";
        }

        public static class HttpMessageType
        {
            public static readonly string NOT_AUTHEN = "not-authen";
            public static readonly string NOT_FOUND = "not-found";
            public static readonly string BAD_REQUEST = "bad-request";
            public static readonly string SUCCESS = "success";
            public static readonly string ERROR = "error";
        }

        public static class FileType
        {
            public static readonly string AVATAR = "/images/avatars/";
            public static readonly string OTHER = "/images/others/";
            public static readonly string PROJECT = "/images/projects/";
            public static readonly string SLIDE = "/images/slides/";
        }

        public static class PopularPointType
        {
            public static readonly int BackingPoint = 10;
            public static readonly int CommentPoint = 2;
            public static readonly int RemindPoint = 1;
            public static readonly int ViewPoint = 1;
            public static readonly int RemoveRemindPoint = -1;
        }
    }
}