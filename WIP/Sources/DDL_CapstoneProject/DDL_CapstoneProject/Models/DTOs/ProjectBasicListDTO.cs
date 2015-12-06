using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models.DTOs
{
    public class ProjectBasicListDTO
    {
        public int ProjectID { get; set; }
        public string ProjectCode { get; set; }
        public string CreatorEmail { get; set; }
        public string CreatorUsername { get; set; }
        public string CreatorFullname { get; set; }
        public string Title { get; set; }
        public decimal CurrentFunded { get; set; }
        public DateTime? ExpireDate { get; set; }
        public decimal FundingGoal { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        public int TotalBacking { get; set; }
    }
}