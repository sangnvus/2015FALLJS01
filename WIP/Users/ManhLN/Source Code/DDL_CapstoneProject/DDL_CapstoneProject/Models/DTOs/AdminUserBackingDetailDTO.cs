using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models.DTOs
{
    public class AdminUserBackingDetailDTO
    {
        public int CategoryID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string ProjectTitle { get; set; }
        public decimal PledgedAmount { get; set; }
        public decimal Total { get; set; }
        public DateTime? BackedDate { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public decimal Reward { get; set; }



    }
}