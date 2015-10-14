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

        public List<QuestionDTO> Questions { get; set; }
        public List<UpdateLogDTO> UpdateLogs { get; set; }
        public List<TimeLineDTO> Timelines { get; set; }
        public List<RewardPkgDTO> RewardPkgs { get; set; }
        public List<CommentDTO> Comments { get; set; }
    }
}