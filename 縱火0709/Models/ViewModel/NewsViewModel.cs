using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using static 縱火0709.Models.Enum;

namespace 縱火0709.Models
{
    /// <summary>
    /// 新聞業務模型
    /// </summary>
    public class NewsViewModel
    {
        /// <summary>
        /// 新聞編號
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        /// <summary>
        /// 新聞日期
        /// </summary>
        [Required]
        [Display(Name = "新聞日期")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime Date { get; set; }

        /// <summary>
        /// 新聞標題
        /// </summary>
        [Required]
        [Display(Name = "新聞標題")]
        public string New_Title { get; set; }

        /// <summary>
        /// 新聞摘要
        /// </summary>
        [Display(Name = "新聞摘要")]
        public string New_Message { get; set; }

        /// <summary>
        /// 新聞圖片
        /// </summary>
        [Display(Name = "新聞圖片")]
        [EmailAddress(ErrorMessage = "請選擇新聞圖片")]
        public string New_img { get; set; }

        /// <summary>
        /// 新聞內容
        /// </summary>
        [Display(Name = "新聞內容")]
        public string New_Content { get; set; }

        /// <summary>
        /// 註記是否顯示
        /// </summary>
        [Display(Name = "顯示新聞")]
        public StickyType Sticky { get; set; }

        /// <summary>
        /// 新增者
        /// </summary>

        [Display(Name = "新增者")]
        public int AddUserId { get; set; }

        /// <summary>
        /// 新增時間
        /// </summary>
        public DateTime? AddTime { get; set; }

        /// <summary>
        /// 編輯者
        /// </summary>
        [Display(Name = "編輯者")]
        public int? EditUserId { get; set; }

        /// <summary>
        /// 編輯時間
        /// </summary>
        public DateTime? EditTime { get; set; }

        //[Required(ErrorMessage = "請選擇新聞圖片")]
        public HttpPostedFileBase image1 { get; set; }
    }
}