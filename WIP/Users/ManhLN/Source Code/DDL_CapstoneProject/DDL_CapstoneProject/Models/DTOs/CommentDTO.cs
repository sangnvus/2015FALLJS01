using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models.DTOs
{
    public class CommentDTO
    {
        public int CommentID { get; set; }
        public int UserID { get; set; }
        public string CommentContent { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsHide { get; set; }
    }
}