using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models.DTOs
{
    public class ProjectDetailDTO
    {
        public int ProjectID { get; set; }
        public string ProjectCode { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public CreatorDTO Creator { get; set; }
        public string Title { get; set; }
        public string Risk { get; set; }
        public string ImageUrl { get; set; }
        public string SubDescription { get; set; }
        public string Location { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsExprired { get; set; }
        public decimal CurrentFunded { get; set; }
        public DateTime? ExpireDate { get; set; }
        public decimal FundingGoal { get; set; }
        public string Description { get; set; }
        public string VideoUrl { get; set; }
        public int NumberBacked { get; set; }
        public List<RewardPkgDTO> RewardDetail { get; set; }
        public bool Reminded { get; set; }
        public List<QuestionDTO> Question { get; set; }

        /// <summary>
        /// Status of a project
        /// include 6 statuses:
        ///     draft,
        ///     pending,
        ///     rejected,
        ///     approved,
        ///     suspended,
        ///     expired.
        /// </summary>
        public string Status { get; set; }

        public int NumberDays { get; set; }

        public int NumberUpdate { get; set; }
        public int NumberComment { get; set; }

        //public List<CommentDTO> CommentsList { get; set; }

        //public List<UpdateLogDTO> UpdateLogsList { get; set; }
    }

    public class CreatorDTO
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public bool IsOwner { get; set; }
        public int NumberBacked { get; set; }
        public int NumberCreated { get; set; }
        public string Website { get; set; }
        public string ProfileImage { get; set; }
    }
}