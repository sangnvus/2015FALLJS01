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
        // POST: api/ProjectApi/CreateProject
        [ResponseType(typeof(NewMessageDTO))]
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
                Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Không tìm thấy địa chỉ người nhận!", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (Exception)
            {
                Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }
            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "", Data = result });
        }

        
        [ResponseType(typeof(ConversationBasicDTO))]
        [HttpGet]
        public IHttpActionResult GetListSentConversations()
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
                Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Không tìm thấy địa chỉ người nhận!", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (Exception)
            {
                Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }
            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "", Data = listSentConversation });
        }

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
                Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Không tìm thấy địa chỉ người nhận!", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (Exception)
            {
                Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }
            return Ok(new HttpMessageDTO
            {
                Status = DDLConstants.HttpMessageType.SUCCESS,
                Message = "",
                Type = "",
                Data = listReceivedConversation
            });
        }

        [ResponseType(typeof(HttpMessageDTO))]
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
                Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Không tìm thấy tin nhắn!", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (Exception)
            {
                Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }
            return Ok(new HttpMessageDTO
            {
                Status = DDLConstants.HttpMessageType.SUCCESS,
                Message = "",
                Type = "",
                Data = conversationDetail
            });
        }

        // POST: api/ProjectApi/CreateProject
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
                Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Không tìm thấy địa chỉ người nhận!", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (KeyNotFoundException)
            {
                Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Không tìm thấy tin nhắn!", Type = DDLConstants.HttpMessageType.NOT_FOUND });
            }
            catch (Exception)
            {
                Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }
            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "", Data = result });
        }
    }
}
