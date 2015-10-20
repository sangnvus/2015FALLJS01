using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models.DTOs
{
    public class BackingDTO
    {
        //attributes
        public int No { get; set; }
        public string FullName { get; set; }
        public decimal Amount { get; set; }
        public DateTime? Date { get; set; }

    }
}