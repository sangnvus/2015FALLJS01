using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using DDL_CapstoneProject.Helper;
using DDL_CapstoneProject.Models.DTOs;
using DDL_CapstoneProject.Respository;

namespace DDL_CapstoneProject.Controllers.ApiControllers
{
    public class CategoryApiController : BaseApiController
    {
        // GET: api/CategoryApi/GetCategories
        [HttpGet]
        [ResponseType(typeof(CategoryDTO))]
        public IHttpActionResult GetCategories()
        {
            var listCategories = CategoryRepository.Instance.GetCategories();
            return Ok(new HttpMessageDTO { Status = "success", Data = listCategories });
        }
    }
}