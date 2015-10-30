using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models.DTOs
{
    public class AdminProjectGeneralInfoDTO
    {
        public int PendingProject { get; set; }
        public int ApprovedProject { get; set; }
        public int SuspendedProject { get; set; }
        public int ExpriredProject { get; set; }
        public int SucceedProject { get; set; }
        public int TotalProject { get; set; }
    }
}