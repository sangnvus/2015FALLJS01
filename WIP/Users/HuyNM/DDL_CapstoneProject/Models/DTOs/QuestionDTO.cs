using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models.DTOs
{
    public class QuestionDTO
    {
        public int QuestionID { get; set; }
        public string QuestionContent { get; set; }
        public string Answer { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}