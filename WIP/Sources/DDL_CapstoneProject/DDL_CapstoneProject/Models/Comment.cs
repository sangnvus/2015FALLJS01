using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models
{
    public class Comment
    {
        #region "Attributes"

        public int CommentID { get; set; }
        public int UserID { get; set; }
        public int ProjectID { get; set; }
        public string CommentContent { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsEdited { get; set; }
        public bool IsHide { get; set; }
        #endregion

        [ForeignKey("UserID")]
        public virtual DDL_User User { get; set; }

        [ForeignKey("ProjectID")]
        public virtual Project Project { get; set; }
    }
}