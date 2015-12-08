using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models.DTOs
{
    public class ProjectBackDTO
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
        public int Quantity { get; set; }
        public string Description { get; set; }
        public string BackerName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ProjectCode { get; set; }
        public bool IsPublic { get; set; }
        public string ProjectName { get; set; }
        public string BackerImg { get; set; }
        public string BackerUsername { get; set; }
        public string ProjectOwner { get; set; }
        public string ProjectOwnerUsername { get; set; }
        public string RewardPkgDesc { get; set; }
        public string RewardPkgType { get; set; }
        public DateTime BackedDate { get; set; }
        public string OrderId { get; set; }
        public string TransactionId { get; set; }
    }
}