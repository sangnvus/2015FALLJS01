using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models.DTOs
{
    public class BackingDTO
    {
        //attributes
        public List<SumAmount> sumAmount { get; set; }
        public List<BackerDTO> listBacker { get; set; }
        public List<string> Date { get; set; }
        public List<decimal> Amount { get; set; }
    }

    public class BackerDTO
    {
        public string FullName { get; set; }
        public decimal Amount { get; set; }
        public DateTime? Date { get; set; }
    }

    public class SumAmount
    {
        public decimal Amount { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}