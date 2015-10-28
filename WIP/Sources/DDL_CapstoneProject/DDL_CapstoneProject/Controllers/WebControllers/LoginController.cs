using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DDL_CapstoneProject.Models;
using DDL_CapstoneProject.Models.DTOs;
using DDL_CapstoneProject.Respository;
using DDL_CapstoneProject.Ultilities;
using Facebook;

namespace DDL_CapstoneProject.Controllers.WebControllers
{
    public class LoginController : Controller
    {
        // GET: Login
        [Route("Login")]
        public ActionResult Login(string returnUrl = "")
        {
            try
            {
                FormsAuthentication.SignOut();
                // Remove all cookies.
                var limit = Request.Cookies.Count;
                for (int i = 0; i < limit; i++)
                {
                    var cookieName = Request.Cookies[i].Name;
                    var cookie = new HttpCookie(cookieName) { Expires = DateTime.UtcNow.AddDays(-1) };
                    Response.Cookies.Add(cookie);
                }
                ViewBag.ReturnUrl = returnUrl;
                return View("Login", new UserLoginDTO());
            }
            catch (Exception)
            {
                return Redirect("/#/error");
            }
        }

        [Route("Login")]
        [HttpPost]
        public ActionResult Login(UserLoginDTO model, string returnUrl)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return View("~/Views/Login/Login.cshtml", model);
                }

                var user =
                    UserRepository.Instance.GetByUserNameOrEmail(model.Username, model.Password);
                if (user == null)
                {
                    ModelState.AddModelError("", "Sai mật khẩu hoặc tài khoản không tồn tài!");

                }
                else if (user.LoginType == DDLConstants.LoginType.FACEBOOK)
                {
                    ModelState.AddModelError("", "Sai mật khẩu hoặc tài khoản không tồn tài!");

                }
                else if (!user.IsActive || !user.IsVerify)
                {
                    ModelState.AddModelError("", "Tài khoản bị khóa hoặc chưa xác nhận Email!");
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(user.Username, model.RememberMe);
                    user.LastLogin = DateTime.UtcNow;
                    UserRepository.Instance.UpdateUser(user);
                    if (string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect("/");
                    }
                    else
                    {
                        return Redirect("/#/" + returnUrl);
                    }
                }

                ViewBag.ReturlUrl = returnUrl;
                return View("~/Views/Login/Login.cshtml", model);
            }
            catch (Exception)
            {
                return Redirect("/#/error");
            }
        }

        [Route("Logout")]
        [Authorize]
        public ActionResult Logout()
        {
            try
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
            catch (Exception)
            {
                return Redirect("/#/error");
            }
        }

        public ActionResult AuthenFacebook()
        {
            try
            {
                var fb = new FacebookClient();
                var loginUrl = fb.GetLoginUrl(new
                {
                    client_id = ConfigurationManager.AppSettings["clientId"],
                    //"412367302292593",
                    client_secret = ConfigurationManager.AppSettings["clientSecret"],
                    //"95b9b97174f94bbff3bdff437e520cc7",
                    redirect_uri = RedirectUri.AbsoluteUri,
                    response_type = "code",

                    scope = "email,user_birthday,user_about_me,user_website" // Add other permissions as needed
                });


                return Redirect(loginUrl.AbsoluteUri);
            }
            catch (Exception)
            {
                return Redirect("/#/error");
            }
            
        }

        public ActionResult FacebookCallback(string code)
        {
            try
            {
                var fb = new FacebookClient();
                dynamic result = fb.Post("oauth/access_token", new
                {
                    client_id = ConfigurationManager.AppSettings["clientId"],
                    client_secret = ConfigurationManager.AppSettings["clientSecret"],
                    redirect_uri = RedirectUri.AbsoluteUri,
                    code = code
                });

                var accessToken = result.access_token;

                // Store the access token in the session
                Session["AccessToken"] = accessToken;

                // update the facebook client with the access token so 
                // we can make requests on behalf of the user
                fb.AccessToken = accessToken;


                // Get the user's information
                dynamic me = fb.Get("/me?fields=id,name,gender,link,birthday,email,bio");
                string facebookId = me.id;
                string email = me.email;

                if (string.IsNullOrEmpty(email))
                {
                    email = facebookId + "@facebook.com";
                }
                me.email = email;

                // select from DB
                var newUser = UserRepository.Instance.GetByUserNameOrEmail(email);

                /*
                 *  Insert into DB
                 */

                if (newUser == null)
                {
                    newUser = UserRepository.Instance.RegisterFacebook(me);
                }
                else if (newUser.LoginType == DDLConstants.LoginType.NORMAL)
                {
                    newUser.LoginType = DDLConstants.LoginType.BOTH;
                    newUser.IsVerify = true;
                    newUser.LastLogin = DateTime.UtcNow;
                    newUser.UserInfo.FacebookUrl = me.link;
                    newUser.UserInfo.ProfileImage = "https://graph.facebook.com/" + facebookId + "/picture?type=large";
                    newUser = UserRepository.Instance.UpdateUser(newUser);
                }
                else if (newUser.IsActive == false)
                {
                    // user is Locked
                    TempData["loginMessageError"] = "Tài khoản của bạn đã bị khóa!";
                    return RedirectToAction("Login", "Login");
                }

                // Set the auth cookie

                FormsAuthentication.SetAuthCookie(newUser.Username, false);
                newUser.LastLogin = DateTime.UtcNow;
                UserRepository.Instance.UpdateUser(newUser);
                //SessionHelper.RenewCurrentUser();

                return Redirect("/");
            }
            catch (Exception)
            {
                return Redirect("/#/error");
            }
            
        }

        [Route("Active")]
        public ActionResult Active(string user_name, string code)
        {
            try
            {
                if (string.IsNullOrEmpty(user_name) || string.IsNullOrEmpty(code))
                {
                    ViewBag.Status = "error";
                    ViewBag.Message = "Có lỗi xảy ra!";
                    return View();
                }

                var result = UserRepository.Instance.VerifyAccount(user_name, code);
                if (result)
                {
                    ViewBag.Status = "success";
                    ViewBag.Message = "Tài khoản của bạn đã kích hoạt thành công!";
                    return View();
                }

                ViewBag.Status = "error";
                ViewBag.Message = "Có lỗi xảy ra!";
                return View();
            }
            catch (Exception)
            {
                return Redirect("/#/error");
            }
            
        }

        /// <summary>
        /// ForgotPassword action.
        /// </summary>
        /// <returns>Forgot password view</returns>
        [Route("ForgotPassword")]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        // GET: Login
        [Route("Admin/Login")]
        public ActionResult AdminLogin(string returnUrl = "")
        {
            try
            {
                FormsAuthentication.SignOut();
                // Remove all cookies.
                var limit = Request.Cookies.Count;
                for (int i = 0; i < limit; i++)
                {
                    var cookieName = Request.Cookies[i].Name;
                    var cookie = new HttpCookie(cookieName) { Expires = DateTime.Now.AddDays(-1) };
                    Response.Cookies.Add(cookie);
                }
                ViewBag.ReturnUrl = returnUrl;
                return View("AdminLogin", new UserLoginDTO());
            }
            catch (Exception)
            {
                return Redirect("/#/error");
            }
            
        }


        [Route("Admin/Login")]
        [HttpPost]
        public ActionResult AdminLogin(UserLoginDTO model, string returnUrl)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return View("~/Views/Login/AdminLogin.cshtml", model);
                }

                var user =
                    UserRepository.Instance.GetByUserNameOrEmail(model.Username, model.Password);
                if (user == null)
                {
                    ModelState.AddModelError("", "Sai mật khẩu hoặc tài khoản không tồn tài!");

                }
                else if (user.LoginType == DDLConstants.LoginType.FACEBOOK || user.UserType == DDLConstants.UserType.USER)
                {
                    ModelState.AddModelError("", "Bạn không có quyền truy cập!");

                }
                else if (!user.IsActive || !user.IsVerify)
                {
                    ModelState.AddModelError("", "Tài khoản bị khóa hoặc chưa xác nhận Email!");
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(user.Username, model.RememberMe);
                    user.LastLogin = DateTime.UtcNow;
                    UserRepository.Instance.UpdateUser(user);
                    if (string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect("/admin");
                    }
                    else
                    {
                        return Redirect("/Admin/#/" + returnUrl);
                    }
                }

                ViewBag.ReturlUrl = returnUrl;
                return View("~/Views/Login/AdminLogin.cshtml", model);
            }
            catch (Exception)
            {
                return Redirect("/#/error");
            }
        }

        [Route("Admin/Logout")]
        [Authorize]
        public ActionResult AdminLogout()
        {
            try
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
                return Redirect("/admin/login");
            }
            catch (Exception)
            {
                return Redirect("/#/error");
            }       
        }

        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url)
                {
                    Query = null,
                    Fragment = null,
                    Path = Url.Action("FacebookCallback")
                };
                return uriBuilder.Uri;
            }
        }
    }
}