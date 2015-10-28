using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models.DTOs
{
    public class AdminUserDashboardDTO
    {
        public List<RecentUserDTO> RecentUser { get; set; }
        public List<TopBackerDTO> TopBacker { get; set; }
        public List<TopCreatorDTO> TopCreator { get; set; }
        public List<NewUserDTO> ListNewUser { get; set; }
        public int NotVerifiedUser { get; set; }
        public int VerifiedUser { get; set; }
        public int TotalUser { get; set; }
        public int NewUser { get; set; }
        public int Creator { get; set; }
        public int Backer { get; set; }
        public int IdleUser { get; set; }
    }

    public class RecentUserDTO
    {
        public string AvartaURL { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public bool Status { get; set; }
        public DateTime? LastLogin { get; set; }
    }
    public class TopBackerDTO
    {
        public string AvartaURL { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public bool Status { get; set; }
        public int TotalProject { get; set; }
        public decimal TotalPledgedAmount { get; set; }
    }
    public class TopCreatorDTO
    {
        public string AvartaURL { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public bool Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int TotalSuccessProject { get; set; }
        public decimal TotalPledgedAmount { get; set; }
    }
    public class NewUserDTO
    {
        public string AvartaURL { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool Status { get; set; }
        public DateTime? CreatedDate { get; set; }
    } 



}