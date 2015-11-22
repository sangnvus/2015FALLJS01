using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models
{
    public class Project
    {
        #region "Attributes"

        public int ProjectID { get; set; }
        public string ProjectCode { get; set; }
        public int CategoryID { get; set; }
        public int CreatorID { get; set; }
        public string Title { get; set; }
        public string Risk { get; set; }
        public string ImageUrl { get; set; }
        public string SubDescription { get; set; }
        public string Location { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsExprired { get; set; }
        public bool IsFunded { get; set; }
        public decimal CurrentFunded { get; set; }
        public DateTime? ExpireDate { get; set; }
        public decimal FundingGoal { get; set; }
        public string Description { get; set; }
        public string VideoUrl { get; set; }
        public int PointOfTheDay { get; set; }

        /// <summary>
        /// Popular point is calculated eachday from number comments, pledged ammount.
        /// </summary>
        public int PopularPoint { get; set; }

        /// <summary>
        /// Status of a project
        /// include 6 statuses:
        ///     draft,
        ///     pending,
        ///     rejected,
        ///     approved,
        ///     suspended,
        ///     expired.
        /// </summary>
        public string Status { get; set; }

        #endregion

        [ForeignKey("CategoryID")]
        public virtual Category Category { get; set; }

        [ForeignKey("CreatorID")]
        [InverseProperty("CreatedProjects")]
        public virtual DDL_User Creator { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<ReportProject> ReportCollection { get; set; }
        public virtual ICollection<UpdateLog> UpdateLogs { get; set; }
        public virtual ICollection<Timeline> Timelines { get; set; }
        public virtual ICollection<RewardPkg> RewardPkgs { get; set; }
        public virtual ICollection<Remind> Reminds { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Backing> Backings { get; set; }
    }

    #region "Enum"

    //public enum ProjectStatus
    //{
    //    Draft = 1,
    //    Pending = 2,
    //    Rejected = 3,
    //    Approved = 4,
    //    Suspended = 5,
    //}
    #endregion
}