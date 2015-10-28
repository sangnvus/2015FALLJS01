using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Script.Serialization;
using DDL_CapstoneProject.Helper;
using DDL_CapstoneProject.Models;
using DDL_CapstoneProject.Respository;
using DDL_CapstoneProject.Models.DTOs;
using DDL_CapstoneProject.Ultilities;

namespace DDL_CapstoneProject.Controllers.ApiControllers
{
    public class SlideApiController : ApiController
    {
        // GET: api/SlideApi/Slides
        public IHttpActionResult GetSlides()
        {
            List<SlideDTO> listSlides;
            try
            {

                listSlides = SlideRepository.Instance.GetSlides();
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }
            return Ok(new HttpMessageDTO { Status = "success", Data = listSlides });
        }

        // GET: api/SlideApi/GetSlidesForAdmin
        public IHttpActionResult GetSlidesForAdmin()
        {
            List<SlideDTO> slideList;
            try
            {
                // Check authen.
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }  // Check role user.
                var currentUser = UserRepository.Instance.GetBasicInfo(User.Identity.Name);
                if (currentUser == null || currentUser.Role != DDLConstants.UserType.ADMIN)
                {
                    throw new NotPermissionException();
                }
                slideList = SlideRepository.Instance.GetSlidesForAdmin();
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
            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "", Data = slideList });
        }

        // GET: api/SlideApi/GetSlidesForAdmin
        [ResponseType(typeof(HttpMessageDTO))]
        [HttpPost]
        public IHttpActionResult CreateSlide()
        {

            var httpRequest = HttpContext.Current.Request;
            SlideDTO slide;
            try
            {
                // Check authen.
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }
                if (httpRequest.Form.Count <= 0)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
                }
                // Check role user.
                var currentUser = UserRepository.Instance.GetBasicInfo(User.Identity.Name);
                if (currentUser == null || currentUser.Role != DDLConstants.UserType.ADMIN)
                {
                    throw new NotPermissionException();
                }

                // Get new slide form.
                var slideJson = httpRequest.Form["slide"];
                var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };

                // For simplicity just use Int32's max value.
                // You could always read the value from the config section mentioned above.
                slide = serializer.Deserialize<SlideDTO>(slideJson);

                //Validation
                var validationResult = (from prop in TypeDescriptor.GetProperties(slide).Cast<PropertyDescriptor>()
                                        from attribute in prop.Attributes.OfType<ValidationAttribute>()
                                        where !attribute.IsValid(prop.GetValue(slide))
                                        select new { Propertie = prop.Name, ErrorMessage = attribute.FormatErrorMessage(string.Empty) }).ToList();

                if (validationResult.Count > 0)
                {
                    throw new InvalidDataException();
                }

                slide = SlideRepository.Instance.CreateSlide(slide);

                // Save image.
                var imageName = "slide_" + slide.SlideID;
                var file = httpRequest.Files["file"];
                var uploadImageName = CommonUtils.UploadImage(file, imageName, DDLConstants.FileType.SLIDE);

                // Change image link.
                slide.ImageUrl = uploadImageName;
                slide = SlideRepository.Instance.EditSlide(slide);
            }
            catch (NotPermissionException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Không có quyền truy cập", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }
            catch (InvalidDataException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Sai kiểu dữ liệu", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }
            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "", Data = slide });
        }

        /// <summary>
        /// DeleteSlide
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeleteSlide(int id)
        {
            List<SlideDTO> result;
            try
            {
                // Check authen.
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }
                // Check role user.
                var currentUser = UserRepository.Instance.GetBasicInfo(User.Identity.Name);
                if (currentUser == null || currentUser.Role != DDLConstants.UserType.ADMIN)
                {
                    throw new NotPermissionException();
                }

                result = SlideRepository.Instance.DeleteSlide(id);
            }
            catch (KeyNotFoundException)
            {
                return
                    Ok(new HttpMessageDTO
                    {
                        Status = DDLConstants.HttpMessageType.ERROR,
                        Message = "Không không tồn tại!",
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
            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "", Data = result });
        }

        /// <summary>
        /// ChangeSlideStatus
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult ChangeSlideStatus(int id)
        {
            SlideDTO slide;
            try
            {
                // Check authen.
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }
                // Check role user.
                var currentUser = UserRepository.Instance.GetBasicInfo(User.Identity.Name);
                if (currentUser == null || currentUser.Role != DDLConstants.UserType.ADMIN)
                {
                    throw new NotPermissionException();
                }
                slide = SlideRepository.Instance.ChangeSlideStatus(id);
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
            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "", Data = slide });
        }

        /// <summary>
        /// EditSlide
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult EditSlide()
        {
            var httpRequest = HttpContext.Current.Request;
            SlideDTO slide;
            try
            {
                // Check authen.
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }
                if (httpRequest.Form.Count <= 0)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
                }
                // Check role user.
                var currentUser = UserRepository.Instance.GetBasicInfo(User.Identity.Name);
                if (currentUser == null || currentUser.Role != DDLConstants.UserType.ADMIN)
                {
                    throw new NotPermissionException();
                }

                // Get new slide form.
                var slideJson = httpRequest.Form["slide"];
                var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };

                // For simplicity just use Int32's max value.
                slide = serializer.Deserialize<SlideDTO>(slideJson);

                //Validation
                var validationResult = (from prop in TypeDescriptor.GetProperties(slide).Cast<PropertyDescriptor>()
                                        from attribute in prop.Attributes.OfType<ValidationAttribute>()
                                        where !attribute.IsValid(prop.GetValue(slide))
                                        select new { Propertie = prop.Name, ErrorMessage = attribute.FormatErrorMessage(string.Empty) }).ToList();

                if (validationResult.Count > 0)
                {
                    throw new InvalidDataException();
                }

                // Save image.
                var imageName = "slide_" + slide.SlideID;
                var file = httpRequest.Files["file"];
                if (file != null)
                {
                    var uploadImageName = CommonUtils.UploadImage(file, imageName, DDLConstants.FileType.SLIDE);
                    // Change image link.
                    slide.ImageUrl = uploadImageName;
                }
                else
                {
                    slide.ImageUrl = String.Empty;
                }
                slide = SlideRepository.Instance.EditSlide(slide);
            }
            catch (NotPermissionException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Không có quyền truy cập", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }
            catch (InvalidDataException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Sai kiểu dữ liệu", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }
            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "", Data = slide });
        }

        /// <summary>
        /// ChangeSlideStatus
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult ChangeOrder(int id, string type)
        {
            List<SlideDTO> slide;
            try
            {
                // Check authen.
                if (User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
                }

                if (!type.Equals("up", StringComparison.OrdinalIgnoreCase) &&
                    !type.Equals("down", StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidDataException();
                }
                // Check role user.
                var currentUser = UserRepository.Instance.GetBasicInfo(User.Identity.Name);
                if (currentUser == null || currentUser.Role != DDLConstants.UserType.ADMIN)
                {
                    throw new NotPermissionException();
                }
                slide = SlideRepository.Instance.ChangeOrder(id, type);
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
            catch (InvalidDataException)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "Sai kiểu dữ liệu", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }
            catch (Exception)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.BAD_REQUEST });
            }
            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "", Data = slide });
        }
    }
}