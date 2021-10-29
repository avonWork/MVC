using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace 縱火0709.Models
{
    public class Contact
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //prop
        public int Id { get; set; }

        /// <summary>
        /// 聯絡人姓名
        /// </summary>
        [Display(Name = "姓　名：")]
        [Required(ErrorMessage = " 姓名必填")]
        [StringLength(50, ErrorMessage = "請輸入姓名,至少3字元", MinimumLength = 3)]
        public string UserName { get; set; }

        /// <summary>
        /// 聯絡人性別
        /// </summary>
        [Display(Name = "性　別：")]
        [Required(ErrorMessage = " 性別必填")]
        public Enum.GenderType Sex { get; set; }

        /// <summary>
        /// 聯絡人信箱
        /// </summary>
        [Display(Name = "E-mail：")]
        [Required(ErrorMessage = "Email必填")]
        //[RegularExpression(@"^\w+@\w+(\.\w+){1,2}$", ErrorMessage = "Email格式不正确")]
        [EmailAddress(ErrorMessage = "Email格式不正确")]
        public string Email { get; set; }

        /// <summary>
        /// 聯絡人手機
        /// </summary>
        [Display(Name = "聯絡電話：")]
        [Required(ErrorMessage = "聯絡電話必填")]
        [RegularExpression(@"^09\d{8}$", ErrorMessage = "手機格式不正確")]
        public string Phone { get; set; }

        /// <summary>
        /// 內容
        /// </summary>
        [Display(Name = "詢問內容：")]
        [Required(ErrorMessage = " 內容必填")]
        public string Content { get; set; }

        /// <summary>
        /// 新增時間
        /// </summary>
        public DateTime? InsertTime { get; set; }
    }
}