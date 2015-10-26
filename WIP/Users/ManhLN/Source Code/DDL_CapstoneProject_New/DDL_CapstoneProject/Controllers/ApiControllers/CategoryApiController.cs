using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using DDL_CapstoneProject.Helper;
using DDL_CapstoneProject.Models.DTOs;
using DDL_CapstoneProject.Respository;
using DDL_CapstoneProject.Ultilities;

namespace DDL_CapstoneProject.Controllers.ApiControllers
{
    public class CategoryApiController : BaseApiController
    {
        

        // GET: api/CategoryApi/GetCategories
        [HttpGet]
        [ResponseType(typeof(CategoryDTO))]
        public IHttpActionResult listDataForStatistic()
        {
            var listCategoryDTO = CategoryRepository.Instance.listDataForStatistic();

            return Ok(new HttpMessageDTO { Status = "success", Data = listCategoryDTO });
        }
        public IHttpActionResult getAllCategories()
        {
            var list = CategoryRepository.Instance.GetAllCategories();
            return Ok(new HttpMessageDTO { Status = "success", Data = list });
        }
        public IHttpActionResult GetCategoryProjectCount()
        {
            var listGetCategoryProjectCount = CategoryRepository.Instance.GetCategoryProjectCount();
            return Ok(new HttpMessageDTO { Status = "success", Data = listGetCategoryProjectCount });
        }

        /// <summary>
        /// GetCategoryProjectCount
        /// </summary>
        /// <returns></returns>
        // GET: api/CategoryApi/GetCategoriesForCreate
        [HttpGet]
        [ResponseType(typeof(CategoryDTO))]
        public IHttpActionResult GetCategoriesForCreate()
        {
            // Check authen.
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.ERROR, Message = "", Type = DDLConstants.HttpMessageType.NOT_AUTHEN });
            }

            var listCategoryDTO = CategoryRepository.Instance.GetAllCategories();

            return Ok(new HttpMessageDTO { Status = "success", Data = listCategoryDTO });
        }

        public IHttpActionResult GetCategoriesForAdmin()
        {
            List<AdminCategoryDTO> categoryList;
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
                categoryList = CategoryRepository.Instance.GetCategoriesForAdmin();
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
            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "", Data = categoryList });
        }

        /// <summary>
        /// ChangeCategoryStatus
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult ChangeCategoryStatus(int id)
        {
            AdminCategoryDTO category;
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
                category = CategoryRepository.Instance.ChangeCategoryStatus(id);
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
            return Ok(new HttpMessageDTO { Status = DDLConstants.HttpMessageType.SUCCESS, Message = "", Type = "", Data = category });
        }

        /// <summary>
        /// AddNewCategory
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddNewCategory(AdminCategoryDTO category)
        {
            AdminCategoryDTO result;
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

                result = CategoryRepository.Instance.AddNewCategory(category);
            }
            catch (DuplicateNameException)
            {
                return
                    Ok(new HttpMessageDTO
                    {
                        Status = DDLConstants.HttpMessageType.ERROR,
                        Message = "Tên danh mục đã được sử dụng!",
                        Type = DDLConstants.HttpMessageType.BAD_REQUEST
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
        /// DeleteCategory
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeleteCategory(int id)
        {
            bool result;
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

                result = CategoryRepository.Instance.DeleteCategory(id);
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
        /// EditCategory
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult EditCategory(AdminCategoryDTO category)
        {
            AdminCategoryDTO result;
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

                result = CategoryRepository.Instance.EditCategory(category);
            }
            catch (KeyNotFoundException)
            {
                return
                    Ok(new HttpMessageDTO
                    {
                        Status = DDLConstants.HttpMessageType.ERROR,
                        Message = "Không tồn tại!",
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
    }
}