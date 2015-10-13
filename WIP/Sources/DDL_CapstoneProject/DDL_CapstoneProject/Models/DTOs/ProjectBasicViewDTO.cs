using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models.DTOs
{
    public class ProjectBasicViewDTO
    {
        public ProjectBasicViewDTO()
        {
            CurrentFunded = 0;
            CurrentFundedNumber = 0;
            FundingGoal = 0;
            Backers = 0;
            PopularPoint = 0;
        }
        public int ProjectID { get; set; }
        public string CreatorName { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string SubDescription { get; set; }
        public string Location { get; set; }
        public decimal CurrentFunded { get; set; }
        public decimal CurrentFundedNumber { get; set; }
        public int? ExpireDate { get; set; }
        public decimal FundingGoal { get; set; }
        public string Category { get; set; }
        public int Backers { get; set; }
        public DateTime CreatedDate { get; set; }
        public int PopularPoint { get; set; }
    }
}