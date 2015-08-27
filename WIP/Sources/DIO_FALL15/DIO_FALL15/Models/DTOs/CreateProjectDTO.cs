using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DIO_FALL15.Models.DTOs
{
    public class CreateProjectDTO
    {
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageLink { get; set; }
        public DateTime Deadline { get; set; }
        public decimal TargetFund { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal CurrentFund { get; set; }
        public Status Status { get; set; }
    }
}