using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models
{
    public class ReportUser
    {
        #region "Attributes"

        /// <summary>
        /// Identify number of ReportUser.
        /// </summary>
        [Key]
        public int ReportID { get; set; }

        /// <summary>
        /// Identify number of Reporter.
        /// </summary>
        public int ReporterID { get; set; }

        /// <summary>
        /// Identify number of Reported User.
        /// </summary>
        public int ReportedUserID { get; set; }

        /// <summary>
        /// Subject of report.
        /// </summary>
        public string Subject { get; set; }
        
        /// <summary>
        /// Content of report.
        /// </summary>
        public string ReportContent { get; set; }

        /// <summary>
        /// Created Report datetime
        /// </summary>
        public DateTime ReportedDate { get; set; }

        /// <summary>
        /// Status of report.
        /// </summary>
        public ReportStatus Status { get; set; }
        #endregion

        [ForeignKey("ReporterID")]
        [InverseProperty("CreatedReportUsers")]
        public virtual DDL_User Reporter { get; set; }

        [ForeignKey("ReportedUserID")]
        [InverseProperty("ReportedReportUsers")]
        public virtual DDL_User ReportedUser { get; set; }
    }

    #region "Enum"

    public enum ReportStatus
    {
        New = 1,
        View = 2,
        Done = 3
    }
    #endregion
}