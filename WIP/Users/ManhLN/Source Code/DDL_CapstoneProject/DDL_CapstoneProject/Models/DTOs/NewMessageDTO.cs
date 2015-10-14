using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models.DTOs
{
    public class NewMessageDTO
    {
        public string ToUser { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}