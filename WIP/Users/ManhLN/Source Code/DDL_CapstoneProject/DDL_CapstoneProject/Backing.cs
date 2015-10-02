using System;
using System.Collections.Generic;
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
        public int RewardPkgID { get; set; }
        public DateTime BackedDate { get; set; }
        public decimal PledgedAmount { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        #endregion

        public virtual DDL_User User { get; set; }
        public virtual RewardPkg RewardPkg { get; set; }
    }
}