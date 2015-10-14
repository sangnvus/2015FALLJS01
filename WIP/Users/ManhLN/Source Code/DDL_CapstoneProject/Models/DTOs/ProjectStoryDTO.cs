using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models.DTOs
{
    public class ProjectStoryDTO
    {
        // Attributes
        public int ProjectID { get; set; }
        public string Risk { get; set; }
        public string Description { get; set; }
        public string VideoUrl { get; set; }

    }
}