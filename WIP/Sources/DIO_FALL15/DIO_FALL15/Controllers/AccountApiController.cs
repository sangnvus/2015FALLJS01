using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using DIO_FALL15.Models;
using DIO_FALL15.Models.DTOs;
using DIO_FALL15.Respository;

namespace DIO_FALL15.Controllers
{
    public class AccountApiController : ApiController
    {
        public DatabaseContext Db = new DatabaseContext();
        // POST: api/AccountsApi
        [ResponseType(typeof(UserRegisterDTO))]
        public IHttpActionResult CreateAccount(UserRegisterDTO Account)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else if (Db.Users.Any(x => x.Username.Equals(Account.Username, StringComparison.OrdinalIgnoreCase)))
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Username has been used!"));
            }
            else if (Db.Users.Any(x => x.Email.Equals(Account.Email, StringComparison.OrdinalIgnoreCase)))
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Email has been used!"));
            }

            var newUser = new User
            {
                Username = Account.Username,
                Password = Account.Password,
                FirstName = Account.FirstName,
                LastName = Account.LastName,
                Gender = Account.Gender,
                Email = Account.Email,
                PhoneNumber = Account.PhoneNumber,
                Address = Account.Address
            };

            Db.Users.Add(newUser);
            Db.SaveChanges();

            return Ok("Success");
        }
    }
}
