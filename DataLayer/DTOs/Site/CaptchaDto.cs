using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DTOs.Site
{
    public class CaptchaDto
    {
        [Required]
        public string Captcah { get; set; }
    }
}
