using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models.DTOs
{
    public class AdminCategoryDTO
    {
        public int CategoryID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int RejectedProjectCount { get; set; }
        public int RunningProjectCount { get; set; }
        public int ExpiredProjectCount { get; set; }
        public int ProjectCount { get; set; }
        public bool IsActive { get; set; }

    }
}