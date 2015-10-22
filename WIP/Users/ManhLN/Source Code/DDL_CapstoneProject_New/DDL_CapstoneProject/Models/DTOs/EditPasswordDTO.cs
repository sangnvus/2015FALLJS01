using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models.DTOs
{
    public class EditPasswordDTO
    {
        public string NewPassword { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPasswordConfirm { get; set; }
        public string Email { get; set; }
        public string LoginType { get; set; }

    }
}