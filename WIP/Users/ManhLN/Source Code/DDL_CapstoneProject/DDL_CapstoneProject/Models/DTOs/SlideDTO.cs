using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models.DTOs
{
    public class SlideDTO
    {
        public int SlideID { get; set; }
        public int Order { get; set; }
        [Required]
        [MaxLength(40)]
        [MinLength(10)]
        public string Title { get; set; }
        [Required]
        [MaxLength(200)]
        [MinLength(10)]
        public string Description { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        public string SlideUrl { get; set; }
        public string ButtonColor { get; set; }
        [Required]
        public string ButtonText { get; set; }
        [Required]
        public string TextColor { get; set; }
        public string VideoUrl { get; set; }
        public bool IsActive { get; set; }
    }
}