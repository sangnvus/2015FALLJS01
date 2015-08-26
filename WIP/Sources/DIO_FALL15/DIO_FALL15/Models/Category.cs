using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DIO_FALL15.Models
{
    public class Category
    {
        // Attributes.

        public int Id { get; set; }
        public string Name { get; set; }
        public Status Status { get; set; }
        public string Description { get; set; }
    }

    public enum Status
    {
        Active,
        Deactive
    }
}