using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models
{
    public class BackingDetail
    {
        #region Attributes

        [Key,ForeignKey("Backing")]
        public int BackingID { get; set; }
        public int RewardPkgID { get; set; }
        public decimal PledgedAmount { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        #endregion

        public virtual Backing Backing { get; set; }
        public virtual RewardPkg RewardPkg { get; set; }
    }
}