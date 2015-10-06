using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models
{
    public class UpdateLog
    {
        #region "Attibutes"

        public int UpdateLogID { get; set; }
        public int ProjectID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        #endregion

        public virtual Project Project { get; set; }
    }
}