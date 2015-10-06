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
                return Ok(new HttpMessageDTO { Status = "error", Message = "", Type = "Bad-Request" });
            }

            if (User.Identity == null || !User.Identity.IsAuthenticated) 
            {
                return Ok(new HttpMessageDTO { Status = "error", Message = "", Type = "Not-Authen" });
            }

            try
            {
                result = MessageRepository.Instance.CreateNewConversation(newMessage, User.Identity.Name);
            }
            catch (UserNotFoundException)
            {
                Ok(new HttpMessageDTO { Status = "error", Message = "Không tìm thấy địa chỉ người nhận!", Type = "UserNotFound" });
            }
            catch (Exception)
            {
                Ok(new HttpMessageDTO { Status = "error", Message = "", Type = "Bad-Request" });
            }
            return Ok(new HttpMessageDTO { Status = "success", Message = "", Type = "", Data = result});
        }

        
        [ResponseType(typeof(ConversationBasicDTO))]
        [HttpGet]
        public IHttpActionResult GetListSentConversations()
        {
            List<ConversationBasicDTO> listSentConversation = null;
            if (!ModelState.IsValid)
            {
                return Ok(new HttpMessageDTO { Status = "error", Message = "", Type = "Bad-Request" });
            }

            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok(new HttpMessageDTO { Status = "error", Message = "", Type = "Not-Authen" });
            }

            try
            {
                listSentConversation = MessageRepository.Instance.GetListSentConversation(User.Identity.Name);
            }
            catch (UserNotFoundException)
            {
                Ok(new HttpMessageDTO { Status = "error", Message = "Không tìm thấy địa chỉ người nhận!", Type = "UserNotFound" });
            }
            catch (Exception)
            {
                Ok(new HttpMessageDTO { Status = "error", Message = "", Type = "Bad-Request" });
            }
            return Ok(new HttpMessageDTO { Status = "success", Message = "", Type = "", Data = listSentConversation});
        }

        [ResponseType(typeof(ConversationBasicDTO))]
        [HttpGet]
        public IHttpActionResult GetListReceivedConversations()
        {
            List<ConversationBasicDTO> listReceivedConversation = null;
            if (!ModelState.IsValid)
            {
                return Ok(new HttpMessageDTO { Status = "error", Message = "", Type = "Bad-Request" });
            }

            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok(new HttpMessageDTO { Status = "error", Message = "", Type = "Not-Authen" });
            }

            try
            {
                listReceivedConversation = MessageRepository.Instance.GetListReceivedConversation(User.Identity.Name);
            }
            catch (UserNotFoundException)
            {
                Ok(new HttpMessageDTO { Status = "error", Message = "Không tìm thấy địa chỉ người nhận!", Type = "UserNotFound" });
            }
            catch (Exception)
            {
                Ok(new HttpMessageDTO { Status = "error", Message = "", Type = "Bad-Request" });
            }
            return Ok(new HttpMessageDTO
            {
                Status = "success",
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
                return Ok(new HttpMessageDTO { Status = "error", Message = "", Type = "Bad-Request" });
            }

            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok(new HttpMessageDTO { Status = "error", Message = "", Type = "Not-Authen" });
            }

            try
            {
                conversationDetail = MessageRepository.Instance.GetConveration(Id,User.Identity.Name);
            }
            catch (KeyNotFoundException)
            {
                Ok(new HttpMessageDTO { Status = "error", Message = "Không tìm thấy tin nhắn!", Type = "MessageNotFound" });
            }
            catch (Exception)
            {
                Ok(new HttpMessageDTO { Status = "error", Message = "", Type = "Bad-Request" });
            }
            return Ok(new HttpMessageDTO
            {
                Status = "success",
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
                return Ok(new HttpMessageDTO { Status = "error", Message = "", Type = "Not-Authen" });
            }

            try
            {
                result = MessageRepository.Instance.ReplyMessage(reply, User.Identity.Name);
            }
            catch (UserNotFoundException)
            {
                Ok(new HttpMessageDTO { Status = "error", Message = "Không tìm thấy địa chỉ người nhận!", Type = "UserNotFound" });
            }
            catch (KeyNotFoundException)
            {
                Ok(new HttpMessageDTO { Status = "error", Message = "Không tìm thấy tin nhắn!", Type = "MessageNotFound" });
            }
            catch (Exception)
            {
                Ok(new HttpMessageDTO { Status = "error", Message = "", Type = "Bad-Request" });
            }
            return Ok(new HttpMessageDTO { Status = "success", Message = "", Type = "", Data = result });
        }
    }
}
