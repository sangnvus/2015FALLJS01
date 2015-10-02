using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models
{
    public class UserInfo
    {
        #region "Attributes"
        
        /// <summary>
        /// UserInfo's identify number.
        /// </summary>
        [Key, ForeignKey("DDL_User")]
        public int DDL_UserID { get; set; }

        /// <summary>
        /// Full name of user.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Avatar url of account.
        /// </summary>
        public string ProfileImage { get; set; }

        /// <summary>
        /// Gender of user.
        /// 1: Male
        /// 2: Female
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// Date of birth of user.
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Country of user.
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// Address of user.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Relation website of user.
        /// set be null if doesn't have.
        /// </summary>
        public string Website { get; set; }

        /// <summary>
        /// Facebook Url of user
        /// set be null if doesn't have.
        /// </summary>
        public string FacebookUrl { get; set; }

        /// <summary>
        /// More info about user.
        /// </summary>
        public string Biography { get; set; }

        #endregion
        public virtual DDL_User DDL_User { get; set; }
    }


    #region "Enum"
    public enum Gender
    {
        Male = 1,
        Female = 2
    }
    #endregion
}