using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace 縱火0709.Models
{
    public class Enum
    {
        public enum GenderType
        {
            男,
            女
        }

        /// <summary>
        /// 物理刪除
        /// </summary>
        public enum StickyType
        {
            不顯示,
            顯示,
            刪除_垃圾箱,
            刪除_廢棄箱
        }
    }
}