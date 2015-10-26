using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using DDL_CapstoneProject.Models;

namespace DDL_CapstoneProject
{
    public class Backing
    {
        #region "Attributes"

        public int BackingID { get; set; }
        public int UserID { get; set; }
        public int ProjectID { get; set; }
        public DateTime BackedDate { get; set; }
        public bool IsPublic { get; set; }
        public bool IsDelivery { get; set; }
        #endregion

        public virtual BackingDetail BackingDetail { get; set; }
        [ForeignKey("UserID")]
        public virtual DDL_User User { get; set; }
        public virtual Project Project { get; set; }
    }
}