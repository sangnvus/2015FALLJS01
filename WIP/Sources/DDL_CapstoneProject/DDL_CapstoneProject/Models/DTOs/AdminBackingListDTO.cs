using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models.DTOs
{
    public class AdminBackingListDTO
    {
        public string ImageURL { get; set; }
        public string ProjectTitle { get; set; }
        public string BackerName { get; set; }
        public decimal PledgeAmount { get; set; }
        public DateTime? BackedDate { get; set; }
        public string UserName { get; set; }
        public int BackingID { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Biography { get; set; }
        public string Content { get; set; }
        public string RewardContent { get; set; }
        public decimal RewardPledgeAmount { get; set; }
        public int RewardID { get; set; }
        public string ProjectCode { get; set; }
    }

}