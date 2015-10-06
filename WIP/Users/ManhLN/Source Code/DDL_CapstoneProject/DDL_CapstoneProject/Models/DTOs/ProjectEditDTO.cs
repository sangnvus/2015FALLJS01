using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models.DTOs
{
    public class ProjectEditDTO
    {
        // Attributes
        public string Title { get; set; }
        public string Risk { get; set; }
        public string ImageUrl { get; set; }
        public string SubDescription { get; set; }
        public string Location { get; set; }
        public DateTime? ExpireDate { get; set; }
        public decimal FundingGoal { get; set; }
        public string Description { get; set; }
        public string VideoUrl { get; set; }
        public string Status { get; set; }
        public int CategoryID { get; set; }
        public int CreatorID { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<UpdateLog> UpdateLogs { get; set; }
        public virtual ICollection<Timeline> Timelines { get; set; }
        public virtual ICollection<RewardPkg> RewardPkgs { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}