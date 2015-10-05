using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    }
}