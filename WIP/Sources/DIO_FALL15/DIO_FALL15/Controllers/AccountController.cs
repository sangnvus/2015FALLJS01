using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DIO_FALL15.Models.DTOs;
using System.Web.Security;
using DIO_FALL15.Respository;

namespace DIO_FALL15.Controllers
{
    public class AccountController : Controller
    {
        private readonly DatabaseContext _dbContext = new DatabaseContext();
        // GET: Account
        public ActionResult Login(string returnUrl = "")
        {
            FormsAuthentication.SignOut();
            ViewBag.ReturlUrl = returnUrl;
            return View(new UserLoginDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLoginDTO model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user =
                _dbContext.Users.FirstOrDefault(x => (x.Username == model.Username && x.Password == model.Password));
            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(user.Username, model.RememberMe);
                if (string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect("/");
                }
                else
                {
                    return Redirect(returnUrl);
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid Username or Password");

            }
            ViewBag.ReturlUrl = returnUrl;
            return View(model);
        }
    }
}