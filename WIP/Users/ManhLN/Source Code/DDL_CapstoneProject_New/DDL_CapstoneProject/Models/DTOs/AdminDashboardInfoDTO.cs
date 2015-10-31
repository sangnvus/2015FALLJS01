using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models.DTOs
{
    public class AdminDashboardInfoDTO
    {
        public int TotalUser { get; set; }
        public int TotalProject { get; set; }
        public decimal TotalFund { get; set; }
        public decimal TotalProfit { get; set; }
        public int NewProject { get; set; }
        public int LiveProject { get; set; }
        public int RankA { get; set; }
        public int RankB { get; set; }
        public int RankC { get; set; }
        public int RankD { get; set; }
        public int TotalSucceed { get; set; }
        public int TotalFail { get; set; }
    }
}