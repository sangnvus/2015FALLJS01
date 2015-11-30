using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models.DTOs
{
    public class UserRegisterDTO
    {
        /// <summary>
        /// Username is unique.
        /// </summary>
        [Required]
        [MinLength(8)]
        [MaxLength(20)]
        public string Username { get; set; }
        /// <summary>
        /// This password is encrypted by md5 code.
        /// </summary>
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string FullName { get; set; }
    }
}