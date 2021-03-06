﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DIO_FALL15.Models.DTOs
{
    public class ProjectCardDTO
    {
        // Attributes.
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageLink { get; set; }
        public DateTime Deadline { get; set; }
        public decimal TargetFund { get; set; }
        public decimal CurrentFund { get; set; }
    }
}