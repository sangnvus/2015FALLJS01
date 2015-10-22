using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using DDL_CapstoneProject.Helper;
using DDL_CapstoneProject.Helpers;
using DDL_CapstoneProject.Models;
using DDL_CapstoneProject.Models.DTOs;
using DDL_CapstoneProject.Ultilities;
using WebGrease.Css.Extensions;

namespace DDL_CapstoneProject.Respository
{
    public class MessageRepository : SingletonBase<MessageRepository>
    {
        private DDLDataContext db;

        private MessageRepository()
        {
            db = new DDLDataContext();
        }

        public ConversationBasicDTO CreateNewConversation(NewMessageDTO newMessage, string senderName)
        {
            var receiver = UserRepository.Instance.GetByUserNameOrEmail(newMessage.ToUser);
            if (receiver == null)
            {
                throw new UserNotFoundException();
            }
            else
            {
                var creator = UserRepository.Instance.GetByUserNameOrEmail(senderName);

                // Create conversation.
                var newConversation = db.Conversations.Create();
                newConversation.CreatedDate = DateTime.UtcNow;
                newConversation.UpdatedDate = DateTime.UtcNow;
                newConversation.CreatorID = creator.DDL_UserID;
                newConversation.ReceiverID = receiver.DDL_UserID;
                newConversation.Subject = newMessage.Title;
                newConversation.ViewStatus = DDLConstants.ConversationStatus.CREATOR;
                newConversation.DeleteStatus = DDLConstants.ConversationStatus.NO;

                // Create message.
                var message = db.Messages.Create();
                message.MessageContent = newMessage.Content;
                message.UserID = creator.DDL_UserID;
                message.SentTime = DateTime.UtcNow;

                // Add message into conversation.
                newConversation.Messages = new List<Message>
                {
                    message
                };

                db.Conversations.Add(newConversation);
                db.SaveChanges();

                var conversation = new ConversationBasicDTO
                {
                    ConversationID = newConversation.ConversationID,
                    UpdatedDate = CommonUtils.ConvertDateTimeFromUtc(newConversation.UpdatedDate.GetValueOrDefault()),
                    Title = newConversation.Subject,
                    IsRead = true,
                    IsSent = true,
                    SenderName = creator.UserInfo.FullName,
                    ReceiverName = receiver.UserInfo.FullName
                };
                return conversation;
            }
        }

        public List<ConversationBasicDTO> GetListSentConversation(string userName)
        {
            var listConversation = from conversation in db.Conversations
                                   orderby conversation.UpdatedDate descending
                                   where conversation.Creator.Username == userName
                                   && (conversation.DeleteStatus == DDLConstants.ConversationStatus.NO
                                   || conversation.DeleteStatus == DDLConstants.ConversationStatus.RECEIVER)
                                   select new ConversationBasicDTO
                                   {
                                       ConversationID = conversation.ConversationID,
                                       SenderName = conversation.Creator.UserInfo.FullName,
                                       ReceiverName = conversation.Receiver.UserInfo.FullName,
                                       Title = conversation.Subject,
                                       UpdatedDate = conversation.UpdatedDate,
                                       IsSent = conversation.Creator.Username == userName,
                                       IsRead = (conversation.Creator.Username == userName && conversation.ViewStatus == DDLConstants.ConversationStatus.CREATOR)
                                               || (conversation.Creator.Username != userName && conversation.ViewStatus == DDLConstants.ConversationStatus.RECEIVER)
                                               || conversation.ViewStatus == DDLConstants.ConversationStatus.BOTH
                                   };

            listConversation.ForEach(x => x.UpdatedDate = CommonUtils.ConvertDateTimeFromUtc(x.UpdatedDate.GetValueOrDefault()));

            return listConversation.ToList();
        }

        public List<ConversationBasicDTO> GetListReceivedConversation(string userName)
        {
            var listConversation = from conversation in db.Conversations
                                   orderby conversation.UpdatedDate descending
                                   where conversation.Receiver.Username == userName
                                   && (conversation.DeleteStatus == DDLConstants.ConversationStatus.CREATOR
                                   || conversation.DeleteStatus == DDLConstants.ConversationStatus.NO)
                                   select new ConversationBasicDTO
                                   {
                                       ConversationID = conversation.ConversationID,
                                       SenderName = conversation.Creator.UserInfo.FullName,
                                       ReceiverName = conversation.Receiver.UserInfo.FullName,
                                       Title = conversation.Subject,
                                       UpdatedDate = conversation.UpdatedDate,
                                       IsSent = conversation.Creator.Username == userName,
                                       IsRead = (conversation.Creator.Username == userName && conversation.ViewStatus == DDLConstants.ConversationStatus.CREATOR)
                                               || (conversation.Creator.Username != userName && conversation.ViewStatus == DDLConstants.ConversationStatus.RECEIVER)
                                               || conversation.ViewStatus == DDLConstants.ConversationStatus.BOTH
                                   };

            listConversation.ForEach(x => x.UpdatedDate = CommonUtils.ConvertDateTimeFromUtc(x.UpdatedDate.GetValueOrDefault()));

            return listConversation.ToList();
        }

        public ConversationDetailDTO GetConveration(int conversationID, string userName)
        {
            var conversation = db.Conversations.FirstOrDefault(c => c.ConversationID == conversationID);
            if (conversation == null)
            {
                throw new KeyNotFoundException();
            }
            // Change viewstate.
            conversation.ViewStatus = DDLConstants.ConversationStatus.BOTH;

            // Save to DB.
            db.Conversations.AddOrUpdate(conversation);
            db.SaveChanges();

            var conversationDTO = new ConversationDetailDTO
            {
                ConversationID = conversation.ConversationID,
                IsSent = conversation.Creator.Username == userName,
                Subject = conversation.Subject,
            };

            // Get list messages.
            var listMessage = from message in db.Messages
                              where message.ConversationID == conversationID
                              orderby message.SentTime ascending
                              select new MessageDTO
                              {
                                  MessageID = message.MessageID,
                                  Content = message.MessageContent,
                                  SentTime = message.SentTime,
                                  SenderID = message.Sender.DDL_UserID,
                                  SenderName = message.Sender.UserInfo.FullName,
                                  SenderProfileImage = message.Sender.UserInfo.ProfileImage
                              };

            var listMessageDTO = listMessage.ToList();

            // Convert datetime to GMT+7
            listMessageDTO.ForEach(x => x.SentTime = CommonUtils.ConvertDateTimeFromUtc(x.SentTime));

            conversationDTO.MessageList = listMessageDTO;

            return conversationDTO;
        }

        /// <summary>
        /// ReplyMessage
        /// </summary>
        /// <param name="reply"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public MessageDTO ReplyMessage(ReplyDTO reply, string userName)
        {
            // Check user exist.
            var user = UserRepository.Instance.GetByUserNameOrEmail(userName);
            if (user == null)
            {
                throw new UserNotFoundException();
            }

            Conversation conversation = null;
            if (reply != null)
            {
                // Check conversation exist.
                conversation = db.Conversations.FirstOrDefault(c => c.ConversationID == reply.ConversationID);
            }
            if (conversation == null)
            {
                throw new KeyNotFoundException();
            }

            // Create new message.
            var newMessage = db.Messages.Create();
            newMessage.ConversationID = reply.ConversationID;
            newMessage.MessageContent = reply.Content;
            newMessage.UserID = user.DDL_UserID;
            newMessage.SentTime = DateTime.UtcNow;

            // Change viewstate.
            conversation.UpdatedDate = DateTime.UtcNow;
            conversation.ViewStatus = conversation.Creator.Username == userName
                ? DDLConstants.ConversationStatus.CREATOR
                : DDLConstants.ConversationStatus.RECEIVER;

            // Add to DB.
            db.Conversations.AddOrUpdate(conversation);
            db.Messages.Add(newMessage);
            db.SaveChanges();

            // Create return message data.
            var messageDTO = new MessageDTO
            {
                SentTime = CommonUtils.ConvertDateTimeFromUtc(newMessage.SentTime),
                Content = newMessage.MessageContent,
                MessageID = newMessage.MessageID,
                SenderID = user.DDL_UserID,
                SenderName = user.UserInfo.FullName,
                SenderProfileImage = user.UserInfo.ProfileImage
            };

            return messageDTO;
        }

        /// <summary>
        /// Delete a conversation
        /// </summary>
        /// <param name="conversationId">conversation Id</param>
        /// <param name="userName">userName</param>
        /// <returns>bool value</returns>
        public bool Delete(int conversationId, string userName)
        {
            // Check user exist.
            var user = UserRepository.Instance.GetByUserNameOrEmail(userName);
            if (user == null)
            {
                throw new UserNotFoundException();
            }

            Conversation conversation = null;

            // Check conversation exist.
            conversation = db.Conversations.FirstOrDefault(c => c.ConversationID == conversationId
                            && (c.CreatorID == user.DDL_UserID || c.ReceiverID == user.DDL_UserID));

            if (conversation == null)
            {
                throw new KeyNotFoundException();
            }

            // Edit delete status.
            // if is creator.
            if (conversation.CreatorID == user.DDL_UserID)
            {
                conversation.DeleteStatus = conversation.DeleteStatus == DDLConstants.ConversationStatus.RECEIVER ?
                    DDLConstants.ConversationStatus.BOTH : DDLConstants.ConversationStatus.CREATOR;
            }
            else // if is receiver.
            {
                conversation.DeleteStatus = conversation.DeleteStatus == DDLConstants.ConversationStatus.CREATOR ?
                    DDLConstants.ConversationStatus.BOTH : DDLConstants.ConversationStatus.RECEIVER;
            }

            // Save in DB.
            db.Conversations.AddOrUpdate(conversation);
            db.SaveChanges();

            return true;
        }

        /// <summary>
        /// Delete List of Messages
        /// </summary>
        /// <param name="idList">list of identify number of conversations</param>
        /// <param name="userName">user name of current user</param>
        /// <returns></returns>
        public bool DeleteMessageList(int[] idList, string userName)
        {
            // Check user exist.
            var user = UserRepository.Instance.GetByUserNameOrEmail(userName);
            if (user == null)
            {
                throw new UserNotFoundException();
            }

            // Check conversation exist.
            var conversationList = db.Conversations.Where(c => idList.Contains(c.ConversationID));

            if (!conversationList.Any())
            {
                throw new KeyNotFoundException();
            }

            // Edit delete status.
            foreach (var conversation in conversationList)
            {
                if (conversation.CreatorID == user.DDL_UserID) // if is creator.
                {
                    conversation.DeleteStatus = conversation.DeleteStatus == DDLConstants.ConversationStatus.RECEIVER ?
                        DDLConstants.ConversationStatus.BOTH : DDLConstants.ConversationStatus.CREATOR;
                }
                else // if is receiver.
                {
                    conversation.DeleteStatus = conversation.DeleteStatus == DDLConstants.ConversationStatus.CREATOR ?
                        DDLConstants.ConversationStatus.BOTH : DDLConstants.ConversationStatus.RECEIVER;
                }
            }

            // Save in DB.
            db.SaveChanges();

            return true;
        }
    }
}