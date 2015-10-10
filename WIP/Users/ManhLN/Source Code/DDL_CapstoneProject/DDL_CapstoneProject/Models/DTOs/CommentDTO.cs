using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models.DTOs
{
    public class CommentDTO
    {
        public int CommentID { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string ProfileImage { get; set; }
        public string CommentContent { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsEdited { get; set; }
        public bool IsHide { get; set; }
    }
}