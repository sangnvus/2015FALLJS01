using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models
{
    public class Question
    {
        #region "Attributes"

        public int QuestionID { get; set; }
        public int ProjectID { get; set; }
        public string QuestionContent { get; set; }
        public string Answer { get; set; }
        public DateTime CreatedDate { get; set; }
        #endregion

        public virtual Project Project { get; set; }
    }
}