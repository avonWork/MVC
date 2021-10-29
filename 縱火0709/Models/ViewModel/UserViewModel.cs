using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static 縱火0709.Models.Enum;

namespace 縱火0709.Models
{
    public class UserViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //prop
        public int Id { get; set; }

        [Display(Name = "用戶類型")]
        public int TypeId { get; set; }

        [ForeignKey("TypeId")]
        public virtual UserType UserType { get; set; }

        [Required]
        [Display(Name = "使用者姓名")]
        [StringLength(50, ErrorMessage = "請輸入姓名,至少3字元", MinimumLength = 3)]
        public string UserName { get; set; }

        [Display(Name = "使用者圖片")]
        public string UserImg { get; set; }

        [Display(Name = "帳號")]
        [Required(ErrorMessage = "帳號必填")]
        [Remote("CheckAccount", "Users", HttpMethod = "POST")]
        public string Account { get; set; }

        [Display(Name = "使用者密碼")]
        [StringLength(50, ErrorMessage = "請輸入密碼,至少3字元", MinimumLength = 3)]
        public string UserPassword { get; set; }

        [Display(Name = "密碼鹽")]
        [StringLength(100)]
        public string PasswordSalt { get; set; }

        [Required]
        [Display(Name = "權限")]
        public string Authority { get; set; }

        [Display(Name = "登入錯誤次數")]
        public int Error { get; set; }

        /// <summary>
        /// 註記是否顯示/刪除
        /// </summary>
        public StickyType Sticky { get; set; }

        [Required(ErrorMessage = "請輸入電話號碼(09)")]
        [Display(Name = "使用者電話號碼")]
        [RegularExpression(@"^09\d{8}$", ErrorMessage = "手機格式錯誤")]
        public string UserPhone { get; set; }

        [Required(ErrorMessage = "請輸入email")]
        [Display(Name = "使用者Email")]
        [EmailAddress(ErrorMessage = "非email格式")]
        public string UserEmail { get; set; }

        [Display(Name = "性別")]
        public GenderType Sex { get; set; }

        [Display(Name = "備註")]
        public string Ramark { get; set; }

        /// <summary>
        /// 新增者
        /// </summary>
        public int? InsertAdminID { get; set; }

        /// <summary>
        /// 新增時間
        /// </summary>
        public DateTime? InsertTime { get; set; }

        /// <summary>
        /// 編輯者
        /// </summary>
        public int? EditAdminID { get; set; }

        /// <summary>
        /// 編輯時間
        /// </summary>
        public DateTime? EditTime { get; set; }

        [Display(Name = "興趣")]
        public List<CheckBoxItem> HobbyItems { get; set; }
    }
}