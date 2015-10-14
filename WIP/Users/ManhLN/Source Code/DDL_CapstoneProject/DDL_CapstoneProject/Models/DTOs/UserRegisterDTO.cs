using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models.DTOs
{
    public class UserRegisterDTO
    {
        /// <summary>
        /// Username is unique.
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// This password is encrypted by md5 code.
        /// </summary>
        public string Password { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
    }
}