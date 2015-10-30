using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models.DTOs
{
    public class AdminUserListDTO
    {
        public List<AdminUserDTO> ListUser { get; set; }
        public int InActiveUser { get; set; }
        public int ActiveUser { get; set; }
        public int TotalUser { get; set; }
        public int NewUser { get; set; }
    }

    public class AdminUserDTO
    {
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string LoginType { get; set; }
        public bool Status { get; set; }
        public DateTime? CreatedDate { get; set; }
    }

}