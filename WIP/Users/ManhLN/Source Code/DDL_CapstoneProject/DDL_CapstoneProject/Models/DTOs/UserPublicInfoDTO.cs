using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models.DTOs
{
    public class UserPublicInfoDTO
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string ProfileImage { get; set; }
        public DateTime? LastLogin { get; set; }
        public string FacebookUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Biography { get; set; }
        public int CountBackedProject { get; set; }
    }
}