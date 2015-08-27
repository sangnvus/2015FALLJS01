using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DIO_FALL15.Models.DTOs
{
    public class ProjectDetailDTO
    {
        // Attributes.
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        [Required]
        [MaxLength(50), MinLength(10)]
        public string Title { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public string ImageLink { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime Deadline { get; set; }
        public decimal TargetFund { get; set; }
        public decimal CurrentFund { get; set; }

        // Reference.
        public string Username { get; set; }
        public string Category { get; set; }
    }
}