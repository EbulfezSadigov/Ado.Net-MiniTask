using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Exercise.Models
{
    public class Element:BaseEntity
    {
        public string Image { get; set; }
        [NotMapped]
        public IFormFile Img { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
    }
}
