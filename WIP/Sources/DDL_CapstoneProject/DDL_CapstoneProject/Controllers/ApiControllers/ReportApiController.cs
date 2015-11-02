using DDL_CapstoneProject.Helper;
using DDL_CapstoneProject.Models.DTOs;
using DDL_CapstoneProject.Respository;
using DDL_CapstoneProject.Ultilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DDL_CapstoneProject.Controllers.ApiControllers
{
    public class ReportApiController : BaseApiController
    {

        public IHttpActionResult GetReportProjects()
        {
            List<ReportProjectDTO> listReport = null;
            // Check authen.
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }
            try
            {
                // Check role user.
                var currentUser = UserRepository.Instance.GetBasicInfo(User.Identity.Name);
                if (currentUser == null || currentUser.Role != DDLConstants.UserType.ADMIN)
                {
                    throw new NotPermissionException();
                }
                listReport = ReportResponsitoy.Instance.GetReportProjects();
            }
            catch (KeyNotFoundException)
            {
                return
                    Ok(new HttpMessageDTO
                    {
                        Status = DDLConstants.HttpMessageType.ERROR,
                        Message = "Không tìm thấy!",
                        Type = DDLConstants.HttpMessageType.NOT_FOUND
                    });
            }
            catch (NotPermissionException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Không có quyền truy cập!", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }
            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "", Data = listReport });
        }


        public IHttpActionResult GetReportUsers()
        {
            List<ReportUserDTO> listReport = null;
            // Check authen.
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }
            try
            {
                // Check role user.
                var currentUser = UserRepository.Instance.GetBasicInfo(User.Identity.Name);
                if (currentUser == null || currentUser.Role != DDLConstants.UserType.ADMIN)
                {
                    throw new NotPermissionException();
                }
                listReport = ReportResponsitoy.Instance.GetReportUsers();
            }
            catch (KeyNotFoundException)
            {
                return
                    Ok(new HttpMessageDTO
                    {
                        Status = DDLConstants.HttpMessageType.ERROR,
                        Message = "Không tìm thấy!",
                        Type = DDLConstants.HttpMessageType.NOT_FOUND
                    });
            }
            catch (NotPermissionException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Không có quyền truy cập!", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }
            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "", Data = listReport });

        }


        /// <summary>
        /// ChangeCategoryStatus
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult changeReportStatus(int id, string status, string reportType)
        {
            // Check authen.
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }
            try
            {
                // Check role user.
                var currentUser = UserRepository.Instance.GetBasicInfo(User.Identity.Name);
                if (currentUser == null || currentUser.Role != DDLConstants.UserType.ADMIN)
                {
                    throw new NotPermissionException();
                }
                ReportResponsitoy.Instance.changeReportStatus(id, status, reportType);
            }
            catch (KeyNotFoundException)
            {
                return
                    Ok(new HttpMessageDTO
                    {
                        Status = DDLConstants.HttpMessageType.ERROR,
                        Message = "Không tìm thấy!",
                        Type = DDLConstants.HttpMessageType.NOT_FOUND
                    });
            }
            catch (NotPermissionException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Không có quyền truy cập!", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }
            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "" });
        }
    }
}
