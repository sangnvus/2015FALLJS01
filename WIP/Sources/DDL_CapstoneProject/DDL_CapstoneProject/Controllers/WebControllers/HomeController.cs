using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DDL_CapstoneProject.Helper;
using DDL_CapstoneProject.Models.DTOs;
using DDL_CapstoneProject.Respository;
using DDL_CapstoneProject.Ultilities;

namespace DDL_CapstoneProject.Controllers.WebControllers
{
    public class HomeController : BaseController
    {
        [Route("")]
        public ActionResult Index()
        {
            getCurrentUser();
            ViewBag.BaseUrl = GetBaseUrl();
            return View();
        }

        [Route("Admin")]
        public ActionResult AdminIndex()
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Redirect("/admin/login");
            }
            var currentUser = getCurrentUser();
            if (currentUser.UserType != DDLConstants.UserType.ADMIN)
            {
                return Redirect("/admin/login");
            }
            ViewBag.BaseUrl = GetBaseUrl() + "admin/";
            return View();
        }

        [Route("Error")]
        public ActionResult Error()
        {

            return View("~/Views/Shared/Error.cshtml");
        }

        [Route("baokim")]
        public ActionResult Baokim()
        {
            try
            {
                var bk = new BaoKimPayment();
                var randomCode = CommonUtils.GenerateVerifyCode();
                var paymentUrl = bk.createRequestUrl(
                    Request.QueryString["ProjectCode"] + randomCode + DateTime.Now.ToString("hmmsstt"),
                    "dev.baokim@bk.vn",
                     Request.QueryString["PledgeAmount"],
                    "0",
                    "0",
                    Request.QueryString["Des"],
                    "http://dandelionvn.com/baokimcallback",
                    "http://dandelionvn.com/#/project/detail/" + Request.QueryString["ProjectCode"],
                    "http://dandelionvn.com/#/project/detail/" + Request.QueryString["ProjectCode"]
                    );

                // Inital cookie
                HttpCookie projectCodeCookie = new HttpCookie("ProjectCode");
                HttpCookie emailCookie = new HttpCookie("Email");
                HttpCookie backerNameCookie = new HttpCookie("BackerName");
                HttpCookie rewardIdCookie = new HttpCookie("RewardId");
                HttpCookie pledgeAmountCookie = new HttpCookie("PledgeAmount");
                HttpCookie quantityCookie = new HttpCookie("Quantity");
                HttpCookie descCookie = new HttpCookie("description");
                HttpCookie addressCookie = new HttpCookie("Address");
                HttpCookie phoneNumberCookie = new HttpCookie("Phonenumber");

                DateTime now = DateTime.Now;

                // Set the cookie value.
                projectCodeCookie.Value = Request.QueryString["ProjectCode"];
                emailCookie.Value = Request.QueryString["Email"];
                backerNameCookie.Value = Request.QueryString["BackerName"];
                rewardIdCookie.Value = Request.QueryString["RewardId"];
                pledgeAmountCookie.Value = Request.QueryString["PledgeAmount"];
                quantityCookie.Value = Request.QueryString["Quantity"];
                descCookie.Value = Request.QueryString["Mes"];
                addressCookie.Value = Request.QueryString["Address"];
                phoneNumberCookie.Value = Request.QueryString["Phone"];

                // Set the cookie expiration date.
                projectCodeCookie.Expires = now.AddMinutes(30);
                emailCookie.Expires = now.AddMinutes(30);
                backerNameCookie.Expires = now.AddMinutes(30);
                rewardIdCookie.Expires = now.AddMinutes(30);
                pledgeAmountCookie.Expires = now.AddMinutes(30);
                quantityCookie.Expires = now.AddMinutes(30);
                descCookie.Expires = now.AddMinutes(30);
                addressCookie.Expires = now.AddMinutes(30);
                phoneNumberCookie.Expires = now.AddMinutes(30);


                // Add the cookie.
                Response.Cookies.Add(projectCodeCookie);
                Response.Cookies.Add(emailCookie);
                Response.Cookies.Add(backerNameCookie);
                Response.Cookies.Add(rewardIdCookie);
                Response.Cookies.Add(pledgeAmountCookie);
                Response.Cookies.Add(quantityCookie);
                Response.Cookies.Add(descCookie);
                Response.Cookies.Add(addressCookie);
                Response.Cookies.Add(phoneNumberCookie);

                return Redirect(paymentUrl);
            }
            catch (Exception)
            {
                return Redirect("/#/error");
            }
        }

        [Route("baokimcallback")]
        public ActionResult BaokimCallBack()
        {
            try
            {
                HttpCookie projectCodeCookie = Request.Cookies["ProjectCode"];
                HttpCookie emailCookie = Request.Cookies["Email"];
                HttpCookie backerNameCookie = Request.Cookies["BackerName"];
                HttpCookie rewardIdCookie = Request.Cookies["RewardId"];
                HttpCookie pledgeAmountCookie = Request.Cookies["PledgeAmount"];
                HttpCookie quantityCookie = Request.Cookies["Quantity"];
                HttpCookie descCookie = Request.Cookies["description"];
                HttpCookie addressCookie = Request.Cookies["Address"];
                HttpCookie phoneNumberCookie = Request.Cookies["Phonenumber"];

                // Convert unixtime to datetime
                string createdSecond = Request.QueryString["created_on"];
                double unixTimeStamp = Double.Parse(createdSecond);
                System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                DateTime createdTime = (new DateTime(1970, 1, 1, 0, 0, 0, 0)).AddSeconds(unixTimeStamp).ToLocalTime();

                var projectBackDTO = new ProjectBackDTO
                {
                    ProjectCode = projectCodeCookie.Value,
                    Email = emailCookie.Value,
                    BackerName = backerNameCookie.Value,
                    RewardPkgID = Int32.Parse(rewardIdCookie.Value),
                    PledgeAmount = Decimal.Parse(pledgeAmountCookie.Value),
                    Quantity = Int32.Parse(quantityCookie.Value),
                    Description = descCookie.Value,
                    Address = addressCookie.Value,
                    PhoneNumber = phoneNumberCookie.Value,
                    BackedDate = createdTime,
                    OrderId = Request.QueryString["order_id"],
                    TransactionId = Request.QueryString["transaction_id"]
                };

                string orderId = ProjectRepository.Instance.BackProject(projectBackDTO);
                ProjectRepository.Instance.CaculateProjectPoint(projectBackDTO.ProjectCode, DDLConstants.PopularPointType.BackingPoint);

                // Remove all cookies.
                var limit = Request.Cookies.Count;
                for (int i = 0; i < limit; i++)
                {
                    var cookieName = Request.Cookies[i].Name;
                    if (cookieName == projectCodeCookie.Name || cookieName == emailCookie.Name || cookieName == backerNameCookie.Name || cookieName == rewardIdCookie.Name
                        || cookieName == pledgeAmountCookie.Name || cookieName == quantityCookie.Name || cookieName == descCookie.Name
                        || cookieName == addressCookie.Name || cookieName == phoneNumberCookie.Name)
                    {
                        var cookie = new HttpCookie(cookieName) { Expires = DateTime.UtcNow.AddDays(-1) };
                        Response.Cookies.Add(cookie);
                    }
                }

                return Redirect("/#/backsuccess/" + orderId);
            }
            catch (Exception)
            {
                return Redirect("/#/error");
            }
        }
    }
}