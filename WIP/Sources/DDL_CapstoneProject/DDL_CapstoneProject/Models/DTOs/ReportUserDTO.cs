using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models.DTOs
{
    public class ReportUserDTO
    {
        public int ReporterID { get; set; }
        public string ReporterUsername { get; set; }
        public string ReporterFullname { get; set; }
        public int ReportedUserID { get; set; }
        public string ReportedUsername { get; set; }
        public string ReportedFullname { get; set; }
        public string Subject { get; set; }
        public string ReportContent { get; set; }
        public string ReportedDate { get; set; }
        public string Status { get; set; }
        public int ReportID { get; set; }
    }
}