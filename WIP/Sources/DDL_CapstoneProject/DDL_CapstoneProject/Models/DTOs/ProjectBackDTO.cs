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
    }
}