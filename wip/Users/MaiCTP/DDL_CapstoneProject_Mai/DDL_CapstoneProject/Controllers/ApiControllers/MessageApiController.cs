using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using DDL_CapstoneProject.Helper;
using DDL_CapstoneProject.Models.DTOs;
using DDL_CapstoneProject.Respository;
using DDL_CapstoneProject.Ultilities;

namespace DDL_CapstoneProject.Controllers.ApiControllers
{
    public class MessageApiController : ApiController
    {
        /// <summary>
        /// api/MessageApi/NewMessage
        /// </summary>
        /// <param name="newMessage">new message content</param>
        /// <returns>ConversationBasicDTO</returns>
        [ResponseType(typeof(HttpMessageDTO))]
        [HttpPost]
        public IHttpActionResult NewMessage(NewMessageDTO newMessage)
        {
            ConversationBasicDTO result = null;
            if (!ModelState.IsValid)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            if (User.Identity == null || !User.Identity.IsAuthenticated) 
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }

            try
            {
                result = MessageRepository.Instance.CreateNewConversation(newMessage, User.Identity.Name);
            }
            catch (UserNotFoundException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Không tìm thấy địa chỉ người nhận!", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }
            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "", Data = result });
        }

        /// <summary>
        /// get list sent conversation of a users
        /// </summary>
        /// <returns>List ConversationBasicDTO</returns>
        [ResponseType(typeof(ConversationBasicDTO))]
        [HttpGet]
        public IHttpActionResult GetListSentConversation()
        {
            List<ConversationBasicDTO> listSentConversation = null;
            if (!ModelState.IsValid)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }

            try
            {
                listSentConversation = MessageRepository.Instance.GetListSentConversation(User.Identity.Name);
            }
            catch (UserNotFoundException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Không tìm thấy địa chỉ người nhận!", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }
            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "", Data = listSentConversation });
        }

        /// <summary>
        /// get list received conversation of a users
        /// </summary>
        /// <returns>List ConversationBasicDTO</returns>
        [ResponseType(typeof(ConversationBasicDTO))]
        [HttpGet]
        public IHttpActionResult GetListReceivedConversations()
        {
            List<ConversationBasicDTO> listReceivedConversation = null;
            if (!ModelState.IsValid)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }

            try
            {
                listReceivedConversation = MessageRepository.Instance.GetListReceivedConversation(User.Identity.Name);
            }
            catch (UserNotFoundException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Không tìm thấy địa chỉ người nhận!", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }
            return Ok(new HttpMessageDTO
            {
                Status = DDLConstants.HttpMessageType.SUCCESS,
                Message = "",
                Type = "",
                Data = listReceivedConversation
            });
        }

        /// <summary>
        /// Get messages of a conversation
        /// </summary>
        /// <param name="Id">identify number of a conversation</param>
        /// <returns>ConversationDetailDTO</returns>
        [ResponseType(typeof(ConversationDetailDTO))]
        [HttpGet]
        public IHttpActionResult GetConversation(int Id)
        {
            ConversationDetailDTO conversationDetail = null;
            if (!ModelState.IsValid)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }

            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }

            try
            {
                conversationDetail = MessageRepository.Instance.GetConveration(Id,User.Identity.Name);
            }
            catch (KeyNotFoundException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Không tìm thấy tin nhắn!", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }
            return Ok(new HttpMessageDTO
            {
                Status = DDLConstants.HttpMessageType.SUCCESS,
                Message = "",
                Type = "",
                Data = conversationDetail
            });
        }

        /// <summary>
        /// Reply a message
        /// </summary>
        /// <param name="reply">reply content</param>
        /// <returns>MessageDTO</returns>
        [ResponseType(typeof(MessageDTO))]
        [HttpPost]
        public IHttpActionResult Reply(ReplyDTO reply)
        {
            MessageDTO result = null;

            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }

            try
            {
                result = MessageRepository.Instance.ReplyMessage(reply, User.Identity.Name);
            }
            catch (UserNotFoundException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Không tìm thấy địa chỉ người nhận!", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (KeyNotFoundException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Không tìm thấy tin nhắn!", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }
            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "", Data = result });
        }
        
        /// <summary>
        /// Delete Message
        /// </summary>
        /// <param name="id">Identify number of conversation</param>
        /// <returns>HttpMessageDTO</returns>
        [ResponseType(typeof(HttpMessageDTO))]
        [HttpDelete]
        public IHttpActionResult DeleteMessage(int id)
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }

            try
            {
                var result = MessageRepository.Instance.Delete(id, User.Identity.Name);
            }
            catch (UserNotFoundException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Không tìm thấy địa chỉ người nhận!", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (KeyNotFoundException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Không tìm thấy tin nhắn!", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }
            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = ""});
        }

        /// <summary>
        /// Delete List of Messages
        /// </summary>
        /// <param name="idList">list of identify number of conversations</param>
        /// <returns>HttpMessageDTO</returns>
        [ResponseType(typeof(HttpMessageDTO))]
        [HttpDelete]
        public IHttpActionResult DeleteMessageList([FromUri] int[] ids)
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }

            try
            {
                var result = MessageRepository.Instance.DeleteMessageList(ids, User.Identity.Name);
            }
            catch (UserNotFoundException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Không tìm thấy địa chỉ người nhận!", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (KeyNotFoundException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Không tìm thấy tin nhắn!", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }
            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "" });
        }
    }
}
