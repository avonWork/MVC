using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace 縱火0709.Models
{
    public class Premission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "編號")]
        public int Id { get; set; }

        [Display(Name = "名稱")]
        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(20)]
        public string Name { get; set; }

        public int? Pid { get; set; }

        [Display(Name = "父類別")]
        [ForeignKey(("Pid"))]
        public virtual Premission Primissions { get; set; }

        /// <summary>
        /// 一對多
        /// </summary>
        [Display(Name = "子類別")]
        public virtual ICollection<Premission> PremissionsSon { get; set; }

        [Display(Name = "權限代號")]
        public string PValue { get; set; }

        [Display(Name = "連結")]
        [Required(ErrorMessage = "{0}必填")]
        public string Url { get; set; }
    }
}