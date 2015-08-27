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

        // POST: api/AccountsApi/CreateAccount
        [ResponseType(typeof (UserRegisterDTO))]
        [HttpPost]
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

        // put: api/AccountsApi/EditAccount/:id
        [Authorize]
        [ResponseType(typeof(UserDetailDTO))]
        [HttpPut]
        public IHttpActionResult EditAccount(int id,UserDetailDTO Account)
        {
            if (Account == null)
            {
                return BadRequest(ModelState);
            }
            var currentUser =
                Db.Users.SingleOrDefault(u => u.Username.Equals(User.Identity.Name, StringComparison.OrdinalIgnoreCase));

            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "User are not login!"));
            } 
            else if (currentUser.Id != id || currentUser.Id != Account.Id)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Not permision to modify"));
            }

            var updateUser =
                Db.Users.SingleOrDefault(u => u.Id == id);

            if (updateUser == null)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "User not found!"));
            }

            updateUser.FirstName = Account.FirstName;
            updateUser.LastName = Account.LastName;
            updateUser.Gender = Account.Gender;
            updateUser.PhoneNumber = Account.PhoneNumber;
            updateUser.Address = Account.Address;

            Db.SaveChanges();

            return Ok("Your Profile has been updated!");
        }

        [Authorize]
        [ResponseType(typeof(UserDetailDTO))]
        [HttpGet]
        public IHttpActionResult GetCurrentAccount()
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "User are not login!"));
            }

            var currentUser =
                Db.Users.SingleOrDefault(u => u.Username.Equals(User.Identity.Name, StringComparison.OrdinalIgnoreCase));

            if (currentUser == null)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Some thing fail!"));
            }

            var userDTO = new UserDetailDTO
            {
                Id = currentUser.Id,
                Username = currentUser.Username,
                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName,
                Gender = currentUser.Gender,
                Email = currentUser.Email,
                PhoneNumber = currentUser.PhoneNumber,
                Address = currentUser.Address
            };

            return Ok(userDTO);
        }
    }
}
