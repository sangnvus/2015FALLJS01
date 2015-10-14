using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using DDL_CapstoneProject.Models;
using DDL_CapstoneProject.Respository;
using DDL_CapstoneProject.Models.DTOs;

namespace DDL_CapstoneProject.Controllers.ApiControllers
{
    public class SlideApiController : ApiController
    {
        // GET: api/Slides
        public IHttpActionResult GetSlides()
        {
            var listSlides = SlideRepository.Instance.GetSlides();
            return Ok(new HttpMessageDTO { Status = "success", Data = listSlides });
        }
    }
}