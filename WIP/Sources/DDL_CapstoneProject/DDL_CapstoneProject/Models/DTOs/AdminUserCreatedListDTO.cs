using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models.DTOs
{
    public class AdminUserCreatedListDTO
    {
        public string ProjectCode { get; set; }
        public string ProjectTitle { get; set; }
        public decimal FundingGoals { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string Isfunded { get; set; }
        public double Isexpired { get; set; }
        public string Category { get; set; }
        public decimal PledgedOn { get; set; }
        public string Status { get; set; }
    }
}