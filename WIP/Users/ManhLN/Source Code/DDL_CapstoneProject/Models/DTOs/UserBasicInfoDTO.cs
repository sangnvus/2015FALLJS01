using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models.DTOs
{
    public class UserBasicInfoDTO
    {
        public string UserName { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
        public string LoginType { get; set; }
        public string FullName { get; set; }
        public string ProfileImage { get; set; }
    }
}