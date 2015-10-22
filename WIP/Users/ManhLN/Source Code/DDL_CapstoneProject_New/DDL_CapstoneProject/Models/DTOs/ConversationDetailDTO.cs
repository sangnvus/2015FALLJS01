using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models.DTOs
{
    public class ConversationDetailDTO
    {
        public int ConversationID { get; set; }
        public string Subject { get; set; }
        public bool IsSent { get; set; }
        public List<MessageDTO> MessageList { get; set; }
    }
}