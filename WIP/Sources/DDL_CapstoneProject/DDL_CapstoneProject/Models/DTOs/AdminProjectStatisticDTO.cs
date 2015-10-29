using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models.DTOs
{
    public class AdminProjectStatisticDTO
    {
        public int Created { get; set; }
        public int Succeed { get; set; }
        public int Fail { get; set; }
        public decimal Funed { get; set; }
        public int Profit { get; set; }
    }
}