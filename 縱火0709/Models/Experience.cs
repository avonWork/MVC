using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace 縱火0709.Models
{
    public class Experience
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //prop
        public int Id { get; set; }

        /// <summary>
        /// 會員過去服務單位
        /// </summary>
        [Display(Name = "服務單位:")]
        public string CurrentEmployer { get; set; }

        /// <summary>
        /// 會員過去職稱
        /// </summary>
        [Display(Name = "職　稱：")]
        public string JobTitle { get; set; }

        /// <summary>
        /// 會員工作年(起：)
        /// </summary>
        [Display(Name = "起：")]
        [Range(10, 200, ErrorMessage = "{0}只能在{1}至{2}之间")]
        public string WorkStartYear { get; set; }

        /// <summary>
        /// 會員工作月(起：)
        /// </summary>
        [Display(Name = "起：")]
        [Range(1, 12, ErrorMessage = "{0}只能在{1}至{2}之间")]
        public string WorkStartMonth { get; set; }

        /// <summary>
        /// 會員工作年(迄：)
        /// </summary>
        [Display(Name = "迄：")]
        [Range(10, 200, ErrorMessage = "{0}只能在{1}至{2}之间")]
        public string WorkEndYear { get; set; }

        /// <summary>
        /// 會員工作月(迄：)
        /// </summary>
        [Display(Name = "迄：")]
        [Range(1, 12, ErrorMessage = "{0}只能在{1}至{2}之间")]
        public string WorkEndMonth { get; set; }

        public string MemUserId { get; set; }

        /// <summary>
        /// 會員編號
        /// </summary>
        [ForeignKey("MemUserId ")]
        public virtual Member Member { get; set; }
    }
}