using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models.DTOs
{
    public class AdminUserBackedListDTO
    {
        public string ProjectCode { get; set; }
        public string ProjectTitle { get; set; }
        public decimal FundingGoals { get; set; }
        public string Status { get; set; }
        public string Isfunded { get; set; }
        public decimal PledgedAmount { get; set; }
    }
}