using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models.DTOs
{
    public class AdminBakingFullInforDTO
    {
        public string ProjectCode { get; set; }
        public string ProjectTitle { get; set; }


        public int RewardID { get; set; }
        public string RewardDes { get; set; }
        public string RewardEstimatedDelivery { get; set; }

        public int BackingID { get; set; }
        public decimal BackingPledgeAmount { get; set; }
        public decimal BackingQuantity { get; set; }
        public string BackingDes { get; set; }
        public string BackedDate { get; set; }

        public string BackerName { get; set; }
        public string BackerUserName { get; set; }
        public string BackerEmail { get; set; }
        public string BackerAddress { get; set; }
        public string BackerPhoneNumber { get; set; }


    }
}