using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models.DTOs
{
    public class ReportProjectDTO
    {
        public int ReportID { get; set; }
        public string ReporterUsername { get; set; }
        public string reporterName { get; set; }
        public int ProjectID { get; set; }
        public string ProjectTitle { get; set; }
        public int CreatorID { get; set; }
        public string CreatorName { get; set; }
        public string Subject { get; set; }
        public string ReportContent { get; set; }
        public string ReportedDate { get; set; }
        public string Status { get; set; }

    }
}