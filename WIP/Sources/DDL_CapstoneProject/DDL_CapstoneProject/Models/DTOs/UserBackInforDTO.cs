using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models.DTOs
{
    public class UserBackInforDTO
    {
        public string Name { get; set; }
        public decimal TotalFunded { get; set; }
        public decimal TotalBacked { get; set; }
        public int projectCount { get; set; }

    }
}