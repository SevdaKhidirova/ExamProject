using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace examPro.Models
{
    public class Service : BaseEntity
    {
        public string Image { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }

        [NotMapped]
        public IFormFile Img { get; set; }
    }
}
