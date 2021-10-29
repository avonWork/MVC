using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace 縱火0709.Models
{
    public class FileClass
    {
        /// <summary>
        /// 檔案編號
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// 檔案名稱
        /// </summary>
        [Required]
        [Display(Name = "檔案名稱")]
        public string fileName { get; set; }

        /// <summary>
        /// 檔案位址
        /// </summary>
        [Display(Name = "檔案位址")]
        public string fileUrl { get; set; }

        /// <summary>
        /// 檔案文字
        /// </summary>
        [Display(Name = "檔案文字")]
        public string fileText { get; set; }

        /// <summary>
        /// 檔案順序
        /// </summary>
        [Display(Name = "檔案順序")]
        public int fileorder { get; set; }

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