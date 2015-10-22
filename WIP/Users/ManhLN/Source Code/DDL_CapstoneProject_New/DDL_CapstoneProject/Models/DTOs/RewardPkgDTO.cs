using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models.DTOs
{
    public class RewardPkgDTO
    {
        public int RewardPkgID { get; set; }
        public decimal PledgeAmount { get; set; }
        /// <summary>
        /// Type of reward
        /// there are 3 type:
        ///     no reward
        ///     unlimited
        ///     limited
        /// </summary>
        public string Type { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public DateTime? EstimatedDelivery { get; set; }
        public bool IsHide { get; set; }
        public int Backers { get; set; }
    }
}