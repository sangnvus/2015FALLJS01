using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models.DTOs
{
    public class TimeLineDTO
    {
        public int TimelineID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public DateTime DueDate { get; set; }
    }
}