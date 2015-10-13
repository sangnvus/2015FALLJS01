using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models.DTOs
{
    public class ProjectEditDTO
    {
        // Attributes
        public int ProjectID { get; set; }
        public string ProjectCode { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string SubDescription { get; set; }
        public string Location { get; set; }
        public DateTime? ExpireDate { get; set; }
        public decimal FundingGoal { get; set; }
        public string Status { get; set; }
        public int CategoryID { get; set; }
        public decimal CurrentFunded { get; set; }
    }
}