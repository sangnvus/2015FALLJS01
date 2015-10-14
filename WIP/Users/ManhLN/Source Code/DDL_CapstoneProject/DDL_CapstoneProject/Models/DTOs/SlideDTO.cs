using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models.DTOs
{
    public class SlideDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string SlideUrl { get; set; }
        public string ButtonColor { get; set; }
        public string ButtonText { get; set; }
        public string TextColor { get; set; }
        public string VideoUrl { get; set; }
    }
}