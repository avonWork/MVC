using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace 縱火0709.Models
{
    public class ImgClass
    {
        /// <summary>
        /// 圖片編號
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// 圖片名稱
        /// </summary>
        [Display(Name = "圖片名稱")]
        public string imgName { get; set; }

        /// <summary>
        /// 圖片位址
        /// </summary>

        [Display(Name = "圖片位址")]
        public string imgUrl { get; set; }

        /// <summary>
        /// 圖片文字
        /// </summary>
        [Display(Name = "圖片文字")]
        public string imgText { get; set; }

        /// <summary>
        /// 圖片順序
        /// </summary>
        [Display(Name = "圖片順序")]
        public int imgorder { get; set; }

        /// <summary>
        /// 註記是否顯示
        /// </summary>
        public bool? Sticky { get; set; }

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
    }
}