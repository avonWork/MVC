using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace 縱火0709.Models
{
    public class Hobbys
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //prop
        public int Id { get; set; }

        public Hobbys()
        {
            this.Users = new HashSet<User>();
        }

        [Required]
        [Display(Name = "興趣名稱")]
        public string HobbyName { get; set; }

        public virtual ICollection<User> Users { get; set; }

        /// <summary>
        /// 新增者
        /// </summary>
        public int? InsertAdminID { get; set; }

        /// <summary>
        /// 新增時間
        /// </summary>
        ///
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",
            ApplyFormatInEditMode = true)]
        public DateTime? InsertTime { get; set; }

        /// <summary>
        /// 編輯者
        /// </summary>
        public int? EditAdminID { get; set; }

        /// <summary>
        /// 編輯時間
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",
            ApplyFormatInEditMode = true)]
        public DateTime? EditTime { get; set; }
    }
}