﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            // Remove all cookies.
            var limit = Request.Cookies.Count;
            for (int i = 0; i < limit; i++)
            {
                var cookieName = Request.Cookies[i].Name;
                var cookie = new HttpCookie(cookieName) {Expires = DateTime.Now.AddDays(-1)};
                Response.Cookies.Add(cookie);
            }
            ViewBag.ReturlUrl = returnUrl;
            return View("Login",new UserLoginDTO());
        }

        public ActionResult Register()
        {
            return PartialView("_Register");
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
                var cookie = new HttpCookie("CurrentUser", user.Username) {Expires = DateTime.MinValue};
                Response.Cookies.Add(cookie);
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
                ModelState.AddModelError("", "Invalid Username or Password!");

            }
            ViewBag.ReturlUrl = returnUrl;
            return View(model);
        }
        // Login Api
        //[HttpPost]
        //public ActionResult Login(UserLoginDTO model, string returnUrl)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Something Error");
        //    }

        //    var user =
        //        _dbContext.Users.FirstOrDefault(x => (x.Username == model.Username && x.Password == model.Password));
        //    if (user != null)
        //    {
        //        FormsAuthentication.SetAuthCookie(user.Username, model.RememberMe);
        //        Session["CurrentUser"] = user.Username;
        //        if (string.IsNullOrEmpty(returnUrl))
        //        {
        //            return new HttpStatusCodeResult(HttpStatusCode.OK, "/");
        //        }
        //        else
        //        {
        //            return new HttpStatusCodeResult(HttpStatusCode.OK, returnUrl);
        //        }
        //    }
        //    else
        //    {
        //        //return   (HttpStatusCode.BadRequest, "Invalid Username or Password");
        //        Response.StatusCode = (int) HttpStatusCode.BadRequest;
        //        return Json("Invalid Username or Password");
        //    }
        //}

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            //for (int index = 0; index < Session.Keys.Count; index++)
            //{
            //    var sessionName = Session.Keys[index];
            //    Session[sessionName] = null;
            //}
            // Remove all cookies.
            var limit = Request.Cookies.Count;
            for (int i = 0; i < limit; i++)
            {
                var cookieName = Request.Cookies[i].Name;
                var cookie = new HttpCookie(cookieName) { Expires = DateTime.Now.AddDays(-1) };
                Response.Cookies.Add(cookie);
            }
            return Redirect("/");
        }

        [Authorize]
        public ActionResult EditProfile()
        {
            return PartialView("_EditProfile");
        }
    }
}