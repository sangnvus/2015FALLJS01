using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DIO_FALL15.Models.DTOs
{
    public class UserDetailDTO
    {
        public int Id { get; set; }
        /// <summary>
        /// Username is unique.
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// This password is encrypted by md5 code.
        /// </summary>
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }
}