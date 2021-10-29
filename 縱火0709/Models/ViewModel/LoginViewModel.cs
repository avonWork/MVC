using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace 縱火0709.Models
{
    public class LoginViewModel
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "帳號")]
        [Required(ErrorMessage = "{0}必填")]
        public string Account { get; set; }

        [Display(Name = "密碼")]
        [Required(ErrorMessage = "{0}必填")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "{0}必填")]
        [EmailAddress(ErrorMessage = "非正確email格式")]
        public string Email { get; set; }
    }
}