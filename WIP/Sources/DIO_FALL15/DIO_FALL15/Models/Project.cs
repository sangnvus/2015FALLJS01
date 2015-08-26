using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DIO_FALL15.Models
{
    public class Project
    {
        // Attributes.
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        [Required]
        [MaxLength(50),MinLength(10)]
        public string Title { get; set; }
        public Status Status { get; set; }
        public string Description { get; set; }
        public string ImageLink { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime Deadline { get; set; }
        public decimal TargetFund { get; set; }
        public decimal CurrentFund { get; set; }

        // Reference.
        public virtual User User { get; set; }
        public virtual Category Category { get; set; }
    }
}