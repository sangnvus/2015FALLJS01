using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models.DTOs
{
    public class StatisticDTO
    {
        public int SuccesedCount { get; set; }
        public decimal TotalFunded { get; set; }
        public int BackingUserCount { get; set; }
        public int UserBackmuchCount { get; set; }
        public int NumberOfBacking { get; set; }
    }
}