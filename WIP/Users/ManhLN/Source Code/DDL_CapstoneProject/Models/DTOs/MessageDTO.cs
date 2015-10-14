using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models.DTOs
{
    public class MessageDTO
    {
        public int MessageID { get; set; }
        public string Content { get; set; }
        public DateTime SentTime { get; set; }
        public string SenderName { get; set; }
        public int SenderID { get; set; }
        public string SenderProfileImage { get; set; }
    }
}