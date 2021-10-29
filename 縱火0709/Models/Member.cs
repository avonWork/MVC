using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using static 縱火0709.Models.Enum;

namespace 縱火0709.Models
{
    public class Member
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// 會員身分識別id
        /// </summary>
        [Key]
        [Display(Name = "身分識別id")]
        public string UserId { get; set; }

        public Member()
        {
            Experience = new List<Experience>();
        }

        /// <summary>
        /// 會員帳號
        /// </summary>
        [Display(Name = "帳　號：")]
        [Required(ErrorMessage = "帳號必填")]
        [StringLength(50, ErrorMessage = "帳號 長度至少必須為 6 個字元。", MinimumLength = 6)]
        [Remote("CheckAccount", "Members", HttpMethod = "POST")]
        public string Account { get; set; }

        /// <summary>
        /// 開通會員帳號
        /// </summary>
        [Display(Name = "會員開通")]
        public bool CheckAccount { get; set; }

        public int TypeId { get; set; }

        /// <summary>
        /// 會員申請類別
        /// </summary>
        [Display(Name = "申請類別：")]
        [ForeignKey("TypeId")]
        public virtual UserType UserType { get; set; }

        /// <summary>
        /// 會員姓名
        /// </summary>
        [Display(Name = "姓　名：")]
        [Required(ErrorMessage = " 姓名必填")]
        [StringLength(50, ErrorMessage = "請輸入姓名,至少3字元", MinimumLength = 3)]
        public string UserName { get; set; }

        /// <summary>
        /// 會員密碼
        /// </summary>
        [Display(Name = "密　碼：")]
        [Required(ErrorMessage = "密碼必填")]
        [StringLength(50, ErrorMessage = "密碼 長度至少必須為 6 個字元。", MinimumLength = 6)]
        public string Password1 { get; set; }

        [Display(Name = "確認密碼：")]
        [Required(ErrorMessage = "密碼必填")]
        [StringLength(50, ErrorMessage = "密碼 長度至少必須為 6 個字元。", MinimumLength = 6)]
        [System.ComponentModel.DataAnnotations.Compare("Password1", ErrorMessage = "密碼與確認密碼不相符。")]
        public string Password2 { get; set; }

        [Display(Name = "密碼鹽")]
        [StringLength(100)]
        public string PasswordSalt { get; set; }

        /// <summary>
        /// 會員生日
        /// </summary>
        [Display(Name = "生　日：")]
        [DataType(DataType.Date, ErrorMessage = "非正確的生日日期")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [Required(ErrorMessage = "生日必填")]
        public DateTime Birthday { get; set; }

        [Display(Name = "權限")]
        public string Authority { get; set; }

        [Display(Name = "登入錯誤次數")]
        public int Error { get; set; }

        /// <summary>
        /// 會員Email
        /// </summary>
        [Display(Name = "Email：")]
        [Required(ErrorMessage = "E-mail必填")]
        [EmailAddress(ErrorMessage = "非正確email格式")]
        public string Email { get; set; }

        /// <summary>
        /// 會員性別
        /// </summary>
        [Display(Name = "性　別：")]
        [Required(ErrorMessage = " 性別必填")]
        public GenderType Sex { get; set; }

        /// <summary>
        /// 註記是否顯示/刪除
        /// </summary>
        public StickyType Sticky { get; set; }

        /// <summary>
        /// 會員住址
        /// </summary>
        [Display(Name = "通訊處：")]
        [Required(ErrorMessage = " 通訊處必填")]
        public string Address { get; set; }

        /// <summary>
        /// 是否有國際會籍
        /// </summary>
        [Display(Name = "國際會籍：")]
        public bool InternationalMembership { get; set; }

        /// <summary>
        /// 會員現職單位
        /// </summary>
        [Display(Name = "現職單位：")]
        [Required(ErrorMessage = " {0}必填")]
        public string CurrentEmployer { get; set; }

        /// <summary>
        /// 會員職稱
        /// </summary>
        [Display(Name = "職　稱：")]
        [Required(ErrorMessage = " 職稱必填")]
        public string JobTitle { get; set; }

        /// <summary>
        /// 會員最高學歷
        /// </summary>
        [Display(Name = "最高學歷：")]
        [Required(ErrorMessage = " {0}必填")]
        public string HighestEducation { get; set; }

        /// <summary>
        /// 會員合計年資(年)
        /// </summary>
        [Display(Name = "相關年資合計：")]
        [Required(ErrorMessage = " 年資(年)必填")]
        [Range(0, 99, ErrorMessage = "{0}只能在{1}至{2}之间")]
        public string TotalRelevantYears { get; set; }

        /// <summary>
        /// 會員合計年資(月)
        /// </summary>
        [Required(ErrorMessage = " 年資(月)必填")]
        [Range(0, 11, ErrorMessage = "{0}只能在{1}至{2}之间")]
        public string TotalRelevantMonths { get; set; }

        [JsonIgnore]
        public virtual IList<Experience> Experience { get; set; }

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
    }
}