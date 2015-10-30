using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models
{
    public class Message
    {
        #region "Attributes"

        /// <summary>
        /// Identify number of message.
        /// </summary>
        public int MessageID { get; set; }

        /// <summary>
        /// Identify number of conversation.
        /// </summary>
        public int ConversationID { get; set; }

        /// <summary>
        /// Identify number of user who sent this msg.
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// Content of message.
        /// </summary>
        public string MessageContent { get; set; }
        
        /// <summary>
        /// Sent datetime.
        /// </summary>
        public DateTime SentTime { get; set; }

        #endregion

        public virtual Conversation Conversation { get; set; }

        [ForeignKey("UserID")]
        [InverseProperty("SentMessages")]
        public virtual DDL_User Sender { get; set; }
    }
}