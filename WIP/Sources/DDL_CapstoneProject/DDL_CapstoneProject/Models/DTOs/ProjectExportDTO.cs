using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models.DTOs
{
    public class ProjectExportDTO
    {
        public string ProjectCode { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Title { get; set; }
        public string CurrentFunded { get; set; }
        public string FundingGoal { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string Status { get; set; }
        public string IsFunded { get; set; }
        public string IsExprired { get; set; }
        public string NumberBacked { get; set; }
        public string CategoryName { get; set; }
        public string Location { get; set; }
        public string NumberUpdate { get; set; }
        public string CreatorFullname { get; set; }
        public string CreatorUserName { get; set; }
    }
}