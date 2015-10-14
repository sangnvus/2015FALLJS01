using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models.DTOs
{
    public class ConversationBasicDTO
    {
        public int ConversationID { get; set; }
        public string SenderName { get; set; }
        public string ReceiverName { get; set; }
        public string Title { get; set; }
        public bool IsRead { get; set; }
        public bool IsSent { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}