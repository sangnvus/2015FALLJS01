using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models
{
    public class Slide
    {
        #region "Attributes"

        public int SlideID { get; set; }
        public int Order { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string SlideUrl { get; set; }
        public string ButtonColor { get; set; }
        public string ButtonText { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }

        #endregion
    }
}