using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models.DTOs
{
    public class ProjectCreateDTO
    {
        public int CreatorID { get; set; }

        public int CategoryID { get; set; }

        public string Title { get; set; }

        public decimal FundingGoal { get; set; }
    }
}