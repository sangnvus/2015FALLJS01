using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models
{
    public class Remind
    {
        #region "Attributes"

        public int RemindID { get; set; }
        public int UserID { get; set; }
        public int ProjectID { get; set; }
        #endregion

        [ForeignKey("UserID")]
        public virtual DDL_User User { get; set; }

        [ForeignKey("ProjectID")]
        public virtual Project Project { get; set; }
    }
}