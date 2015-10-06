using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models
{
    public class Category
    {
        #region "Attributes"

        public int CategoryID { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }

        #endregion

        public virtual ICollection<Project> Projects { get; set; }
    }
}