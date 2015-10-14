using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models.DTOs
{
    public class CategoryProjectCountDTO
    {
        public int CategoryID { get; set; }
        public string Name { get; set; }
        public int projectCount { get; set; }
    }
}