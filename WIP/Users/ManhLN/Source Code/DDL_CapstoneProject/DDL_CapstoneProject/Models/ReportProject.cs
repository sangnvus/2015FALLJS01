using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models
{
    public class ReportProject
    {
        #region "Attributes"

        /// <summary>
        /// Identify number of ReportProject.
        /// </summary>
        [Key]
        public int ReportID { get; set; }

        /// <summary>
        /// Identify number of Reporter.
        /// </summary>
        public int ReporterID { get; set; }

        /// <summary>
        /// Identify number of Reported Project.
        /// </summary>
        public int ProjectID { get; set; }

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
        public string Status { get; set; }

        #endregion

        [ForeignKey("ReporterID")]
        [InverseProperty("CreatedReportProjects")]
        public virtual DDL_User Reporter { get; set; }

        [ForeignKey("ProjectID")]
        [InverseProperty("ReportCollection")]
        public virtual Project Project { get; set; }
    }
}