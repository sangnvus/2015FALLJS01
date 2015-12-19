using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models
{
    public class BackingDetail
    {
        #region Attributes

        [Key, ForeignKey("Backing")]
        public int BackingID { get; set; }
        public string BackerName { get; set; }
        public int RewardPkgID { get; set; }
        public decimal PledgedAmount { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string OrderId { get; set; }
        public string TransactionId { get; set; }
        #endregion

        public virtual Backing Backing { get; set; }
        public virtual RewardPkg RewardPkg { get; set; }
    }
}