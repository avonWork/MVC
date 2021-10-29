using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace 縱火0709.Models
{
    public class UserType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("類型編號")]
        public int Id { get; set; }

        [DisplayName("類型名稱")]
        public string TypeName { get; set; }

        /// <summary>
        /// 一對多
        /// </summary>
        [JsonIgnore]
        public virtual ICollection<User> Users { get; set; }

        [JsonIgnore]
        public virtual ICollection<Member> Members { get; set; }
    }
}