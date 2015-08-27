using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DIO_FALL15.Models
{
    public class Back
    {
        // Attributes.
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProjectId { get; set; }
        public DateTime BackedDate { get; set; }
        public decimal BackAmount { get; set; }

        // Reference.
        public virtual User User { get; set; }
        public virtual Project Project { get; set; }
    }
}