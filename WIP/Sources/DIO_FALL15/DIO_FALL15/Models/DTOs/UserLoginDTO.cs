using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DIO_FALL15.Models.DTOs
{
    public class UserLoginDTO
    {
        [DisplayName("User name")]
        [Required(ErrorMessage = "Username is required!")]
        [MaxLength(30)]
        [MinLength(6)]
        public string Username { get; set; }
        [DisplayName("Password")]
        [Required(ErrorMessage = "Password is required!")]
        public string Password { get; set; }
        [DisplayName("Remember Me")]
        public bool RememberMe { get; set; }
    }
}