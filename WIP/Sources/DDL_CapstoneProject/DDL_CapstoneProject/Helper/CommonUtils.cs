using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace DDL_CapstoneProject.Ultilities
{
    public static class CommonUtils
    {
        // This class includes common functions.
        public static string GenerateVerifyCode()
        {
            var g = Guid.NewGuid();
            var guidString = Convert.ToBase64String(g.ToByteArray());
            guidString = new string(guidString.Where(char.IsLetterOrDigit).ToArray());
            return guidString;
        }

        public static byte[] EncryptData(string data)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5Hasher = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] hashedBytes;
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes(data));
            return hashedBytes;
        }
        public static string Md5(string data)
        {
            return BitConverter.ToString(EncryptData(data)).Replace("-", "").ToLower();
        }

        public static string UploadImage(HttpPostedFile file, string fileName, string uploadPath)
        {
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);
                fileName = fileName + extension;
                var path = Path.Combine(HttpContext.Current.Request.MapPath(uploadPath), fileName);

                file.SaveAs(path);

                return fileName;
            }

            return string.Empty;

        }


        public static DateTime ConvertDateTimeFromUtc(DateTime datetime)
        {
            var cetZone = TimeZoneInfo.FindSystemTimeZoneById(DDLConstants.TimeZoneId);
            var cetTime = TimeZoneInfo.ConvertTimeFromUtc(datetime, cetZone);
            return cetTime;
        }
        public static DateTime ConvertDateTimeToUtc(DateTime datetime)
        {
            var cetZone = TimeZoneInfo.FindSystemTimeZoneById(DDLConstants.TimeZoneId);
            var cetTime = TimeZoneInfo.ConvertTimeToUtc(datetime, cetZone);
            return cetTime;
        }

        public static DateTime DateTimeNowGMT7()
        {
            var cetZone = TimeZoneInfo.FindSystemTimeZoneById(DDLConstants.TimeZoneId);
            var cetTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, cetZone);
            return cetTime;
        }

        public static DateTime DateTodayGMT7()
        {
            var cetZone = TimeZoneInfo.FindSystemTimeZoneById(DDLConstants.TimeZoneId);
            var cetTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow.Date, cetZone);
            return cetTime;
        }
    }
}