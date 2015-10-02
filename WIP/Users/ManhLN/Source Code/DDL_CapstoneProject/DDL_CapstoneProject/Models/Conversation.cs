using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Models
{
    public class Conversation
    {
        #region "Attribute"

        /// <summary>
        /// Identify number of conversation.
        /// </summary>
        public int ConversationID { get; set; }

        /// <summary>
        /// Identify number of user who create this conversation.
        /// </summary>
        public int CreatorID { get; set; }

        /// <summary>
        /// Identify number of user who receive this conversation
        /// </summary>
        public int ReceiverID { get; set; }

        /// <summary>
        /// Subject of conversation.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Conversation created date.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Last updated datetime of conversation
        /// or sent time of last message.
        /// </summary>
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        /// Delete status of conversation
        /// </summary>
        public DeleteStatus DeleteStatus { get; set; }

        /// <summary>
        /// View status of conversation
        /// </summary>
        public ViewStatus ViewStatus { get; set; }

        #endregion

        [ForeignKey("CreatorID")]
        [InverseProperty("CreatedConversations")]
        public virtual DDL_User Creator { get; set; }

        [ForeignKey("ReceiverID")]
        [InverseProperty("ReceivedConversations")]
        public virtual DDL_User Receiver { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }

    #region "Enum"

    public enum ViewStatus
    {
        CreatorView = 1,
        ReceiverView = 2,
        BothView = 3
    }

    public enum DeleteStatus
    {
        CreatorDelete = 1,
        ReceiverDelete = 2,
        BothDelete = 3
    }
    #endregion
}