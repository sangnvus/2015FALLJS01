﻿using System;
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
            var listCategoryDTO = CategoryRepository.Instance.GetCategories();

            return Ok(new HttpMessageDTO { Status = "success", Data = listCategoryDTO });
        }
        public IHttpActionResult GetCategoryProjectCount()
        {
            var listGetCategoryProjectCount = CategoryRepository.Instance.GetCategoryProjectCount();
            return Ok(new HttpMessageDTO { Status = "success", Data = listGetCategoryProjectCount });
        }
    }
}