using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace examPro.Extensions
{
    public static class Extension
    {
        public static bool isImage(this IFormFile file)
        {
            if (file.ContentType.Contains("image")){
                return true;
            }
            return false;
        }

        public static bool isSmallerThan(this IFormFile file,int bytes)
        {
            if (file.Length > bytes)
            {
                return false;
            }
            return true;
        }

    }
}
