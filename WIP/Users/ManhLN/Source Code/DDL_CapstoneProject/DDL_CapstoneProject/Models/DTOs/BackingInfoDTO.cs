using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models.DTOs
{
    public class BackingInfoDTO
    {
        //attributes
     
        public string RewadDiscription { get; set; }
        public decimal PledgeAmount { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime BackedDate { get; set; }
        public string BackingDiscription { get; set; }
    }
}