using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models
{
    public class RewardPkg
    {
        #region "Attributes"

        public int RewardPkgID { get; set; }
        public int ProjectID { get; set; }

        /// <summary>
        /// Type of reward
        /// there are 3 type:
        ///     no reward
        ///     unlimited
        ///     limited
        /// </summary>
        public string Type { get; set; }
        public decimal PledgeAmount { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public DateTime? EstimatedDelivery { get; set; }
        public bool IsHide { get; set; }

        #endregion

        public virtual Project Project { get; set; }
        public virtual ICollection<BackingDetail> BackingDetails { get; set; }
    }

    #region "Enum"

    //public enum RewardType
    //{
    //    NoReward = 1,
    //    Unlimited = 2,
    //    Limited = 3
    //}
    #endregion
}