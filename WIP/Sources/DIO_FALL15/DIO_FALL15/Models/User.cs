using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Web;

namespace DIO_FALL15.Models
{
    public class User
    {
        // Attributes.

        public int Id { get; set; }
        /// <summary>
        /// Username is unique.
        /// </summary>
        [Required]
        [MaxLength(20), MinLength(8)]
        public string Username { get; set; }
        /// <summary>
        /// This password is encrypted by md5 code.
        /// </summary>
        [Required]
        public string Password { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        [Required]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        [MaxLength(100)]
        public string Address { get; set; }

        // Reference List.
        public ICollection<Project> Projects { get; set; }
        public ICollection<Back> Backs { get; set; } 
    }

    public enum Gender
    {
        Male,
        Female
    }
}