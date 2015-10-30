using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models.DTOs
{
    public class AdminProjectStatisticDTO
    {
        public List<Statistic> Created { get; set; }

        public List<Statistic> Succeed { get; set; }
        public List<Statistic> Fail { get; set; }
        public List<Statistic> Funded { get; set; }
        public List<Statistic> Profit { get; set; }
    }
    public class Statistic
    {
        public decimal Amount { get; set; }
        public int Month { get; set; }
    }
}