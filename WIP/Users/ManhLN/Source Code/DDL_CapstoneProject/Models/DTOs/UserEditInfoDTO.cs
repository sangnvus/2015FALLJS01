using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models.DTOs
{
    public class UserEditInfoDTO
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string ProfileImage { get; set; }
        public string Gender { get; set; }
        public DateTime LastLogin { get; set; }
        public string FacebookUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Biography { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string Addres { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}