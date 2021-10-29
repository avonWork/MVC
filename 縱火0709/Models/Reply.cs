using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace 縱火0709.Models
{
    public class Reply
    {
        /// <summary>
        /// 回覆編號
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public int ArtID { get; set; }

        [ForeignKey("ArtID")]
        public virtual Article Article { get; set; }

        /// <summary>
        /// 回覆人
        /// </summary>
        [Display(Name = "回覆人")]
        public string Reply_name { get; set; }

        /// <summary>
        /// 回覆內容
        /// </summary>
        [Display(Name = "回覆內容")]
        public string Reply_Content { get; set; }

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
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime AddTime { get; set; }
    }
}