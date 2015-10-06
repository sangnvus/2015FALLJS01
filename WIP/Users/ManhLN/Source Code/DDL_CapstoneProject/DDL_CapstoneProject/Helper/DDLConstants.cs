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
    }
}