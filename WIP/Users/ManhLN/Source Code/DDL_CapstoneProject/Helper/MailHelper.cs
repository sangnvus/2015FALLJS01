using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using DDL_CapstoneProject.Helpers;
using DDL_CapstoneProject.Ultilities;

namespace DDL_CapstoneProject.Helper
{
    public class MailHelper: SingletonBase<MailHelper>
    {
        private const string SenderId = "ngocmanh1712@gmail.com";
        private const string SenderPassword = "145625827";
        private readonly SmtpClient _smtp;
        private readonly MailMessage _mail;

        private MailHelper()
        {
            _mail = new MailMessage();
            _smtp = new SmtpClient
            {
                Host = "smtp.gmail.com", // smtp server address here…
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(SenderId, SenderPassword),
                Timeout = 30000,
            };
            _mail.From = new MailAddress(SenderId);
            _mail.IsBodyHtml = true;
        }

        public void SendMailActive(string email, string userName, string verifyCode, string fullName)
        {
            try
            {
                _mail.To.Add(email);
                _mail.Subject = "Dandelion - Kích hoạt tài khoản!";
                _mail.Body = "Xin chào " + fullName + "," +
                            "<br/>Bạn đã đăng ký thành công tài khoản tại hệ thống của Dandelion. " +
                            "<br/>Để kích hoạt tài khoản, vui lòng bấm vào link dưới đây:" +
                            "<br/><a href='http://localhost:34747/active?user_name=" + userName + "&code=" + verifyCode + "'>http://localhost:34747/active?use_name=" + userName + "&code=" + verifyCode + "</a>";
                _smtp.Send(_mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

        public void SendMailResetPassword(string email, string newPassword,string fullName)
        {
            try
            {
                _mail.To.Add(email);
                _mail.Subject = "Dandelion - Mật khẩu mới!";
                _mail.Body = "Xin chào " + fullName + "," +
                            "<br/>Mật khẩu mới của bạn là: " + newPassword +
                            "<br/>Bạn có thể bấm vào link dưới đây để đăng nhập với mật khẩu mới" +
                            "<br/><a href='http://localhost:34747/login'>http://localhost:34747/login</a>";
                _smtp.Send(_mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }
    }
}