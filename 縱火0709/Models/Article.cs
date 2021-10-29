using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace 縱火0709.Models
{
    public class Article
    {
        /// <summary>
        /// 留言文章編號
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        /// <summary>
        /// 留言標題
        /// </summary>
        [Required(ErrorMessage = "標題必填")]
        [Display(Name = "標題:")]
        public string Article_Title { get; set; }

        /// <summary>
        /// 留言發表人
        /// </summary>
        [Display(Name = "留言發表人")]
        public string Article_name { get; set; }

        /// <summary>
        /// 留言板內容
        /// </summary>
        [Display(Name = "留言板內容")]
        public string Article_Content { get; set; }

        /// <summary>
        /// 註記是否顯示
        /// </summary>
        public bool? Sticky { get; set; }

        public virtual ICollection<Reply> Replys { get; set; }

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